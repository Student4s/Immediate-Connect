using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsPanel : MonoBehaviour
{
    [SerializeField] private Text newstext;

    public void GetNewsText(string text)
    {
        newstext.text = text;
    }
}
