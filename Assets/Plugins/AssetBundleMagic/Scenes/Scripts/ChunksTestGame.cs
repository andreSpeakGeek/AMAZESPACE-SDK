using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jacovone.AssetBundleMagic;


public class ChunksTestGame : MonoBehaviour {

    private ChunkManager cm;

	// Use this for initialization
	void Start () {
        cm = GameObject.Find("ChunkManager").GetComponent<ChunkManager>();
	}

    public void SetDistanceBias(float distanceBias)
    {
        cm.distanceBias = distanceBias;
    }

    public void CleanCache()
    {
        AssetBundleMagic.CleanBundlesCache();
    }
}
