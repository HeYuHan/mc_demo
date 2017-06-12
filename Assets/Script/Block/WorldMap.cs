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
    [SerializeField]
    int m_MapHeight = 10;
    [SerializeField]
    int m_MapSize = 20;
    
    public WorldMap()
    {
        m_PerlinNoise = new PerlinNoise();
        m_ExportMapBlockList = new List<MapBlock>();
    }
    public void InitMap()
    {
        for(int x=0;x< m_MapSize; x++)
        {
            for(int z=0;z< m_MapSize; z++)
            {
                int y= GetShapeHeight(x, z);
                var shape_info = new ShapeInfo();
                shape_info.SpaceType = GetShapeSpaceType(new Vector3(x, y, z));
                shape_info.Index.x = x;
                shape_info.Index.y = y;
                shape_info.Index.z = z;
                shape_info.Init();
                shape_info.Apply();
                AddShapeInfo(shape_info);

            }
        }
    }
    public int GetShapeHeight(int x,int z)
    {
        return Mathf.RoundToInt((m_PerlinNoise.PerlinNoise2D(x, z) * m_MapHeight))+m_MapHeight;
    }
    public ShapeInfo GetMapInfo(int x,int z)
    {
        int y = GetShapeHeight(x,z);
        var shape_info = new ShapeInfo();
        shape_info.SpaceType = GetShapeSpaceType(new Vector3(x, y, z));
        shape_info.Index.x = x;
        shape_info.Index.y = y;
        shape_info.Index.z = z;
        
        return shape_info;
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
    public override void BeginExport()
    {
        for (int i = 0; i < SPACE_COUNT; i++)
        {
            for (int j = 0; j < m_Blocks[i].Length; j++)
            {
                if (m_Blocks[i][j] != null)
                {
                    m_Blocks[i][j].BeginExport();
                    m_ExportMapBlockList.Add(m_Blocks[i][j]);
                }
            }
        }
    }
    public override void EndExport()
    {
        for (int i=0;i<m_ExportMapBlockList.Count;i++)
        {
            m_ExportMapBlockList[i].EndExport();
        }
        m_ExportMapBlockList.Clear();
    }
    public override void BeginImport()
    {
        Init();
    }
    public override void EndImport()
    {
        int len = m_ExportMapBlockList.Count;
        for (int i = 0; i < len; i++)
        {
            SetBlock(m_ExportMapBlockList[i]);
        }
        m_ExportMapBlockList.Clear();
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
    public static WorldSpaceType GetShapeSpaceType(Vector3 pos)
    {
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
            if (block_array.Length<= array_index)
            {
                int len = (block_index.z+EXPAND_BLOCK_SIZE) * BLOCKMAP_SIZE * BLOCKMAP_SIZE;
                MapBlock[] bs = new MapBlock[len];
                Array.Copy(block_array, bs, block_array.Length);
                SetMapBlocks(type, bs);
                block_array = bs;
            }

            block = block_array[array_index];
            if (block==null)
            {
                block = new MapBlock();
                block.SpaceType = type;
                block.Index = block_index;
                block.Init();
                block_array[array_index] = block;
            }
            
        }
        return block;
    }
    public int GetBlockArrayIndex(ShapeIndex block_index)
    {
        int z = block_index.y * BLOCKMAP_SIZE * BLOCKMAP_SIZE;
        int cell = block_index.z;
        int row = block_index.x * BLOCKMAP_SIZE;
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
        index.x = (Mathf.Abs(index.x)) / MapBlock.BLOCK_SIZE;
        index.z = (Mathf.Abs(index.z)) / MapBlock.BLOCK_SIZE;
        index.y = (Mathf.Abs(index.y)) / MapBlock.BLOCK_HEIGHT;
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
    public void UpdateAllBlock()
    {

        for (int i = 0; i < SPACE_COUNT; i++)
        {
            for (int j = 0; j < m_Blocks[i].Length; j++)
            {
                if (m_Blocks[i][j] != null) m_Blocks[i][j].UpdateIndexByNoise(m_PerlinNoise,m_MapHeight);
            }
        }

    }
}
