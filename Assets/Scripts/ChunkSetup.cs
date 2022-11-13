using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChunkSetup : MonoBehaviour
{
    private bool build; //allow to build a white block when the block isn't "air" but isn't in JSON
    private string blockID; // ID of the block in chunkfile.txt
    private Textures texture; 
    private List<string> list; // array with x;y;z;id values of a block in chunkfile.txt
    public GameObject prefab;
    public GameObject target;
    private UnityEngine.Object prefabTemp;
    public bool isActive; // allow to launch the generation or not
    public TextAsset jsonFile;

    // Start is called before the first frame update
    void Start()
    {
        if (isActive)
        {
            Blocks blocksInJson = JsonUtility.FromJson<Blocks>(jsonFile.text);
            foreach (string line in System.IO.File.ReadLines(@"Assets/Resources/chunkfile.txt"))
            {
                list = line.Split(';').ToList();
                blockID = list[3];
                //if (blockID != "air" && float.Parse(list[2])<11f && float.Parse(list[2])>4f && float.Parse(list[0])<11f && float.Parse(list[0])>4f) // if air -> go next block
                if (blockID != "air")
                {
                    build = true;
                    foreach (Block block in blocksInJson.blocks)
                    {
                        if (blockID == block.ID) // if blockID not in JSON -> build white prefab
                        {
                            build = false;
                            if(block.Type == 7)
                            {   
                                prefabTemp = Resources.Load(block.Path);
                                GameObject b = (GameObject)GameObject.Instantiate(prefabTemp, new Vector3(float.Parse(list[2]) - 8f, float.Parse(list[1]), float.Parse(list[0]) -8f), Quaternion.identity);
                                b.transform.parent = target.transform;
                            }
                            else
                            {
                                GameObject b = Instantiate(prefab, new Vector3(float.Parse(list[2]) - 8f, float.Parse(list[1]), float.Parse(list[0]) - 8f), Quaternion.identity);
                                b.transform.parent = target.transform;
                                if(block.Type == 1) // if block has 1 texture, call the 1T constructor
                                {
                                    Textures texture = new Textures(block.Path);
                                    texture.SetTexture(b);
                                }
                                else if(block.Type <= 7) // if block has more than 1T call the second constructor
                                {
                                    Textures texture = new Textures
                                        (
                                        Resources.LoadAll(block.Path, typeof(Material)).Cast<Material>().ToArray()
                                        );
                                        texture.SetTexture(b);
                                }
                            }
                        }
                    }
                    if(build)
                    {
                        GameObject b = Instantiate(prefab, new Vector3(float.Parse(list[2]) - 8f, float.Parse(list[1]), float.Parse(list[0]) - 8f), Quaternion.identity);
                        b.transform.parent = target.transform;
                    }
                }
                //if (blockID != "air"){
                //GameObject b = Instantiate(prefab, new Vector3(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2])), Quaternion.identity);
                //}
                //Debug.Log(block);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
