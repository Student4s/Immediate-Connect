using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using System.Linq;

public class NewsFetcher : MonoBehaviour
{
    [SerializeField] private string newsUrl = "https://crypto.news/news/";
    public List<string> newsLinks = new List<string>(); //  ������ ������
    public List<string> newsTitles = new List<string>(); //  ������ ����������
    [SerializeField] private int maxLinks;

    [SerializeField] private ArticleFetcher article;
    [SerializeField] private List<ArticleFetcher> articlesList = new List<ArticleFetcher>();
    [SerializeField] private GameObject ScrollPanel;

    private void Awake()
    {
        FetchNews();
    }

    private void Start()
    {
        StartCoroutine("Crutch", 0.2f);
    }

    public void SpawnNews(bool B)// ������� ����� � ���������.
    {
        if(B)
        {
            for (int i = 0; i < newsTitles.Count; i++)
            {
                var A = Instantiate(article, ScrollPanel.transform);
                A.FetchArticle(newsLinks[i]);
                A.article.text = newsTitles[i];
                articlesList.Add(A);
            }
        }
        
    }

    public void DespawnNews(bool B)// ��������� ����� � ���������.
    {
        if (!B)
        {
            foreach (ArticleFetcher obj in articlesList)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            articlesList.Clear();
        }
       
    }
    
    public void FetchNews()
    {
        StartCoroutine(GetNewsLinks(newsUrl));
    }

    
    private IEnumerator GetNewsLinks(string url)// ��������� HTML ��������
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

    private void ParseNewsLinks(string htmlContent)// ����� ��� �������� HTML � ��������� ������ + ����������
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var nodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'post-loop__link')]");// "post-loop__link" - ��� ������� �����

        newsLinks.Clear();
        newsTitles.Clear();

        if (nodes != null)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (count >= maxLinks) break; // ����������� �� ���������� ������
                string link = node.GetAttributeValue("href", "");
                string title = node.InnerText.Trim();
                newsLinks.Add(link);
                newsTitles.Add(title);
                count++;
            }
        }
    }
}