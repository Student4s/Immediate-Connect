using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EventBlocks : MonoBehaviour
{
    public Text eventName;
    public Text eventMonth;
    public Text eventDay;
    public Text eventYear;
    public string eventLink;
    public string eventDescription;
    [SerializeField] private DescriptionPanel panel;

    [SerializeField] private ImageFetcher imageFetcher;

    private void Start()
    {
        panel = Resources.FindObjectsOfTypeAll<DescriptionPanel>()[0];
    }

    private void OnEnable()
    {
        StartCoroutine(FetchText(eventLink));
    }
    public void GetInfo(string name, string month, string day, string year, string link, string description)
    {
        eventName.text = name;
        eventMonth.text = month;
        eventDay.text = day;
        eventYear.text = year;
        eventDescription = description;
        eventLink = link;
        //eventDescription = StringConverter.Convert(eventDescription);
        imageFetcher.url = eventLink;
        imageFetcher.GetImage();


    }


    public void AdditionalInfo()
    {
        panel.gameObject.SetActive(true);
        panel.description.text = eventDescription;
    }

    IEnumerator FetchText(string pageUrl)
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

        // Ищем текст в блоке <div class="post_text">
        var postTextNode = doc.DocumentNode.SelectSingleNode("//div[@class='post_text']");
        if (postTextNode != null)
        {
            string postText = postTextNode.InnerText;

            // Удаляем весь текст в угловых скобках <>
            postText = Regex.Replace(postText, "<.*?>", string.Empty);

            // Удаляем весь текст после "Booking.com"
            int index = postText.IndexOf("Booking.com");
            if (index >= 0)
            {
                postText = postText.Substring(0, index);
            }

            // Сохраняем текст в переменную
            eventDescription = postText;
            // Debug.Log("Post Text: " + fetchedText);
        }
        else
        {
            Debug.LogError("post_text block not found");
        }
    }
}
