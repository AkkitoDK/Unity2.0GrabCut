using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class NTScript : MonoBehaviour
{
    //Lets make our calls from the Plugin
    [DllImport("FancyLibrary", EntryPoint = "Initialize")]
    private static extern void Initialize();

    [DllImport("FancyLibrary", EntryPoint = "ProcessFrame")]
    private static extern byte[] ProcessFrame();
    Material m;

    [DllImport("FancyLibrary", EntryPoint = "Coordinates")]
    private static extern int Coordinates();

    
    void Start()
    {
        Initialize();
        Debug.Log("done");
        m = new Material(Shader.Find("Transparent/Diffuse"));
    }

    void Update()
    {

        byte[] imgData = ProcessFrame();
        byte[] imgData2 = new byte[640 * 480 * 4];
        for (int i = 0; i < imgData2.Length; i++) {
            imgData2[i] = imgData[i];

        }
        //TextureFormat.BGRA32
        Texture2D tex = new Texture2D(640, 480, TextureFormat.BGRA32, false);

        tex.LoadRawTextureData(imgData2);


        tex.Apply();
        m.mainTexture = tex;
        this.GetComponent<Renderer>().material = m;

        int middle = Coordinates();
        int x = middle / 1000;
        int y = middle % 1000;


        print(x);
        print(y);

        int radius = 350;
        float rSquared = radius * radius;


        Color tempColor = m.color;

        Material materialShader;

        

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if (((x - u) * (x - u) + (y - v) * (y - v)) * 5.5 > rSquared)
                    tex.SetPixel(u, v, Color.white);

        tex.Apply();

       
    }

}