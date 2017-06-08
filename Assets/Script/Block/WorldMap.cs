using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WorldSpaceType
{
    NONE,
    UP_RIGHT_FORWARD,
    UP_LEFT_FORWARD,
    UP_RIGHT_BACK,
    UP_LEFT_BACK,
    DOWN_RIGHT_FORWARD,
    DOWN_LEFT_FORWARD,
    DOWN_RIGHT_BACK,
    DOWN_LEFT_BACK
}

public class WorldMap
{
    public const int INIT_BLOCK_SIZE = 4;
    private MapBlock[] m_UpRightForwardBlocks;
    private MapBlock[] m_UpLeftForwardBlocks;
    private MapBlock[] m_UpRightBackBlocks;
    private MapBlock[] m_UpLeftBackBlocks;

    private MapBlock[] m_DownRightForwardBlocks;
    private MapBlock[] m_DownLeftForwardBlocks;
    private MapBlock[] m_DownRightBackBlocks;
    private MapBlock[] m_DownLeftBackBlocks;
    public WorldMap()
    {
        m_UpRightForwardBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_UpLeftForwardBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_UpRightBackBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_UpLeftBackBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];

        m_DownRightForwardBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_DownLeftForwardBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_DownRightBackBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
        m_DownLeftBackBlocks = new MapBlock[INIT_BLOCK_SIZE * INIT_BLOCK_SIZE * INIT_BLOCK_SIZE];
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
        switch(type)
        {
            default:
            case WorldSpaceType.NONE:
                return null;
            case WorldSpaceType.UP_RIGHT_FORWARD:
                return m_UpRightForwardBlocks;
            case WorldSpaceType.UP_RIGHT_BACK:
                return m_UpRightBackBlocks;
            case WorldSpaceType.UP_LEFT_FORWARD:
                return m_UpLeftForwardBlocks;
            case WorldSpaceType.UP_LEFT_BACK:
                return m_UpLeftBackBlocks;
            case WorldSpaceType.DOWN_RIGHT_FORWARD:
                return m_DownRightForwardBlocks;
            case WorldSpaceType.DOWN_RIGHT_BACK:
                return m_DownRightBackBlocks;
            case WorldSpaceType.DOWN_LEFT_FORWARD:
                return m_DownLeftForwardBlocks;
            case WorldSpaceType.DOWN_LEFT_BACK:
                return m_DownLeftBackBlocks;
        }
    }

    public void AddShape(Shape shape)
    {

    }
}
