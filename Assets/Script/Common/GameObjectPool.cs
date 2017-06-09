using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T:PoolObject
{
    public const int EXPAND_SIZE = 8;
    T[] m_Pool;
    int[] m_DataPosition;
    T m_EmptyObject;
    int m_Count;
    string m_Name;
    bool m_Inited = false;
    public bool Init()
    {
        m_Inited = true;
        m_Name = typeof(T).ToString();
        m_Count = EXPAND_SIZE;
        m_Pool = new T[m_Count];
        m_DataPosition = new int[m_Count];
        for(int i=0;i< m_Count;i++)
        {
            m_Pool[i] = New();
            m_Pool[i].SetPoolPosition(i);
            m_DataPosition[i] = i+1;
        }
        m_EmptyObject = m_Pool[0];
        return true;
    }
    
    private T New()
    {
        GameObject go = new GameObject(m_Name);
        T t = go.SafeAddComponent<T>();
        t.Create();
        go.SetActive(false);
        return t;
    }
    public void Push(T o)
    {
        m_DataPosition[o.GetPoolPosition()] = m_EmptyObject.GetPoolPosition();
        m_EmptyObject = o;
        o.Recycle();
    }
    public T Pop()
    {
        if (!m_Inited) Init();
        T result = m_EmptyObject;
        m_EmptyObject = null;
        if (result.GetPoolPosition() < m_Count - 1)
        {
            int pos = m_DataPosition[result.GetPoolPosition()];
            if (pos >= 0 && pos < m_Count) m_EmptyObject = m_Pool[pos];
        }
        if (!m_EmptyObject)
        {
            T[] new_Pool = new T[m_Count + EXPAND_SIZE];
            int[] new_Position = new int[m_Count + EXPAND_SIZE];
            Array.Copy(m_Pool, new_Pool, m_Count);
            Array.Copy(m_DataPosition, new_Position, m_Count);
            for (int i = m_Count; i < EXPAND_SIZE + m_Count; i++)
            {
                new_Pool[i] = New();
                new_Pool[i].SetPoolPosition(i);
                new_Position[i] = i + 1;
            }
            m_EmptyObject = new_Pool[m_Count];
            m_Pool = new_Pool;
            m_DataPosition = new_Position;
            m_Count += EXPAND_SIZE;
        }

        return result;
    }
    public void Clean()
    {
        for(int i=0;i<m_Count;i++)
        {
            GameObject.Destroy(m_Pool[i]);
        }
        m_Pool = null;
        m_DataPosition = null;
        m_EmptyObject = null;
        m_Inited = false;
    }
}
