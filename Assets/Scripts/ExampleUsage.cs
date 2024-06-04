using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    private ArticleFetcher articleFetcher;
    public string articleUrl;

    void Start()
    {
        articleFetcher = GetComponent<ArticleFetcher>();
        articleFetcher.FetchArticle(articleUrl);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Article Text: " + articleFetcher.articleText);
            if (articleFetcher.articleImage != null)
            {
                Debug.Log("Article Image is downloaded and available as Texture2D");
                // ������ ������������� �����������: �������� Sprite � ����������� �� UI
                Sprite articleSprite = Sprite.Create(articleFetcher.articleImage, new Rect(0, 0, articleFetcher.articleImage.width, articleFetcher.articleImage.height), new Vector2(0.5f, 0.5f));
                // �������������� ��� ��� ����������� Sprite �� UI (��������, Image component)
            }
            else
            {
                Debug.Log("Article Image not available");
            }
        }
    }
}
