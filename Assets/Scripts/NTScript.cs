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

    MeshRenderer rend;

    void Start()
    {
        Initialize();
        Debug.Log("done");
        m = new Material(Shader.Find("Transparent/Diffuse"));
        rend = GetComponent<MeshRenderer>();
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


        int radius = 330;
        float rSquared = radius * radius;

        print(x);
        
        Color tempColor = m.color;

        tempColor.a = 0f;
        //Material test = (Material)Resources.Load("test", typeof(Material)); ;


        m = new Material(Shader.Find("Unlit/camshader"));
        rend.material.SetTexture("_MainTex", tex);

        if (x > 140 && x < 500 && y > 140 && y < 340)
        {
            for (int u = x - radius; u < x + radius + 1; u++)
                for (int v = y - radius; v < y + radius + 1; v++)
                    if (((x - u) * (x - u) + (y - v) * (y - v)) * 5.6 > rSquared)
                        tex.SetPixel(u, v, Color.white);

        }
        else
        {
            Color[] cols = tex.GetPixels();
            for (int j = 0; j < cols.Length; j++)

                cols[j] = tempColor;
            tex.SetPixels(cols);
        }


            //rend.GetMaterials(test.SetTexture("_MainText" , tex));
            //rend.material.SetTexture("_MainTex", tex);

            /*else
            {
                Color[] cols = tex.GetPixels();
                for (int j = 0; j < cols.Length; j++)

                    cols[j] = tempColor;
                tex.SetPixels(cols);
            }*/


            /* Material unlit;
             unlit = new Material(Shader.Find("Unlit/camshader"));
             this.GetComponent<Renderer>().material = unlit;

             for (x=0; x < tex.height; x++)
             {
                 for(y =0; y<tex.width; y++)
                 {
                     if(tex.GetPixel(x,y) == Color.white)
                     {

                         tex.SetPixel(x, y, unlit.color);

                     }
                 }
             }
             //this.GetComponent<Renderer>().material = unlit;*/

            tex.Apply();

        
    }

}