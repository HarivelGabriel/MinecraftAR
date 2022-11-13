using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textures
// This class helps to set texture of every block when the minecraft world is set
{
    [HideInInspector]
    public List<Material> faces;
    // This List will be filled with every faces textures. Once this list is created, SetTexture method
    // apply each texture to each Block faces
    public Textures(string path)
    // This constructor create a list of 6 time the same texture for 1T Blocks
    {
        Material texture = Resources.Load(path, typeof(Material)) as Material;
        this.faces = new List<Material>();
        for (int i = 0; i < 6; i++)
        {
            this.faces.Add(texture);
        }
    }

    public Textures(Material[] ressources)
    // This constructor create a list of 6 material for 2T and 3T blocks
    {
        switch(ressources.Length)
        {
            case 2:
            // In this case top and bot textures are the same and all other faces have the same texture
                this.faces = new List<Material>();
                this.faces.Add(ressources[1]);
                this.faces.Add(ressources[1]);
                for (int i = 2; i < 6; i++)
                    {
                        this.faces.Add(ressources[0]);
                    }
                break;
            case 3:
            // In this case top ,bot and all other faces have a different texture
                this.faces = new List<Material>();
                this.faces.Add(ressources[0]);
                this.faces.Add(ressources[2]);
                for (int i = 2; i < 6; i++)
                    {
                        this.faces.Add(ressources[1]);
                    }
                break;
        }
    }

    public void SetTexture(GameObject b)
    {
        for (int i = 0; i < 6; i++)
        {
            b.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = this.faces[i];
        }
    }
}
