using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.Networking;

public class EventsFetcher : MonoBehaviour
{
    public string url = "https://cryptoevents.global/";
    public List<string> eventNames = new List<string>();
    public List<string> eventMonths = new List<string>();
    public List<string> eventDays = new List<string>();
    public List<string> eventYears = new List<string>();
    public List<string> eventLinks = new List<string>();
    public List<string> eventDescriptions = new List<string>();

    public SpawnEventBlocks spawner;
    void Start()
    {
        FetchEvents();
        SpawnBlocks();
        
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < 5; i++)
        {
            spawner.AddBlock(eventNames[i], eventMonths[i], eventDays[i], eventYears[i], eventLinks[i], eventDescriptions[i]);
        }

    }
    public void FetchEvents()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();

        while (!request.isDone)
        {
            // Ждем завершения запроса
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching events: " + request.error);
        }
        else
        {
            string html = request.downloadHandler.text;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var eventBlocks = doc.DocumentNode.SelectNodes("//div[contains(@class, 'event_blk')]");

            if (eventBlocks != null)
            {
                foreach (var eventBlock in eventBlocks)
                {
                    var eventNameNode = eventBlock.SelectSingleNode(".//p[@class='event_name']");
                    var eventMonthNode = eventBlock.SelectSingleNode(".//span[@class='event_month has_range']");
                    var eventDayNode = eventBlock.SelectSingleNode(".//span[@class='event_day has_range']");
                    var eventYearNode = eventBlock.SelectSingleNode(".//span[@class='event_year']");
                    var eventLinkNode = eventBlock.SelectSingleNode(".//a[contains(@href, 'https://cryptoevents.global')]");
                    var eventDescriptionNode = eventBlock.SelectSingleNode(".//div[@class='event_short_description']");

                    if (eventNameNode != null) eventNames.Add(eventNameNode.InnerText.Trim());
                    if (eventMonthNode != null) eventMonths.Add(eventMonthNode.InnerText.Trim());
                    if (eventDayNode != null) eventDays.Add(eventDayNode.InnerText.Trim());
                    if (eventYearNode != null) eventYears.Add(eventYearNode.InnerText.Trim());
                    if (eventLinkNode != null) eventLinks.Add(eventLinkNode.GetAttributeValue("href", "").Trim());
                    if (eventDescriptionNode != null) eventDescriptions.Add(eventDescriptionNode.InnerText.Trim());
                }
            }
        }
    }
}
