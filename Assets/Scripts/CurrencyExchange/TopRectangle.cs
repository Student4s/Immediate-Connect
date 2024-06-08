using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRectangle : MonoBehaviour
{
    [SerializeField] private Text currencyName;
    [SerializeField] private Text price;
    [SerializeField] private PreferencesManager mamger;
    
    public void UpdateInfo(string name, string currencyPrice)
    {
        currencyName.text = name;
        price.text = currencyPrice;

        mamger.SetFavoriteCurrency(currencyName.text, price.text);
    }
}
