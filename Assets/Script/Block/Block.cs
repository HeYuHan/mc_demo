using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockManager
{
    static Dictionary<string, List<Material>> MaterialList = new Dictionary<string, List<Material>>();
    static BlockManager instance;
    public static BlockManager Instance
    {
        get
        { if(instance==null)
            {
                instance = new BlockManager();
            }
            return instance;
        }
    }
    private BlockManager() { }
    public Material GetMaterial(BlockInfo info)
    {
        
        List<Material> color_mats = null;
        Material result=null;
        if (!MaterialList.TryGetValue(info.TextureName, out color_mats))
        {
            color_mats = new List<Material>();
            MaterialList[info.TextureName] = color_mats;
        }
        for(int i=0;i<color_mats.Count;i++)
        {
            //if(info.m_Color==color_mats[i].color)
            //{
            //    return result;
            //}
        }
        switch (info.MatType)
        {
            case BlockMatType.COLOR_MAP_ONLY:
                result = new Material(Shader.Find("Custom/color_map_only"));
                break;
            case BlockMatType.MASK_COLOR_MAP:
                result = new Material(Shader.Find("Unlit/mask_color_map"));
                break;
        }
        color_mats.Add(result);
        return result;
    }
}

public class Block : MonoBehaviour {
    [SerializeField]
    BlockMatType m_MaterialType;
    [SerializeField]
    BlockInfo m_Info;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
