using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class CryptoPriceFetcher : MonoBehaviour
{
    public List<string> cryptoNames = new List<string>();
    public List<float> cryptoPrices = new List<float>();
    public List<float> oldCryptoPrices;// цены от которых отталкиваемся для просчета разницы

    [SerializeField] private List<PriceRectangle> priceRectangles;//прямоугольники с инфой
    [SerializeField] private TopRectangle topRectangel;// верхний прямоугольник с любимой криптой

    private readonly List<string> cryptoIds = new List<string>// Список криптовалют
    {
        "bitcoin", "ethereum", "binancecoin", "tether", "cardano",
        "dogecoin", "ripple", "polkadot", "usd-coin", "uniswap"
    };
    private void Awake()
    {
        FetchCryptoPrices();
    }
    private void Start()
    {
        SetPrices();
    }
    public void FetchCryptoPrices()
    {
        StartCoroutine(GetCryptoPrices());
    }

    private IEnumerator GetCryptoPrices()// Корутина для получения цен криптовалют с API CoinGecko
    {
        string ids = string.Join(",", cryptoIds);
        string url = $"https://api.coingecko.com/api/v3/simple/price?ids={ids}&vs_currencies=usd";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ParseCryptoPrices(jsonResponse);
                for (int i = 0; i < cryptoNames.Count; i++)
                {
                    Debug.Log($"{cryptoNames[i]}: ${cryptoPrices[i]}");
                }
            }
        }
    }
     void ParseCryptoPrices(string jsonResponse)    // Метод для парсинга JSON-ответа и извлечения цен криптовалют
    {
        JObject json = JObject.Parse(jsonResponse);
        cryptoNames.Clear();
        cryptoPrices.Clear();

        foreach (var id in cryptoIds)
        {
            cryptoNames.Add(id);
            cryptoPrices.Add(json[id]["usd"].Value<float>());
        }
    }

    void SetPrices()
    {
        for(int i=0;i<cryptoNames.Count;i++)
        {
            priceRectangles[i].UpdateInfo(cryptoPrices[i], oldCryptoPrices[i]);
        }
    }

   public void FavoriteCurrency(int index)
    {
        for (int i = 0; i < cryptoNames.Count; i++)
        {
            if(i!=index)
            {
                priceRectangles[i].starActive.gameObject.SetActive(false);
            }
            else
            {
                priceRectangles[i].ActiveStar();
                topRectangel.UpdateInfo(priceRectangles[i].gameObject.name, priceRectangles[i].currencyPrice.text);
            }
        }
    }
}