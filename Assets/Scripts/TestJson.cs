using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using StreamReader;

public class TestJson : MonoBehaviour
// This class allows me to extract data from a JSON file, it parses block.json into an array of block
{
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        Blocks blocksInJson = JsonUtility.FromJson<Blocks>(jsonFile.text);
        foreach (Block block in blocksInJson.blocks)
        {
            //Debug.Log("Found employee: " + block.ID + " " + block.Path);
        }   
    }

}
