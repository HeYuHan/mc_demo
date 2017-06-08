using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects()]
[CustomEditor(typeof(Shape), true)]

public class ShapeEditor : Editor
{
    Shape m_Shape;
    ShapeRenderInfo m_RenderInfo;
    ShapeMeshType m_MeshType;
    MeshFilter m_MeshFilter;
    MeshRenderer m_MeshRenderer;
    private void OnEnable()
    {
        m_Shape = target as Shape;
        m_RenderInfo = m_Shape.m_RenderInfo;
        m_MeshType = m_RenderInfo.MeshType;
        m_MeshFilter = m_Shape.gameObject.SafeAddComponent<MeshFilter>();
        m_MeshRenderer = m_Shape.gameObject.SafeAddComponent<MeshRenderer>();
    }
    public override void OnInspectorGUI()
    {
        m_RenderInfo.MeshType = (ShapeMeshType)EditorGUILayout.EnumPopup("MeshType",m_RenderInfo.MeshType);

        GUILayout.Label("--------------------------------------------------");
        GUILayout.Label("ColorMap: "+m_RenderInfo.ColorMapName);
        GUILayout.Label("MaskMap: "+m_RenderInfo.MaskMapName);
        GUILayout.Label("MaterialType: " + m_RenderInfo.MaterialType);
        GUILayout.Label("--------------------------------------------------");
        if (m_MeshType!=m_RenderInfo.MeshType)
        {
            m_MeshType = m_RenderInfo.MeshType;
            switch(m_MeshType)
            {
                case ShapeMeshType.NONE:
                    m_MeshFilter.mesh = null;
                    break;
                case ShapeMeshType.CBUE:
                    m_MeshFilter.mesh = Shape.GetCubeMesh();
                    break;
                case ShapeMeshType.PLANE:
                    m_MeshFilter.mesh = Shape.GetPlaneMesh();
                    break;
            }
        }
        if(m_MeshRenderer.sharedMaterial!=null)
        {
            string shader_name = m_MeshRenderer.sharedMaterial.shader.name;
            shader_name = shader_name.Substring(shader_name.IndexOf('/')+1);
            switch(shader_name)
            {
                case "color_map_only":
                    m_RenderInfo.MaterialType = ShapeMaterialType.COLOR_MAP_ONLY;
                    m_RenderInfo.ColorMapName = string.Empty;
                    m_RenderInfo.MaskMapName = string.Empty;
                    if(m_MeshRenderer.sharedMaterial.mainTexture)
                    {
                        m_RenderInfo.ColorMapName = m_MeshRenderer.sharedMaterial.mainTexture.name;
                    }
                    break;
                case "mask_color_map":
                    m_RenderInfo.MaterialType = ShapeMaterialType.MASK_COLOR_MAP;
                    m_RenderInfo.ColorMapName = string.Empty;
                    m_RenderInfo.MaskMapName = string.Empty;
                    if (m_MeshRenderer.sharedMaterial.mainTexture)
                    {
                        m_RenderInfo.ColorMapName = m_MeshRenderer.sharedMaterial.mainTexture.name;
                    }
                    var mask = m_MeshRenderer.sharedMaterial.GetTexture("_MaskTex");
                    if(mask)
                    {
                        m_RenderInfo.MaskMapName = mask.name;
                    }
                    break;
                default:
                    m_RenderInfo.MaterialType = ShapeMaterialType.NONE;
                    m_RenderInfo.ColorMapName = string.Empty;
                    m_RenderInfo.MaskMapName = string.Empty;
                    break;
            }
        }
        serializedObject.ApplyModifiedProperties();

    }
    
}
