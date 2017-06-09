using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ShapeInfo : SerializableObject
{
    public ShapeIndex Index;
    public WorldSpaceType SpaceType;
    public int ResourceID;
    Shape m_ShapeObject;
    public Shape GetOrCreateShape()
    {
        if(!m_ShapeObject)
        {
            m_ShapeObject=Shape.Pool.Pop();
        }
        return m_ShapeObject;
    }
    public void Clean()
    {
        if (m_ShapeObject)
        {
            Shape.Pool.Push(m_ShapeObject);
            m_ShapeObject = null;
        }
    }
    public void Apply()
    {
        if (m_ShapeObject) m_ShapeObject.Applay(this);
    }

}
