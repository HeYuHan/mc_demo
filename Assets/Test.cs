using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public static Material mat;
    public Material mm;
    public GameObject[,,] gos;
    GameObjectPool<PoolObject> pool;
    List<PoolObject> ps = new List<PoolObject>();
    [SerializeField]
    WorldMap map;
    // Use this for initialization
    void Start () {

        mat = mm;
        map.Init();
        map.InitMap();
        //string s = string.Empty;
        //map.Serialize(out s);
        //System.IO.File.WriteAllText(@"C:\Users\HeLiXia\Desktop\map.json", s);
        return;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            map.UpdateAllBlock();
        }
    }
    private void OnPostRender()
    {
    }
}
