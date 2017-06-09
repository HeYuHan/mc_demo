using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject
{
    public virtual bool Init()
    {
        return true;
    }
}
[Serializable]
public class SerializableObject : BaseObject, IJson
{
    public virtual bool Deserialize(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
        return true;
    }

    public virtual bool Serialize(out string json)
    {
        json = JsonUtility.ToJson(this, true);
        return !string.IsNullOrEmpty(json);
    }
}

