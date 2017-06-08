using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class JTexture : IJson
{
    public int[] Colors;
    public int Width, Height;
    private Texture2D m_Texture;
    public JTexture(int w,int h, Color[] data)
    {
        Width = w;
        Height = h;
        Colors = new int[w*h*4];
        for(int i=0;i< Height; i++)
        {
            for(int j=0;j< Width; j++)
            {
                int index = i * Width + j;
                Color color = data[index];
                Colors[index * 4 + 0] = (int)(255*color.r);
                Colors[index * 4 + 1] = (int)(255*color.g);
                Colors[index * 4 + 2] = (int)(255*color.b);
                Colors[index * 4 + 3] = (int)(255*color.a);
            }
        }
    }
    public JTexture()
    {
    }
    //public Texture2D ToTexutre2D()
    //{
    //    if(!m_Texture)
    //    {
    //        m_Texture = new Texture2D(Width, Height);
    //        Color[] cs = new Color[Width * Height];
    //        for (int i = 0; i < Width; i++)
    //        {
    //            for (int j = 0; j < Height; j++)
    //            {
    //                cs[i * j + j] = colors[i * j + j];
    //            }
    //        }
    //        m_Texture.SetPixels(cs);
    //        m_Texture.Apply();
    //    }
    //    return m_Texture;
    //}
    public Color[] ToColorArray(out int width,out int height)
    {
        width = Width;
        height = Height;
        Color[] result = new Color[Width*Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                int index = i * Height + j;
                Color color=new Color();
                color.r = Colors[index * 4 + 0] / 255.0f;
                color.g = Colors[index * 4 + 1] / 255.0f;
                color.b = Colors[index * 4 + 2] / 255.0f;
                color.a = Colors[index * 4 + 3] / 255.0f;
                result[index] = color;
            }
        }
        return result;
    }
    public bool Deserialize(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        return Colors != null;
    }

    public bool Serialize(out string json)
    {
        json = JsonUtility.ToJson(this);
        return !string.IsNullOrEmpty(json);
    }
}
