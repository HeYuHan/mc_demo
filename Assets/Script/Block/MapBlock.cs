using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock
{
    public const int BLOCK_LENGTH = 16;
    public const int BLOCK_HEIGHT = 256;

    public readonly ShapeIndex Index;
    public readonly WorldSpaceType WorldSpaceType;
    public int Count { get; private set; }
    private Shape[] m_Shapes;
    public MapBlock(ShapeIndex index, WorldSpaceType type)
    {
        Count = 0;
        Index = index;
        WorldSpaceType = type;
        m_Shapes = new Shape[BLOCK_LENGTH * BLOCK_LENGTH * BLOCK_HEIGHT];
    }
    public int SwicthWorldIndexToBlock(ShapeIndex index)
    {
        int data_start = index.z % BLOCK_HEIGHT;
        int cell_start = index.x;
        int row_start = index.y;
        return BLOCK_LENGTH * BLOCK_LENGTH * data_start + row_start * BLOCK_LENGTH + cell_start;
    }
    public void AddShape(Shape shape)
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
    public bool RemoveShape(Shape shape)
    {
        if (shape == null) return false;
        return RemoveShape(shape.Index);
    }
    public Shape GetShape(ShapeIndex index)
    {
        int i = SwicthWorldIndexToBlock(index);
        if (i > 0)
        {
            return m_Shapes[i];
        }
        return null;
    }
}
