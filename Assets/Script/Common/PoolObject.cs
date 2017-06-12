using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolObject : MonoBehaviour, IPool
{
    int m_PoolPosition;
    public void SetPoolPosition(int position)
    {
        m_PoolPosition = position;
    }

    public int GetPoolPosition()
    {
        return m_PoolPosition;
    }

    public virtual void Reset()
    {
        throw new NotImplementedException();
    }

    public virtual void Recycle()
    {
        throw new NotImplementedException();
    }
}
