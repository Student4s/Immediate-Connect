using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEventBlocks : MonoBehaviour
{
    [SerializeField] private EventBlocks[] blocks;
    private int currentBlock=0;

    public void AddBlock(string name, string month, string day, string year, string link, string description)
    {
        blocks[currentBlock].GetInfo(name, month, day, year, link, description);
        currentBlock += 1;
    }
}
