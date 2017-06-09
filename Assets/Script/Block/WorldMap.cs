using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum WorldSpaceType
{
    NONE=-1,
    UP_RIGHT_FORWARD,
    UP_LEFT_FORWARD,
    UP_RIGHT_BACK,
    UP_LEFT_BACK,
    DOWN_RIGHT_FORWARD,
    DOWN_LEFT_FORWARD,
    DOWN_RIGHT_BACK,
    DOWN_LEFT_BACK,
    TYPE_COUNT,
}
[Serializable]
public class WorldMap:SerializableObject
{
    public const int INIT_BLOCK_SIZE = 2;
    public const int BLOCKMAP_SIZE = 16;
    public const int EXPAND_BLOCK_SIZE = 4;
    public const int SPACE_COUNT = (int)WorldSpaceType.TYPE_COUNT;
    MapBlock[][] m_Blocks;
    [SerializeField]
    PerlinNoise m_PerlinNoise;
    [SerializeField]
    List<MapBlock> m_ExportMapBlockList;
    public WorldMap()
    {
        m_PerlinNoise = new PerlinNoise();
        m_ExportMapBlockList = new List<MapBlock>();
    }
    private void CleanBlock()
    {
        for (int i = 0; i < SPACE_COUNT; i++)
        {
            for (int j = 0; j < m_Blocks[i].Length; j++)
            {
                if (m_Blocks[i][j] != null) m_Blocks[i][j].Clean();
                m_Blocks[i][j] = null;
            }
        }
 
    }
    private void ExportBlock()
    {
        for(int i=0;i< SPACE_COUNT; i++)
        {
            for(int j=0;j<m_Blocks[i].Length;j++)
            {
                if (m_Blocks[i][j] != null) m_ExportMapBlockList.Add(m_Blocks[i][j]);
            }
        }
    }
    public override bool Serialize(out string json)
    {
        m_ExportMapBlockList.Clear();
        ExportBlock();
        bool result = base.Serialize(out json);
        m_ExportMapBlockList.Clear();
        return result;
    }
    public override bool Deserialize(string json)
    {
        Init();
        bool result = base.Deserialize(json);
        int len = m_ExportMapBlockList.Count;
        for(int i=0;i<len;i++)
        {
            SetBlock(m_ExportMapBlockList[i]);
        }
        m_ExportMapBlockList.Clear();
        return result;
    }
    public override bool Init()
    {
        m_Blocks = new MapBlock[SPACE_COUNT][];
        int len = INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE;
        for (int i = 0; i < SPACE_COUNT; i++)
        {
            m_Blocks[i] = new MapBlock[len];
        }
        return true;
    }
    public static WorldSpaceType GetShapeSpaceType(Shape shape)
    {
        if (shape == null) return WorldSpaceType.NONE;
        Vector3 pos = shape.transform.position;
        //up
        if (pos.y>=0)
        {
            //right
            if(pos.x>=0)
            {
                //forward
                if(pos.z>=0)
                {
                    return WorldSpaceType.UP_RIGHT_FORWARD;
                }
                else
                {
                    return WorldSpaceType.UP_RIGHT_BACK;
                }
            }
            else
            {
                //forward
                if (pos.z >= 0)
                {
                    return WorldSpaceType.UP_LEFT_FORWARD;
                }
                else
                {
                    return WorldSpaceType.UP_LEFT_BACK;
                }
            }
        }
        else
        {
            //right
            if (pos.x >= 0)
            {
                //forward
                if (pos.z >= 0)
                {
                    return WorldSpaceType.DOWN_RIGHT_FORWARD;
                }
                else
                {
                    return WorldSpaceType.DOWN_RIGHT_BACK;
                }
            }
            else
            {
                //forward
                if (pos.z >= 0)
                {
                    return WorldSpaceType.DOWN_LEFT_FORWARD;
                }
                else
                {
                    return WorldSpaceType.DOWN_LEFT_BACK;
                }
            }
        }
    }
    private MapBlock[] GetMapBlocks(WorldSpaceType type)
    {
        if(type>WorldSpaceType.NONE&&type<WorldSpaceType.TYPE_COUNT)
        {
            return m_Blocks[(int)type];
        }
        return null;
       
    }
    private void SetMapBlocks(WorldSpaceType type, MapBlock[] block_array)
    {
        if (type > WorldSpaceType.NONE && type < WorldSpaceType.TYPE_COUNT)
        {
            m_Blocks[(int)type]=block_array;
        }
    }
    public void SetBlock(MapBlock block)
    {
        var block_array = GetMapBlocks(block.SpaceType);
        if (block_array != null)
        {
            int array_index = GetBlockArrayIndex(block.Index);
            if (block_array.Length < array_index)
            {
                int len = (block.Index.z + EXPAND_BLOCK_SIZE) * BLOCKMAP_SIZE * BLOCKMAP_SIZE;
                MapBlock[] bs = new MapBlock[len];
                Array.Copy(block_array, bs, block_array.Length);
                SetMapBlocks(block.SpaceType, bs);
                block_array[array_index] = block;
            }
            block = block_array[array_index];
        }
    }
    public MapBlock CreateOrGetBlock(WorldSpaceType type,ShapeIndex block_index)
    {
        MapBlock block = null;
        var block_array = GetMapBlocks(type);
        if(block_array!=null)
        {
            int array_index = GetBlockArrayIndex(block_index);
            if (block_array.Length< array_index)
            {
                int len = (block_index.z + EXPAND_BLOCK_SIZE) * BLOCKMAP_SIZE * BLOCKMAP_SIZE;
                MapBlock[] bs = new MapBlock[len];
                Array.Copy(block_array, bs, block_array.Length);
                SetMapBlocks(type, bs);
                block_array = bs;
                block = new MapBlock();
                block.SpaceType = type;
                block.Index = block_index;
                block_array[array_index] = block;
            }
            block = block_array[array_index];
        }
        return block;
    }
    public int GetBlockArrayIndex(ShapeIndex block_index)
    {
        int z = block_index.z * BLOCKMAP_SIZE * BLOCKMAP_SIZE;
        int cell = block_index.x;
        int row = block_index.y * BLOCKMAP_SIZE;
        return z + row + cell;
    }
    public MapBlock GetMapBlock(WorldSpaceType type,ShapeIndex index)
    {
        var block_array = GetMapBlocks(type);
        if(block_array!=null)
        {
            int array_index = GetBlockArrayIndex(index);
            if(array_index>=0&&array_index<block_array.Length)
            {
                return block_array[array_index];
            }
        }
        return null;
    }
    public ShapeIndex ShapeIndexToBlockIndex(ShapeIndex index)
    {
        index.x = (index.x + 1) / MapBlock.BLOCK_SIZE;
        index.y = (index.y + 1) / MapBlock.BLOCK_SIZE;
        index.z = (index.z + 1) / MapBlock.BLOCK_HEIGHT;
        return index;
    }
    public MapBlock GetMapBlock(ShapeInfo info)
    {
        ShapeIndex index = ShapeIndexToBlockIndex(info.Index);
        return GetMapBlock(info.SpaceType, index);
    }
    public void AddShapeInfo(ShapeInfo info)
    {
        ShapeIndex index = ShapeIndexToBlockIndex(info.Index);
        MapBlock block = CreateOrGetBlock(info.SpaceType, index);
        if(block!=null)
        {
            block.AddShape(info);
        }
    }
    public void Clean()
    {
        CleanBlock();
    }
}
