using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public TextAsset json;
    [Range(0, 1)]
    public float Per=0.5f;
    [Range(0, 100)]
    public int Oct=2;
    PerlinNoise noise;
    [Range(0, 1)]
    public float Delta = 0.1f;
    public GameObject[,,] gos;
    GameObjectPool<PoolObject> pool;
    List<PoolObject> ps = new List<PoolObject>();
    // Use this for initialization
    void Start () {
        var map = new WorldMap();
        map.Init();
        string s = string.Empty;
        map.Serialize(out s);
        System.IO.File.WriteAllText(@"C:\Users\HeLiXia\Desktop\map.json", s);
        return;

        noise = new PerlinNoise(Per,Oct);
        gos = new GameObject[200, 1, 200];
        for(int x=0;x<200;x++)
        {
            for(int y=0;y<1;y++)
            {
                for(int z=0;z<200;z++)
                {
                    gos[x,y,z] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        return;
        if(Input.GetMouseButtonDown(0))
        {
            var o = pool.Pop();
            o.gameObject.SetActive(true);
            ps.Add(o);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(ps.Count>0)
            {
                pool.Push(ps[0]);
                ps[0].gameObject.SetActive(false);
                ps.RemoveAt(0);
            }
        }
        return;
        noise.SetOctaves(Oct);
        noise.SetPersistence(Per);
        if(Input.GetKeyDown(KeyCode.B))
        {
            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    for (int z = 0; z < 200; z++)
                    {
                        gos[x, y, z].transform.position = new Vector3(x, Mathf.RoundToInt((noise.PerlinNoise2D(x*Delta, z * Delta)) * (10)), z);
                    }
                }
            }
        }
	}
    private void OnPostRender()
    {

    }
}
