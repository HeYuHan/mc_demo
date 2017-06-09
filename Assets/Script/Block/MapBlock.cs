using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapBlock:SerializableObject
{
    public const int BLOCK_SIZE = 16;
    public const int BLOCK_HEIGHT = 8;
    ShapeInfo[] m_Shapes;
    public WorldSpaceType SpaceType { get { return m_SpaceType; }set { m_SpaceType = value; } }
    public ShapeIndex Index { get { return m_Index; }set { m_Index = value; } }
    [SerializeField]
    List<ShapeInfo> m_ExportShapeList;
    [SerializeField]
    WorldSpaceType m_SpaceType;
    [SerializeField]
    ShapeIndex m_Index;
    public int Count { get; private set; }
    public override bool Init()
    {
        m_Shapes = new ShapeInfo[BLOCK_SIZE * BLOCK_SIZE * BLOCK_HEIGHT];
        m_ExportShapeList = new List<ShapeInfo>();
        return true;
    }
    public override bool Serialize(out string json)
    {
        m_ExportShapeList.Clear();
        for(int i=0;i<m_Shapes.Length;i++)
        {
            if(m_Shapes[i]!=null)
            {
                m_ExportShapeList.Add(m_Shapes[i]);
            }
        }
        bool result = base.Serialize(out json);
        m_ExportShapeList.Clear();
        return result;
    }
    public override bool Deserialize(string json)
    {

        bool result = Deserialize(json);
        int len = m_ExportShapeList.Count;
        
        for (int i = 0; i < len; i++)
        {
            AddShape(m_ExportShapeList[i]);
        }
        Count = len;
        m_ExportShapeList.Clear();
        return result;
    }
    public int SwicthWorldIndexToBlock(ShapeIndex index)
    {
        int data_start = index.z % BLOCK_HEIGHT;
        int cell_start = index.x;
        int row_start = index.y;
        return BLOCK_SIZE * BLOCK_SIZE * data_start + row_start * BLOCK_SIZE + cell_start;
    }
    public void AddShape(ShapeInfo shape)
    {
        if (shape == null) return;
        int index = SwicthWorldIndexToBlock(shape.Index);
        if (index>0)
        {
            if(m_Shapes[index]==null&&shape!=null)
            {
                Count++;
            }
            m_Shapes[index] = shape;
        }
    }
    public bool RemoveShape(ShapeIndex index)
    {
        int i = SwicthWorldIndexToBlock(index);
        if (i > 0)
        {
            var shape = m_Shapes[i];
            m_Shapes[i] = null;
            if(shape!=null)
            {
                Count--;
                return true;
            }
        }
        return false;
    }
    public bool RemoveShape(ShapeInfo shape)
    {
        if (shape == null) return false;
        return RemoveShape(shape.Index);
    }
    public ShapeInfo GetShape(ShapeIndex index)
    {
        int i = SwicthWorldIndexToBlock(index);
        if (i > 0)
        {
            return m_Shapes[i];
        }
        return null;
    }
    public void Clean()
    {
        for (int i = 0; i < m_Shapes.Length; i++)
        {
            if (m_Shapes[i] != null)
            {
                m_Shapes[i].Clean();
            }
            m_Shapes[i] = null;
            
        }
        Count = 0;
    }
    
}
