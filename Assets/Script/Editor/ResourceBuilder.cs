using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourceBuilder
{

    [MenuItem("Assets/BuildAssetBundle")]
    public static void CreateAssetBundle()
    {
        string path = "Assets\\StreamingAssets";
        CreateDirectory(GetFullPath(path));
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        //刷新编辑器
        AssetDatabase.Refresh();
    }
    public static string GetFullPath(string path)
    {
        path = path.Replace("\\", "/");
        return Application.dataPath.Replace("Assets", "") + path;
    }
    public static string GetRelativePath(string full)
    {
        full = full.Replace("\\", "/");
        return full.Replace(Application.dataPath, "");
    }
    public static void CreateDirectory(string full_path)
    {
        if(!System.IO.Directory.Exists(full_path))
        {
            System.IO.Directory.CreateDirectory(full_path);
        }
    }
    [MenuItem("Assets/CreateMesh")]
    static void CreateMesh()
    {
        Mesh m = Shape.GetCubeMesh();
        AssetDatabase.CreateAsset(m, "Assets/Resource/Mesh/cube.asset");
        m = Shape.GetPlaneMesh();
        AssetDatabase.CreateAsset(m, "Assets/Resource/Mesh/plane.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
