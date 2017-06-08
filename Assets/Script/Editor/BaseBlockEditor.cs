using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects()]
[CustomEditor(typeof(Block),true)]
public class BlockEditor:Editor
{
    public enum NewDir
    {
        UP,DOWN,FORWARD,BACK,LEFT,RIGHT
    }
    Block block;
    private void OnEnable()
    {
        block = target as Block;
    }
    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Format Position"))
        {
            FormatPosition();
            GUI.changed = true;
        }
        if (GUILayout.Button("Delete"))
        {
            GameObject.DestroyImmediate(block.gameObject);
            EditorGUIUtility.ExitGUI();
            return;
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Instance New");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Up"))
        {
            InstanceNew(NewDir.UP);
        }
        if (GUILayout.Button("Down"))
        {
            InstanceNew(NewDir.DOWN);
        }
        if (GUILayout.Button("Forward"))
        {
            InstanceNew(NewDir.FORWARD);
        }
        if (GUILayout.Button("Back"))
        {
            InstanceNew(NewDir.BACK);
        }
        if (GUILayout.Button("Left"))
        {
            InstanceNew(NewDir.LEFT);
        }
        if (GUILayout.Button("Right"))
        {
            InstanceNew(NewDir.RIGHT);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MaterialType"));
        
        if(GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
        Debug.Log(GUI.changed);
    }
    public void FormatPosition()
    {
        Vector3 pos = block.transform.position;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        pos.z = Mathf.RoundToInt(pos.z);
        block.transform.position = pos;
    }
    public void InstanceNew(NewDir dir)
    {
        Vector3 pos_offset = Vector3.zero;
        switch (dir)
        {
            case NewDir.BACK:
                pos_offset = Vector3.back;
                break;
            case NewDir.DOWN:
                pos_offset = Vector3.down;
                break;
            case NewDir.FORWARD:
                pos_offset = Vector3.forward;
                break;
            case NewDir.LEFT:
                pos_offset = Vector3.left;
                break;
            case NewDir.RIGHT:
                pos_offset = Vector3.right;
                break;
            case NewDir.UP:
                pos_offset = Vector3.up;
                break;
        }
        if (!Physics.Raycast(block.transform.position, pos_offset, 1))
        {
            GameObject go = GameObject.Instantiate(block.gameObject);
            go.name = block.name;
            Selection.activeObject = go;
            
            go.transform.position = block.transform.position + pos_offset;
        }
        else
        {
            Debug.LogError("crate new block failed");
        }
        
    }
}
