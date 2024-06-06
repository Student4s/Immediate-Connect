using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.Networking;

public class EventsFetcher : MonoBehaviour
{
    public List<string> eventTitles = new List<string>();
    public List<string> eventDates = new List<string>();
    public List<Sprite> eventImages = new List<Sprite>();
    public List<string> eventLinks = new List<string>();

    [SerializeField] private SpawnEventBlocks spawner;
    void Start()
    {
        string url = "https://crypto.news/events/";
        FetchEventData(url);

        StartCoroutine("Spawn", 1f);
    }

    void Spawn()
    {
        for (int i = 0; i < eventTitles.Count; i++)
        {
            spawner.AddBlock(eventTitles[i], eventDates[i], eventLinks[i]);
        }
    }

    private void FetchEventData(string url)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;
        string pageContent = response.Content.ReadAsStringAsync().Result;

        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(pageContent);

        var eventNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'event-card')]");
        if (eventNodes != null)
        {
            foreach (var eventNode in eventNodes)
            {
                // Extract event title
                var titleNode = eventNode.SelectSingleNode(".//p[contains(@class, 'event-card__title')]");
                string eventTitle = titleNode?.InnerText.Trim();
                if (!string.IsNullOrEmpty(eventTitle) && !eventTitles.Contains(eventTitle))
                {
                    eventTitles.Add(eventTitle);
                }

                // Extract event dates
                var dateNodes = eventNode.SelectNodes(".//p[contains(@class, 'event-card__duration')]/time");
                if (dateNodes != null && dateNodes.Count == 2)
                {
                    string startDate = dateNodes[0].Attributes["datetime"].Value;
                    string endDate = dateNodes[1].Attributes["datetime"].Value;
                    string eventDate = $"{startDate} - {endDate}";

                    if (!string.IsNullOrEmpty(eventDate) && !eventDates.Contains(eventDate))
                    {
                        eventDates.Add(eventDate);
                    }
                }

                // Extract event image and link
                var imageNode = eventNode.SelectSingleNode(".//a[contains(@class, 'event-card__link')]");
                string eventLink = imageNode?.Attributes["href"].Value;
                if (!string.IsNullOrEmpty(eventLink) && !eventLinks.Contains(eventLink))
                {
                    eventLinks.Add(eventLink);
                }

                var imgNode = eventNode.SelectSingleNode(".//img[contains(@class, 'event-card__image')]");
                string imageUrl = imgNode?.Attributes["src"].Value;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    StartCoroutine(LoadImageFromUrl(imageUrl));
                }
            }
        }
    }

    private IEnumerator LoadImageFromUrl(string imageUrl)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading image: " + uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                eventImages.Add(sprite);
            }
        }
    }
}