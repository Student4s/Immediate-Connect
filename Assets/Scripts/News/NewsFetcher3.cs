using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.Linq;
using System.Net.Http;

public class NewsFetcher3 : MonoBehaviour
{
    [SerializeField] private string newsUrl;
    [SerializeField] private int maxLinks = 8; // максимум ссылок
    public List<string> newsTitles = new List<string>();
    public List<string> newsLinks = new List<string>();

    [SerializeField] private ArticleFetcher2 article;
    [SerializeField] private List<ArticleFetcher2> articlesList = new List<ArticleFetcher2>();
    [SerializeField] private GameObject ScrollPanel;

    private void Awake()
    {
        FetchNews(newsUrl);
        newsLinks.RemoveRange(0, maxLinks);
        newsTitles.RemoveRange(0, maxLinks);
    }

    public void SpawnNews(bool B)// спауним блоки с новостями.
    {
        if (B)
        {
            for (int i = 0; i < newsTitles.Count; i++)
            {
                ArticleFetcher2 A = Instantiate(article, ScrollPanel.transform);
                A.url = newsLinks[i];
                A.article.text = newsTitles[i];
                articlesList.Add(A);
            }
        }

    }
    public void DespawnNews(bool B)// деспауним блоки с новостями.
    {
        if (!B)
        {
            for (int i = 0; i < articlesList.Count; i++)
            {
                Debug.Log(i);
                Destroy(articlesList[i].gameObject);
            }
            articlesList.Clear();
        }
    }


    private void FetchNews(string url)
    {
        HttpClient client = new HttpClient();

        // Adding User-Agent header to mimic a web browser
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");// иногда без этого не пускало

        HttpResponseMessage response = client.GetAsync(url).Result;

        if (response.IsSuccessStatusCode)
        {
            string pageContent = response.Content.ReadAsStringAsync().Result;
            ParseHtml(pageContent);
        }
        else
        {
            Debug.LogError("Error fetching news: " + response.ReasonPhrase);
        }
    }

    private void ParseHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        var newsItems = doc.DocumentNode.SelectNodes("//a[@class='title']");

        if (newsItems != null)
        {
            int count = 0;

            foreach (var item in newsItems)
            {
                // Break the loop if we have reached the maximum number of links
                if (count >= maxLinks*2)
                {
                    break;
                }

                // Get title
                string title = item.InnerText.Trim();

                // Get link
                string link = item.GetAttributeValue("href", "No link");

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(link))
                {
                    // Ensure unique titles
                    if (!newsTitles.Contains(title))
                    {
                        newsTitles.Add(title);
                        newsLinks.Add("https://cryptonews.net" + link);
                        count++;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No news items found.");
        }
    }
}
