using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEventBlocks : MonoBehaviour
{
    [SerializeField] private EventBlocks block;


    public void AddBlock(string name, string time, string link)
    {
        var a = Instantiate(block, gameObject.transform);
        a.GetInfo(name, time, link);
    }
}
