using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jacovone.AssetBundleMagic
{
    public class LoadingIndicatorScript : MonoBehaviour
    {
        public Image RotatingLoading;
        public Image ColoredCircle;
        private ChunkManager _chunkManager;

        void Start ()
        {
            GameObject go = GameObject.Find ("ChunkManager");
            if (go != null) {
                _chunkManager = go.GetComponent<ChunkManager> ();
            }
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (_chunkManager != null) {
                if (_chunkManager.currentProgress != null) {
                    RotatingLoading.enabled = true;
                    ColoredCircle.enabled = true;
                    ColoredCircle.fillAmount = _chunkManager.currentProgress.GetProgress ();
                } else {
                    RotatingLoading.enabled = false;
                    ColoredCircle.enabled = false;
                }
            } else {

                RotatingLoading.enabled = false;
                ColoredCircle.enabled = false;
            }
        }
    }
}
