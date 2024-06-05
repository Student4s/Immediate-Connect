using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRectangle : MonoBehaviour
{
    [SerializeField] private Text currencyName;
    [SerializeField] private Text price;
    
    public void UpdateInfo(string name, string currencyPrice)
    {
        currencyName.text = name;
        price.text = currencyPrice;
    }
}
