using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockType
{
    None = 0,
    Base,
    Count
}
public enum BlockMatType
{
    NONE=0,
    COLOR_MAP_ONLY,
    MASK_COLOR_MAP,
    WATAER_ANIMATION,
    COUNT
}


[Serializable]
public class BlockInfo : IJson
{
    public BlockType Type = BlockType.Base;
    public BlockMatType MatType=BlockMatType.COLOR_MAP_ONLY;
    public Vector3 Position=Vector3.zero;
    public string TextureName=string.Empty;
    [SerializeField]
    Color m_Color = Color.white;


    public bool Deserialize(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        return Type > BlockType.None && Type< BlockType.Count;
    }

    public bool Serialize(out string json)
    {
#if UNITY_EDITOR
        json = JsonUtility.ToJson(this,true);
#else
        json = JsonUtility.ToJson(this);
#endif
        return !string.IsNullOrEmpty(json);
    }
    public static BlockInfo Test(string json)
    {
        BlockInfo info = new BlockInfo();
        info.Deserialize(json);
        return info;
    }
    public static string Test(BlockInfo info)
    {
        string result;
        info.Serialize(out result);
        return result;
    }
}
[Serializable]
public class GrassBlockInfo:BlockInfo
{

}
