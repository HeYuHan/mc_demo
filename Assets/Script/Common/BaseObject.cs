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
        BeginImport();
        JsonUtility.FromJsonOverwrite(json, this);
        EndImport();
        return true;
    }

    public virtual bool Serialize(out string json)
    {
        BeginExport();
        json = JsonUtility.ToJson(this, false);
        EndExport();
        return !string.IsNullOrEmpty(json);
    }
    public virtual void BeginExport()
    {

    }
    public virtual void EndExport()
    {

    }
    public virtual void BeginImport()
    {

    }
    public virtual void EndImport()
    {

    }
}

