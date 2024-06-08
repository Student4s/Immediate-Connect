using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesManager : MonoBehaviour
{
    [SerializeField] private const string FavoriteCurrencyKey = "FavoriteCurrency";
    [SerializeField] private const string FavoriteCurrencyPrice = "FavoriteCurrencyPrice";
    [SerializeField] private const string Toggle1Key = "Toggle1";
    [SerializeField] private const string Toggle2Key = "Toggle2";
    [SerializeField] private const string Toggle3Key = "Toggle3";

    public string FavoriteCurrencyN { get; private set; }
    public string FavoriteCurrencyP { get; private set; }
    public bool Toggle1 { get; private set; }
    public bool Toggle2 { get; private set; }
    public bool Toggle3 { get; private set; }

    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public TopRectangle top;
    public NewsFetcher1 ev1;
    public NewsFetcher2 ev2;
    public NewsFetcher3 ev3;

    void Start()
    {
        LoadPreferences();

        toggle1.isOn = Toggle1;
        toggle2.isOn = Toggle2;
        toggle3.isOn = Toggle3;

        if (toggle1.isOn)
        {
            ev1.SpawnNews(true);
        }
        if (toggle2.isOn)
        {
            ev2.SpawnNews(true);
        }
        if (toggle3.isOn)
        {
            ev3.SpawnNews(true);
        }

      top.UpdateInfo(FavoriteCurrencyN, FavoriteCurrencyP);
    }

    public void SetFavoriteCurrency(string currencyName, string price)
    {
        FavoriteCurrencyN = currencyName;
        FavoriteCurrencyP = currencyName;
        PlayerPrefs.SetString(FavoriteCurrencyKey, currencyName);
        PlayerPrefs.SetString(FavoriteCurrencyPrice, price);
        PlayerPrefs.Save();
    }

    public void SetToggle1(bool state)
    {
        Toggle1 = state;
        PlayerPrefs.SetInt(Toggle1Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetToggle2(bool state)
    {
        Toggle2 = state;
        PlayerPrefs.SetInt(Toggle2Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetToggle3(bool state)
    {
        Toggle3 = state;
        PlayerPrefs.SetInt(Toggle3Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Toggle1Changed(Toggle toggle)
    {
        SetToggle1(toggle.isOn);
    }

    public void Toggle2Changed(Toggle toggle)
    {
        SetToggle2(toggle.isOn);
    }

    public void Toggle3Changed(Toggle toggle)
    {
        SetToggle3(toggle.isOn);
    }

    private void LoadPreferences()
    {
        if (PlayerPrefs.HasKey(FavoriteCurrencyKey))
        {
            FavoriteCurrencyN = PlayerPrefs.GetString(FavoriteCurrencyKey);
            FavoriteCurrencyP = PlayerPrefs.GetString(FavoriteCurrencyPrice);
        }
        else
        {
            FavoriteCurrencyN = ""; // Default value
            FavoriteCurrencyP = ""; // Default value
        }

        Toggle1 = PlayerPrefs.GetInt(Toggle1Key, 0) == 1;
        Toggle2 = PlayerPrefs.GetInt(Toggle2Key, 0) == 1;
        Toggle3 = PlayerPrefs.GetInt(Toggle3Key, 0) == 1;
    }

    public void ResetPreferences()
    {
        PlayerPrefs.DeleteKey(FavoriteCurrencyKey);
        PlayerPrefs.DeleteKey(Toggle1Key);
        PlayerPrefs.DeleteKey(Toggle2Key);
        PlayerPrefs.DeleteKey(Toggle3Key);
        PlayerPrefs.Save();

        LoadPreferences();
    }
}
