using System.Net.Http;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.UI;

public class EventBlocks : MonoBehaviour
{
    public Text eventName;
    public Text eventtime;
    public Image eventImage;

    public string eventLink;
    [SerializeField] private string eventDescription;
    [SerializeField] private DescriptionPanel panel;

    private void Start()
    {
        panel = Resources.FindObjectsOfTypeAll<DescriptionPanel>()[0];
    }
    public void GetInfo(string name, string time, string link)
    {
        eventName.text = name;
        eventtime.text = time;
        eventLink = link;

        FetchEventDetailText(eventLink);
        eventDescription = StringConverter.Convert(eventDescription);
    }

    public void FetchEventDetailText(string url)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;
        string pageContent = response.Content.ReadAsStringAsync().Result;

        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(pageContent);

        var detailNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'event-detail__content blocks')]");
        if (detailNode != null)
        {
            eventDescription = detailNode.InnerText.Trim();
        }
        else
        {
            Debug.LogError("Event detail content not found.");
        }
    }

    public void AdditionalInfo()
    {
        panel.gameObject.SetActive(true);
        panel.description.text = eventDescription;
    }
}
