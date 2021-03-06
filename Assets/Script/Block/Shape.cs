﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Shape:PoolObject
{

    public static readonly Vector3[] CUBE_VECTOR_DATA = new Vector3[]
    {
        //前
        new Vector3(0.5f,0,0.5f),new Vector3(0.5f,1,0.5f),new Vector3(-0.5f,1,0.5f),new Vector3(-0.5f,0,0.5f),
        //后
        new Vector3(-0.5f,0,-0.5f),new Vector3(-0.5f,1,-0.5f),new Vector3(0.5f,1,-0.5f),new Vector3(0.5f,0,-0.5f),
        //左
        new Vector3(-0.5f,0,0.5f),new Vector3(-0.5f,1,0.5f),new Vector3(-0.5f,1,-0.5f),new Vector3(-0.5f,0,-0.5f),
        //右
        new Vector3(0.5f,0,-0.5f),new Vector3(0.5f,1,-0.5f),new Vector3(0.5f,1,0.5f),new Vector3(0.5f,0,0.5f),
        //上
        new Vector3(-0.5f,1,-0.5f),new Vector3(-0.5f,1,0.5f),new Vector3(0.5f,1,0.5f),new Vector3(0.5f,1,-0.5f),
        //下
        new Vector3(-0.5f,0,0.5f),new Vector3(-0.5f,0,-0.5f),new Vector3(0.5f,0,-0.5f),new Vector3(0.5f,0,0.5f),

    };
    public static readonly int[] CUBE_TRIANGLES_DATA = new int[]
    {
        //前
        0,1,2,0,2,3,
        //后
        4,5,6,4,6,7,
        //左
        8,9,10,8,10,11,
        //右
        12,13,14,12,14,15,
        //上
        16,17,18,16,18,19,
        //下
        20,21,22,20,22,23

    };
    public static readonly Vector2[] CUBE_UV_DATA = new Vector2[]
    {
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right,
    };
    public static readonly Vector3[] PLANE_VECTOR_DATA = new Vector3[]
    {
        new Vector3(-0.5f,0,0),new Vector3(-0.5f,1,0),new Vector3(0.5f,1,0),new Vector3(0.5f,0,0)
    };
    public static readonly int[] PLANE_TRIANGLES_DATA = new int[]
    {
        0,1,2,0,2,3
    };
    public static readonly Vector2[] PLANE_UV_DATA = new Vector2[]
    {
        Vector2.zero,Vector2.up,Vector2.one,Vector2.right
    };
    public static Mesh CUBE_MESH = null;
    public static Mesh PLANE_MESH = null;
    public static Mesh GetCubeMesh()
    {
        if (CUBE_MESH==null)
        {
            CUBE_MESH = new Mesh();
            CUBE_MESH.vertices = CUBE_VECTOR_DATA;
            CUBE_MESH.triangles = CUBE_TRIANGLES_DATA;
            CUBE_MESH.uv = CUBE_UV_DATA;
            CUBE_MESH.RecalculateNormals();
            CUBE_MESH.RecalculateBounds();
        }
        return CUBE_MESH;
    }
    public static Mesh GetPlaneMesh()
    {
        if (PLANE_MESH == null)
        {
            PLANE_MESH = new Mesh();
            PLANE_MESH.vertices = PLANE_VECTOR_DATA;
            PLANE_MESH.triangles = PLANE_TRIANGLES_DATA;
            PLANE_MESH.uv = PLANE_UV_DATA;
            PLANE_MESH.RecalculateNormals();
            PLANE_MESH.RecalculateBounds();
        }
        return PLANE_MESH;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////
    public static Shape Create()
    {
        GameObject go = new GameObject("Shape");
        return go.SafeAddComponent<Shape>();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////
    static GameObjectPool<Shape> m_ShapePool;
    MeshFilter m_MeshFilter;
    MeshRenderer m_MeshRenderer;
    [SerializeField]
    ShapeInfo m_ShapeInfo;
    public static GameObjectPool<Shape> Pool
    {
        get
        {
            if(m_ShapePool==null)
            {
                m_ShapePool = new GameObjectPool<Shape>(Create);
                m_ShapePool.Init();
            }
            return m_ShapePool;
        }
    }

    private void Awake()
    {
        m_MeshFilter = gameObject.SafeAddComponent<MeshFilter>();
        m_MeshRenderer = gameObject.SafeAddComponent<MeshRenderer>();
    }
    public void SetPoistion(Vector3 pos)
    {
    }
    public void Applay(ShapeInfo info)
    {
        m_ShapeInfo = info;
        UpdateMesh(info.MeshType);
        UpdatePosition(info.Index);
        m_MeshRenderer.sharedMaterial = Test.mat;
    }
    public void UpdateColorMap(int resourceID)
    {

    }
    public void UpdateMaskMap(int resourceID)
    {

    }
    public void UpdateMaterial(ShapeMaterialType type)
    {
        switch(type)
        {
            case ShapeMaterialType.COLOR_MAP_ONLY:
                break;
            case ShapeMaterialType.MASK_COLOR_MAP:
                break;
        }
    }
    public void UpdatePosition(ShapeIndex index)
    {
        this.transform.position = index;
    }
    public void UpdateMesh(ShapeMeshType type)
    {
        switch(type)
        {
            case ShapeMeshType.PLANE:
                m_MeshFilter.sharedMesh = GetPlaneMesh();
                break;
            case ShapeMeshType.CBUE:
                m_MeshFilter.sharedMesh = GetCubeMesh();
                break;
            default:
                m_MeshFilter.sharedMesh = null;
                break;
        }

    }
    public override void Reset()
    {
        this.gameObject.SetActive(true);
    }

    public override void Recycle()
    {
        this.gameObject.SetActive(false);
    }
}

