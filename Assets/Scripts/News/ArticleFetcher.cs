using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.ComponentModel;
using UnityEngine.UI;

public class ArticleFetcher : MonoBehaviour
{
    public string articleText;// текст
    public Text article;// заголовок

    public Texture2D articleImage; 
    private string articleImageUrl;

    [SerializeField] private NewsPanel newsPanel;// панелька, куда фулл текст будет закинут. 

    public void FetchArticle(string url)
    {
        StartCoroutine(GetArticleContent(url));
    }
    private IEnumerator GetArticleContent(string url)// Корутина для получения HTML страницы
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string htmlContent = webRequest.downloadHandler.text;
                ParseArticleContent(htmlContent);
                Debug.Log("Article Text: " + articleText);

                if (!string.IsNullOrEmpty(articleImageUrl))
                {
                    StartCoroutine(DownloadImage(articleImageUrl));
                }
            }
        }
    }
    private void ParseArticleContent(string htmlContent)// Метод для парсинга HTML и извлечения текста и изображения статьи
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Ищем <div> с классом "post-detail__content blocks" для текста статьи
        var textNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'post-detail__content blocks')]");
        articleText = textNode != null ? StringConverter.Convert(textNode.InnerText.Trim()) : "Text not found";

        // Ищем <img> с классом "post-detail__image wp-post-image" для изображения
        var imgNode = doc.DocumentNode.SelectSingleNode("//img[contains(@class, 'post-detail__image wp-post-image')]");
        articleImageUrl = imgNode != null ? imgNode.GetAttributeValue("src", "") : "";
    }

    
    private IEnumerator DownloadImage(string imageUrl)// Корутина для загрузки изображения
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + webRequest.error);
            }
            else
            {
                articleImage = DownloadHandlerTexture.GetContent(webRequest);
                Debug.Log("Image downloaded successfully");
            }
        }
    }

    public void News()// перекидываем текст на панельку.
    {
        newsPanel.GetNewsText(articleText);
    }
}