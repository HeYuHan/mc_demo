using System;
using UnityEngine;

[Serializable]
public class JosnVector : IJson
{
    public float[] D;
    public float x { get { return D[0]; } set { D[0] = value; } }
    public float y { get { return D[1]; } set { D[1] = value; } }
    public float z { get { return D[2]; } set { D[2] = value; } }
    public float w { get { return D[3]; } set { D[3] = value; } }
    public JosnVector()
    {
        D = new float[] { 0, 0, 0, 0 };
    }
    public bool Deserialize(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        return D != null && D.Length > 0;
    }

    public bool Serialize(out string json)
    {
        json = JsonUtility.ToJson(this);
        return !string.IsNullOrEmpty(json);
    }
    public static implicit operator Vector3(JosnVector j)
    {
        Vector3 result = new Vector3(j.D[0], j.D[1], j.D[2]);
        return result;
    }
    public static implicit operator JosnVector(Vector3 v)
    {
        JosnVector result = new JosnVector();
        result.D[0] = v.x;
        result.D[1] = v.y;
        result.D[2] = v.z;
        return result;
    }
    public static implicit operator Vector2(JosnVector j)
    {
        Vector2 result = new Vector2(j.D[0], j.D[1]);
        return result;
    }
    public static implicit operator JosnVector(Vector2 v)
    {
        JosnVector result = new JosnVector();
        result.D[0] = v.x;
        result.D[1] = v.y;
        return result;
    }
    public static implicit operator Vector4(JosnVector j)
    {
        Vector4 result = new Vector4(j.D[0], j.D[1], j.D[2], j.D[3]);
        return result;
    }
    public static implicit operator JosnVector(Vector4 v)
    {
        JosnVector result = new JosnVector();
        result.D[0] = v.x;
        result.D[1] = v.y;
        result.D[2] = v.z;
        result.D[3] = v.w;
        return result;
    }
    public static implicit operator Color(JosnVector j)
    {
        Color result = new Color(j.D[0], j.D[1], j.D[2], j.D[3]);
        return result;
    }
    public static implicit operator JosnVector(Color v)
    {
        JosnVector result = new JosnVector();
        result.D[0] = v.r;
        result.D[1] = v.g;
        result.D[2] = v.b;
        result.D[3] = v.a;
        return result;
    }
}
