using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceRectangle : MonoBehaviour
{
    public int currencyIndex;
    public Text currencyPrice;
    public Text percent;
    public Image starActive;
    public Image starNotActive;
  
    [SerializeField] private Image vectorDown;
    [SerializeField] private Image vectorUp;


    public void UpdateInfo(float price, float oldPrice)
    {
        currencyPrice.text = price.ToString()+"$";
        vectorDown.gameObject.SetActive(false);
        vectorUp.gameObject.SetActive(false);

        if(price> oldPrice)
        {
            vectorUp.gameObject.SetActive(true);
        }
        else
        {
            vectorDown.gameObject.SetActive(true);
        }

        if (oldPrice != 0)
        {
            float percentageChange = ((price - oldPrice) / oldPrice) * 100;
            percent.text = $"{percentageChange:F1}%";
        }
    }
    public void ActiveStar()
    {
        starActive.gameObject.SetActive(true);
    }
}
