using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.ComponentModel;
using UnityEngine.UI;

public class ArticleFetcher : MonoBehaviour
{
    public string articleText;// �����
    public Text article;// ���������

    public Texture2D articleImage; 
    private string articleImageUrl;

    [SerializeField] private NewsPanel newsPanel;// ��������, ���� ���� ����� ����� �������. 

    public void FetchArticle(string url)
    {
        StartCoroutine(GetArticleContent(url));
    }
    private IEnumerator GetArticleContent(string url)// �������� ��� ��������� HTML ��������
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
    private void ParseArticleContent(string htmlContent)// ����� ��� �������� HTML � ���������� ������ � ����������� ������
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // ���� <div> � ������� "post-detail__content blocks" ��� ������ ������
        var textNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'post-detail__content blocks')]");
        articleText = textNode != null ? StringConverter.Convert(textNode.InnerText.Trim()) : "Text not found";

        // ���� <img> � ������� "post-detail__image wp-post-image" ��� �����������
        var imgNode = doc.DocumentNode.SelectSingleNode("//img[contains(@class, 'post-detail__image wp-post-image')]");
        articleImageUrl = imgNode != null ? imgNode.GetAttributeValue("src", "") : "";
    }

    
    private IEnumerator DownloadImage(string imageUrl)// �������� ��� �������� �����������
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

    public void News()// ������������ ����� �� ��������.
    {
        newsPanel.GetNewsText(articleText);
    }
}