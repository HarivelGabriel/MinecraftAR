using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class New_Block : MonoBehaviour
{
    public GameObject newBlockTarget;
    // This Object is the target of the newBlock. It appears in the middle of the target except if
    // the target is in the worldTarget. Then the block appears in a block slot
    private GameObject newBlock;
    // This Object is instantiated in Start(), it is a block that follows block Target
    // it shows to the user where a block will be placed.
    private bool wasInWorld;
    // This variable help to display or not the UI
    public GameObject worldTarget;
    // Target of the minecraft world (where the world is generated 
    public GameObject prefab;
    // prefab object of a block without texture
    public Button nextTextureButton;
    public Button prevTextureButton;
    public Button saveBlockButton;
    private Material[] textures;
    // This is a list containing every textures as Material (currently new block can only be 1 texture block)
    private int currentTexture;
    // Represent the index of the current texture.
    // For exemple : Index 0 represents the first texture in the folder Normal_Texture (acacia_leaves)
    //               Index 1 represents the first texture in the folder Normal_Texture (acacia_planks)
    [HideInInspector]
    public List<int> worldList;
    [HideInInspector]
    public List<int> topology;
    //private Vector3 worldTargetPos;


    // Start is called before the first frame update
    void Start()
    {
        newBlock = Instantiate(prefab, newBlockTarget.transform.position, Quaternion.identity);
        DesactivateButtons();
        wasInWorld = false;
        textures = Resources.LoadAll("Normal_Texture/Materials", typeof(Material)).Cast<Material>().ToArray();
        
        // Set newBlock Texture to 0 and apply it
        currentTexture = 1;
        OnButtonPrevPressed();

        // Create worldList then topology
        worldList = new List<int>();
        foreach (string line in System.IO.File.ReadLines(@"Assets/Resources/chunkfile.txt"))
        {
            if (line.Split(';').ToList()[3] == "air") { worldList.Add((int)0); }
            else { worldList.Add((int)1);}
        }
        topology = CreateTopo();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInWorld(worldTarget.transform.position, newBlockTarget.transform.position))
        {
            TranslateBlock(newBlockTarget.transform.position, newBlock.transform.position);
            ActivateButtons();
            wasInWorld = true;
        }
        else if (wasInWorld)
        {
            DesactivateButtons();
            wasInWorld = false;
        }
    }

    bool IsInWorld(Vector3 worldPos, Vector3 blockPos)
    // return true if the newBlock target is in minecraft world, and false if not
    {
        return (blockPos.x > worldPos.x - 8.5f && blockPos.x < worldPos.x + 7.5f && blockPos.z > worldPos.z - 8.5f && blockPos.z < worldPos.z + 7.5f);
    }

    void TranslateBlock(Vector3 blockTargetPos, Vector3 blockPos)
    // Change position of newBlock so that it fits an available block slot
    {
        Vector3 worldPos = worldTarget.transform.position;
        float xIndex =(float)Math.Round(blockTargetPos.x - worldPos.x);
        float zIndex =(float)Math.Round(blockTargetPos.z - worldPos.z);
        
        newBlock.transform.position = new Vector3(
            worldPos.x + xIndex,
            worldPos.y + (float)topology[((int) zIndex + 8) + ((int)xIndex + 8) * 16],
            worldPos.z + zIndex
            );
    }

    List<int> CreateTopo()
    // Create a List of 256 int which contains the Yindex of the first "air block" 
    // inside the minecraft world from worldList
    {
        int tmp = 15;
        List<int> topo = new List<int>();
        for (int zz = 0; zz <= 15; zz++)
        {
            for (int xx = 0; xx <= 15; xx++)
            {
                tmp = CalculateTopo(worldList, xx, zz);
                topo.Add(tmp);
            }
        }
        return topo;
    }

    int CalculateTopo(List<int> list, int x, int z)
    // return Y index of the first "air block" inside the minecraft world at given {x,z} 
    // from the worldList
        {
            int maxTemp = 15;
            for (int yy = 15; yy >= 0; yy--)
                {
                    if (list[x + 16 * z + 256 * yy] == 0)
                    {
                        maxTemp = yy;
                    }
                }
            return maxTemp;
        }

    public void OnButtonNextPressed(GameObject target)
    // Change texture of the newBlock to the next one in the Normal_Texture folder
    {
        if (currentTexture < textures.Count() - 1)
        {
            currentTexture++;
        }
        else
        {
            currentTexture = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            newBlock.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = textures[currentTexture];
        }
    }

    public void OnButtonPrevPressed()
    // Change texture of the newBlock to the previous one in the Normal_Texture folder
    {
        if (currentTexture > 0)
        {
            currentTexture--;
        }
        else
        {
            currentTexture = 206;
        }
        for (int i = 0; i < 6; i++)
        {
            newBlock.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = textures[currentTexture];
        }
    }

    public void OnButtonSavePressed()
    // Instantiate the new block in the scene, then actualize y position for the next block 
    {
        Instantiate(newBlock, newBlock.transform.position, Quaternion.identity);
        int xIndex =(int)Math.Round(newBlockTarget.transform.position.x - worldTarget.transform.position.x) + 8;
        int zIndex =(int)Math.Round(newBlockTarget.transform.position.z - worldTarget.transform.position.z) + 8;
        int yIndex =(int)(newBlock.transform.position.y - worldTarget.transform.position.y);
        worldList[zIndex + 16 * xIndex + 256 * yIndex] = 1;
        topology[zIndex + xIndex * 16] = CalculateTopo(worldList, zIndex, xIndex);
    }


    void ActivateButtons()
    // When the newBlock target is in the minecraft world, UI appears
    {
        nextTextureButton.gameObject.SetActive(true);
        prevTextureButton.gameObject.SetActive(true);
        saveBlockButton.gameObject.SetActive(true);
    }

    void DesactivateButtons()
    // When the newBlock target isn't in the minecraft world, UI disappears
    {
        nextTextureButton.gameObject.SetActive(false);
        prevTextureButton.gameObject.SetActive(false);
        saveBlockButton.gameObject.SetActive(false);
    }
}
