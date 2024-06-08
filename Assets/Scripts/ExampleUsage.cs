using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class ExampleUsage : MonoBehaviour
{
    string[] cryptocurrencies = { "bitcoin", "ethereum", "binancecoin", "tether", "cardano", "dogecoin", "ripple", "polkadot", "usd-coin", "uniswap" };

    // Адрес API CoinGecko
    string apiUrl = "https://api.coingecko.com/api/v3/simple/price?ids={0}&vs_currencies=usd&date={1}-01-01";

    void Start()
    {
        StartCoroutine(FetchCryptoPrices());
    }

    IEnumerator FetchCryptoPrices()
    {
        // Для каждой криптовалюты делаем запрос к API CoinGecko
        foreach (string crypto in cryptocurrencies)
        {
            string url = string.Format(apiUrl, crypto, 2024);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // Получаем ответ в формате JSON
                    string jsonResponse = webRequest.downloadHandler.text;
                    // Разбираем JSON для получения цены криптовалюты
                    float price = ParseCryptoPrice(jsonResponse, crypto);
                    Debug.Log("Price of " + crypto + " on January 1, " + 2024 + ": $" + price);
                }
                else
                {
                    Debug.Log("Error fetching data: " + webRequest.error);
                }
            }
        }
    }

    float ParseCryptoPrice(string jsonResponse, string crypto)
    {
        // Разбираем JSON и извлекаем цену криптовалюты
        try
        {
            // Преобразуем JSON в объект
            JObject jsonObject = new JObject(jsonResponse);
            // Получаем цену криптовалюты
            float price = jsonObject[crypto]["usd"].Value<float>();
            return price;
        }
        catch (Exception e)
        {
            Debug.Log("Error parsing JSON: " + e.Message);
            return 0f;
        }
    }
}
