using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.ComponentModel;
using UnityEngine.UI;
using System.Net.Http;

public class ArticleFetcher2 : MonoBehaviour
{
    public string url; // URL of the news article
    public Image imageComponent; // Image component to display the news image
    public string textComponent; // Text component to display the news text
    public Text article;
    [SerializeField] private NewsPanel newsPanel;// панелька, куда фулл текст будет закинут. 
    // Method to start fetching the news details
    public void FetchNews()
    {
        FetchNewsDetails(url);
    }
    private void Start()
    {
        FetchNewsDetails(url);
        newsPanel = Resources.FindObjectsOfTypeAll<NewsPanel>()[0];
    }
    public void News()// перекидываем текст на панельку.
    {
        newsPanel.GetNewsText(textComponent);
        newsPanel.gameObject.SetActive(true);
    }

    private async void FetchNewsDetails(string url)
    {
        HttpClient client = new HttpClient();

        // Adding User-Agent header to mimic a web browser
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string pageContent = await response.Content.ReadAsStringAsync();
            ParseHtml(pageContent);
        }
        else
        {
            Debug.LogError("Error fetching news: " + response.ReasonPhrase);
        }
    }

    private IEnumerator LoadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                imageComponent.sprite = sprite;
            }
        }
    }

    private void ParseHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Extracting the news image URL
        var newsItemNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'news-item detail content_text')]");
        if (newsItemNode != null)
        {
            string imageUrl = newsItemNode.GetAttributeValue("data-image", "");
            if (!string.IsNullOrEmpty(imageUrl))
            {
                StartCoroutine(LoadImage(imageUrl));
            }
            else
            {
                Debug.LogError("News image URL not found.");
            }

            // Extracting the news text
            var contentNode = newsItemNode.SelectSingleNode(".//div[@class='content']");
            if (contentNode != null)
            {
                string newsText = contentNode.InnerText;
                string cleanedText = CleanText(newsText);
                textComponent = cleanedText;
            }
            else
            {
                Debug.LogError("News text not found.");
            }
        }
        else
        {
            Debug.LogError("News item block not found.");
        }
    }

    private string CleanText(string input)
    {

        input = System.Text.RegularExpressions.Regex.Replace(input, "<!--.*?-->", "", System.Text.RegularExpressions.RegexOptions.Singleline);
        input = input.Replace("Back to the list", "");

        input = System.Text.RegularExpressions.Regex.Replace(input, @"(\r?\n\s*){2,}", "\n\n");

        int endContentIndex = input.IndexOf("<!-- end-content -->");
        if (endContentIndex != -1)
        {
            input = input.Substring(0, endContentIndex);
        }

        return input;
    }
}

