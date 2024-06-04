using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.Linq;

public class NewsFetcher : MonoBehaviour
{
    public string newsUrl = "https://crypto.news/news/";
    public List<string> newsLinks = new List<string>(); // Публичный список ссылок
    public List<string> newsTitles = new List<string>(); // Публичный список заголовков
    [SerializeField] private int maxLinks;

    private void Start()
    {
        FetchNews();
    }

    public void FetchNews()
    {
        StartCoroutine(GetNewsLinks(newsUrl));
    }

    
    private IEnumerator GetNewsLinks(string url)// получение HTML страницы
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string htmlContent = webRequest.downloadHandler.text;
                ParseNewsLinks(htmlContent);
                for (int i = 0; i < newsLinks.Count; i++)
                {
                    Debug.Log("News Link: " + newsLinks[i] + " | Title: " + newsTitles[i]);
                }
            }
        }
    }

    private void ParseNewsLinks(string htmlContent)// Метод для парсинга HTML и получения ссылок + заголовков
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var nodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'post-loop__link')]");// "post-loop__link" - для первого сайта

        newsLinks.Clear();
        newsTitles.Clear();

        if (nodes != null)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (count >= maxLinks) break; // ограничение на количество ссылок
                string link = node.GetAttributeValue("href", "");
                string title = node.InnerText.Trim();
                newsLinks.Add(link);
                newsTitles.Add(title);
                count++;
            }
        }
    }
}