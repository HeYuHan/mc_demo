using UnityEngine;
using System;
public interface IJson
{
    bool Serialize(out string json);
    bool Deserialize(string json);
}
[Serializable]
public struct ShapeIndex
{
    public int x, y, z;
}

public static class Common
{
    public static T SafeAddComponent<T>(this GameObject go)where T:Component
    {
        T c = go.GetComponent<T>();
        if(c)
        {
            return c;
        }
        c = go.AddComponent<T>();
        return c;
    }
    public static T SafeAddComponent<T>(this Transform trans) where T : Component
    {
        T c = trans.GetComponent<T>();
        if (c)
        {
            return c;
        }
        c = trans.gameObject.AddComponent<T>();
        return c;
    }
}