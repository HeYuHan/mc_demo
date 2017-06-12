using System;
using System.Collections.Generic;
using UnityEngine;
public enum ShapeMaterialType
{
    NONE=0,
    COLOR_MAP_ONLY,
    MASK_COLOR_MAP,
    WATAER_ANIMATION,
}
public enum ShapeMeshType
{
    NONE=0,CBUE,PLANE
}
public enum ShapeUVType
{
    NONE = 0, ONE_TEX, TWO_TEX,THREE_TEX
}
[Serializable]
public class ShapeRenderInfo:ShapeInfo
{
    public ShapeMeshType MeshType;
    //[HideInInspector]
    public ShapeMaterialType MaterialType;
    //[HideInInspector]
    [SerializeField]
    public string ColorMapName;
    //[HideInInspector]
    [SerializeField]
    public string MaskMapName;
}
