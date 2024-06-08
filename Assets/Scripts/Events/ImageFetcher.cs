using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using UnityEngine.UI;

public class ImageFetcher : MonoBehaviour
{
    public string url; // —сылка на сайт
    public Image image; // UI элемент дл€ отображени€ картинки
    private void Start()
    {
        GetImage();
    }
    public void GetImage()
    {
        StartCoroutine(FetchImage(url));
    }
    IEnumerator FetchImage(string pageUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(pageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching page: " + request.error);
            yield break;
        }

        string html = request.downloadHandler.text;
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        // »щем первую картинку в блоке <div class="post_text">
        var postTextNode = doc.DocumentNode.SelectSingleNode("//div[@class='post_text']");
        if (postTextNode != null)
        {
            var imgNode = postTextNode.SelectSingleNode(".//img");
            if (imgNode != null)
            {
                string imgUrl = imgNode.GetAttributeValue("src", "");
                Debug.Log("Image URL: " + imgUrl);
                StartCoroutine(DownloadImage(imgUrl));
            }
            else
            {
                Debug.LogError("Image not found in the post_text block");
            }
        }
        else
        {
            Debug.LogError("post_text block not found");
        }
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading image: " + request.error);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        image.SetNativeSize();
    }
}
