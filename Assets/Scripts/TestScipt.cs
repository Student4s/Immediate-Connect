using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScipt : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.SaveTime();
    }
    public void TestFunc1()
    {
        DateTime? loadedTime = SaveManager.Instance.LoadTime();
        if (loadedTime.HasValue)
        {
            Debug.Log("Loaded Time: " + loadedTime.Value.ToString("o"));
        }
        else
        {
            Debug.Log("No saved time found.");
        }
    }

    public void TestFunc2()
    {

    }
}
