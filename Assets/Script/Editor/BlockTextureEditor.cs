using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockTextureEditor:EditorWindow
{
    [MenuItem("Editor/TextureEditor")]
    static void Create()
    {
        if(instance==null)
        {
            instance = BlockTextureEditor.GetWindow<BlockTextureEditor>();
        }
        instance.Init();
        instance.Show();
    }
    private static BlockTextureEditor instance;
    const int TEXTURE_SIZE = 16;
    Color[] colors;
    GUIStyle box_style;
    int m_SelectX, m_SelectY;
    int m_TextureWidth= TEXTURE_SIZE, m_TextureHeight= TEXTURE_SIZE;
    float BLOCK_SIZE = 10;
    float m_BackUpBlockSize = 10;
    const int SCALE_CTRL_HEIGHT = 20;
    ColorPicker picker;
    private BlockTextureEditor()
    {
        picker = new ColorPicker();
        colors = new Color[m_TextureWidth * m_TextureHeight];
        for (int i = 0; i < m_TextureHeight; i++)
        {
            for (int j = 0; j < m_TextureWidth; j++)
            {
                colors[i * m_TextureWidth + j] = new Color(0,0,0,0);
            }
        }
        box_style = new GUIStyle();
        

    }
    public void Init()
    {
        box_style.normal.background = Texture2D.whiteTexture;
        m_SelectX = m_SelectY = 0;
        picker.Init();
        picker.SetArg(10, 190, new Rect(BLOCK_SIZE* m_TextureWidth+ BLOCK_SIZE, 10, 200, 220));
        picker.SetColor(colors[0]);
    }
    private int GetSelectColor()
    {
        return m_TextureWidth * m_SelectY + m_SelectX;
    }
    private void OnGUI()
    {
        GUI.changed = false;
        BLOCK_SIZE = GUI.HorizontalSlider(new Rect(0, 0, 200, SCALE_CTRL_HEIGHT), BLOCK_SIZE,5, 20);
        if(m_BackUpBlockSize!= BLOCK_SIZE)
        {
            m_BackUpBlockSize = BLOCK_SIZE;
            picker.SetArg(10, 190, new Rect(BLOCK_SIZE * m_TextureWidth + BLOCK_SIZE, 10, 200, 220));
        }
        picker.Draw();
        for (int i=0;i< m_TextureHeight; i++)
        {
            for(int j=0;j< m_TextureWidth; j++)
            {
                GUI.color = Color.white;
                GUI.Box(new Rect(j * BLOCK_SIZE + 1, (m_TextureHeight - 1 - i) * BLOCK_SIZE + 1+ SCALE_CTRL_HEIGHT, BLOCK_SIZE - 1, BLOCK_SIZE - 1), "");
                GUI.color = colors[i*m_TextureWidth+j];
                if(GUI.Button(new Rect(j * BLOCK_SIZE+1, (m_TextureHeight - 1 - i) * BLOCK_SIZE+1 + SCALE_CTRL_HEIGHT, BLOCK_SIZE-1, BLOCK_SIZE-1),"",box_style))
                {
                    m_SelectY = i;
                    m_SelectX = j;
                    picker.SetColor(colors[i * m_TextureWidth + j]);
                }
            }
        }
        colors[m_SelectY * m_TextureWidth + m_SelectX] = picker.GetColor();
        Color tag_color = Color.white - colors[m_SelectY * m_TextureWidth + m_SelectX];
        tag_color.a = 1;
        GUI.color = tag_color;
        GUI.Box(new Rect(m_SelectX * BLOCK_SIZE+ BLOCK_SIZE*0.25f, (m_TextureHeight - 1 - m_SelectY) * BLOCK_SIZE+ BLOCK_SIZE * 0.25f+ SCALE_CTRL_HEIGHT, BLOCK_SIZE * 0.5f, BLOCK_SIZE * 0.5f),"",box_style);
        GUI.color = Color.white;
        if(GUI.Button(new Rect(BLOCK_SIZE * m_TextureWidth + BLOCK_SIZE, 235, 60, 20),"Save"))
        {
            string path = EditorUtility.SaveFilePanel("Save", "Assets/Resource/BlockTexture","NewTexture.json", "*");
            if(!string.IsNullOrEmpty(path))
            {
                if(path.Contains("png"))
                {
                    Texture2D tex = new Texture2D(m_TextureWidth, m_TextureHeight,TextureFormat.ARGB32,false);
                    tex.SetPixels(colors);
                    tex.Apply();
                    System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
                    Texture2D.DestroyImmediate(tex,true);
                    AssetDatabase.Refresh();
                }
                else
                {
                    JTexture tex = new JTexture(m_TextureWidth, m_TextureHeight, colors);
                    string json = string.Empty;
                    if (tex.Serialize(out json))
                    {
                        System.IO.File.WriteAllText(path, json);
                        AssetDatabase.Refresh();
                    }
                }
                

            }
        }
        if (GUI.Button(new Rect(BLOCK_SIZE * m_TextureWidth + BLOCK_SIZE + 60, 235, 65, 20), "Load"))
        {
            string path = EditorUtility.OpenFilePanel("Load","Assets/Resource/BlockTexture","*");
            if (System.IO.File.Exists(path))
            {
                if (path.Contains("json"))
                {
                    string json = System.IO.File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(json))
                    {
                        JTexture tex = new JTexture();
                        if (tex.Deserialize(json))
                        {
                            colors = tex.ToColorArray(out m_TextureWidth, out m_TextureHeight);
                            Init();
                        }
                    }
                }
                else
                {
                    path = path.Replace(Application.dataPath, "Assets");
                    Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    if (tex)
                    {
                        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
                        bool t = importer.isReadable;
                        if (!importer.isReadable)
                        {
                            importer.isReadable = true;
                            importer.SaveAndReimport();
                        }
                        colors = tex.GetPixels();
                        if(!t)
                        {
                            importer.isReadable = false;
                            importer.SaveAndReimport();
                        }
                        m_TextureWidth = tex.width;
                        m_TextureHeight = tex.height;
                        
                        Init();
                    }
                }
            }

        }
        if (GUI.changed) Repaint();
    }
}
