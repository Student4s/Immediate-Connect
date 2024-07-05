using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{
    public string url;
    public List<string> Header;
    public List<string> text;
    public List<Image> image;

    private void Start()
    {
        FetchNews();
    }
    public void FetchNews()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        while (!request.isDone)
        {
            // ∆дем завершени€ запроса
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string html = request.downloadHandler.text;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var eventBlocks = doc.DocumentNode.SelectNodes("//div[contains(@class, 'structural-patterns')]");// выбираем нужный блок

            if(eventBlocks != null )
            {
                foreach (var eventBlock in eventBlocks)
                {
                    var header = eventBlock.SelectSingleNode(".//span[@class='pattern-name']");
                    var texts = eventBlock.SelectSingleNode(".//span[@class='pattern-aka']");
                    //   var image = eventBlock.SelectSingleNode(".//span[@class='pattern-image']");

                    if (header != null) Header.Add(header.InnerText.Trim());
                    if (texts != null) text.Add(texts.InnerText.Trim());
                }
                    
            }
            else
            {
                Debug.Log("Nothing fetched");
            }
        }
    }
}
