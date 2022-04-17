using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Jacovone.AssetBundleMagic
{
    /// <summary>
    /// ChunkManager is a Class that manages chunks. A chunk is a UNITY scene that represent a piece of game level.
    /// The idea is that each chunk has a specified position, a "enter trigger radius" and a "exit trigger radius".
    /// When the player enter within the enter trigger radius, the ChunkManager loads the chunk, so load the 
    /// corresponding scene asynchronously. When the player exit the "exit trigger radius", the ChunkManager
    /// unload that.
    /// Each chunk must specify a position, a radius for enter, a radius for exit, and a scene name to load/unload
    /// when appropriate. ChunkManager can use bundles (via AssetBundleMagic) to manage download of asset bundles
    /// dynamically.
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        /// <summary>
        /// Fired when the download (or locally load) of all asset bundles for a specified chunk has initiated.
        /// </summary>
        /// <param name="progress">The Progress instance for monitoring of the load.</param>
        public delegate void LoadAllBundlesStarted (AssetBundleMagic.Progress progress);

        /// <summary>
        /// Fired when the download (or locally load) of all asset bundles for a specified chunk has termianted.
        /// </summary>
        /// <param name="chunkIndex">The chunk index that has terminated.</param>
        public delegate void LoadAllBundlesFinished (int chunkIndex);

        /// <summary>
        /// A bundle definition represents a single instance of bundle within the ChunkManager.
        /// </summary>
        [Serializable]
        public class BundleDef
        {
            public string bundleName;
            public bool fromFile;
            public bool checkVersion;
        }

        /// <summary>
        /// A Chunk is a compelte description of a piece of levelthat can be
        /// loaded and unloaded under particular circumstances.
        /// </summary>
        [Serializable]
        public class Chunk
        {
            public string sceneName;
            public Vector3 center;
            public float loadDistance;
            public float unloadDistance;
            public BundleDef[] bundleList = new BundleDef[]{ };

            public UnityEvent onLoad;
            public UnityEvent onUnload;
        }

        /// <summary>
        /// The subject that is monitored to activate/deactivate chunks.
        /// </summary>
        public Transform subject;

        /// <summary>
        /// The interval for checking player position.
        /// </summary>
        public float interval;

        /// <summary>
        /// Array of all defined chunks.
        /// </summary>
        public Chunk[] chunks;

        /// <summary>
        /// Indicates if the ChunkManager have to refresh asset bundles versions (Versions.txt file)
        /// to refresh bundle's version on the server (only for remote bundles).
        /// </summary>
        public bool downloadVersionsAtStartup;

        /// <summary>
        /// DistanceBias allow to globally tweak enter and exit raius of all chunks togheter.
        /// </summary>
        public float distanceBias = 1f;

        /// <summary>
        /// The current progress for current download operation.
        /// </summary>
        public AssetBundleMagic.Progress currentProgress;

        /// <summary>
        /// Last time ChunkManager has checked player's position.
        /// </summary>
        private float lastCheckTime = 0f;

        /// <summary>
        /// Indicates that ChunkManager is working. If this member is false, it means that
        /// ChunkManager don't yet downloaded Versions.txt files for asset bundles. 
        /// </summary>
        private bool started = false;

        /// <summary>
        /// The Start() method check if the user requests download of Versions.txt file.
        /// If so starts the download, and set the "started" value to false.
        /// </summary>
        void Start ()
        {
            started = true;
            if (downloadVersionsAtStartup) {
                started = false;
                AssetBundleMagic.DownloadVersions (
                    delegate(string v) {
                        started = true;
                        Debug.Log("Versions.txt downloaded: " + v);
                    },
                    delegate(string error) {
                        started = true;
                        Debug.LogError (error);
                        Debug.LogWarning ("The ChunkManager can't download versions. This will disable cache on bundle versions.");
                    });
            }
        }
	
        /// <summary>
        /// In the Update method, if the started value is true, we check the player position
        /// respect all defined chunks via CheckDistances() method.
        /// </summary>
        void Update ()
        {
            if (started)
            {
                if (lastCheckTime + interval <= Time.time)
                {
                    CheckDistances();
                    lastCheckTime = Time.time;
                }
            }
        }

        /// <summary>
        /// Is the main method to check if the player has entered a new chunk zone, or exited an older one.
        /// This method is called by Update() method each "interval" seconds.
        /// </summary>
        void CheckDistances ()
        {
            // Don't enqueue multiple requests
            if (currentProgress != null)
                return;

            for (int i = 0; i < chunks.Length; i++) {
                if (Vector3.Distance (chunks [i].center, subject.position) >= (distanceBias * chunks [i].unloadDistance) &&
                    SceneManager.GetSceneByName (chunks [i].sceneName).isLoaded) {
                    SceneManager.UnloadSceneAsync (chunks [i].sceneName);

                    if (chunks [i].bundleList.Length > 0) {
                        for (int j = 0; j < chunks [i].bundleList.Length; j++) {
                            AssetBundleMagic.UnloadBundle (chunks [i].bundleList [j].bundleName, false);
                        }
                    }

                    chunks [i].onUnload.Invoke ();
                }
                if (Vector3.Distance (chunks [i].center, subject.position) <= (distanceBias * chunks [i].loadDistance) &&
                    !SceneManager.GetSceneByName (chunks [i].sceneName).isLoaded) {

                    LoadAllBundles (i, 0, 
                        delegate(AssetBundleMagic.Progress progress) {
                            currentProgress = progress;
                        },
                        delegate(int chunkIndex) {
                            SceneManager.LoadSceneAsync (chunks [chunkIndex].sceneName, LoadSceneMode.Additive);
                            currentProgress = null;
                            chunks[chunkIndex].onLoad.Invoke ();
                        },
                        delegate(string error) {
                            Debug.LogError (error);
                            currentProgress = null;
                        });
                }
            }
        }

        /// <summary>
        /// This method is responsible to recursively load all bundle needed by a particular chunk
        /// using the AssetBundleMagic class.
        /// </summary>
        /// <param name="chunkIndex">The chunk index for which download bundles.</param>
        /// <param name="startingBundle">The starting bundle index to load (allow recursive work).</param>
        /// <param name="started">The callback delegate method called when a download is started.</param>
        /// <param name="finished">The callback delegate method called when a download is terminated.</param>
        /// <param name="error">The callback delegate method called when an error occurs.</param>
        void LoadAllBundles (int chunkIndex, int startingBundle, LoadAllBundlesStarted started, LoadAllBundlesFinished finished, AssetBundleMagic.LoadBundleErrorDelegate error)
        {
            if (chunks [chunkIndex].bundleList.Length > startingBundle) {

                if (chunks [chunkIndex].bundleList [startingBundle].fromFile) {
                    AssetBundleMagic.Progress p = AssetBundleMagic.LoadBundle (chunks [chunkIndex].bundleList [startingBundle].bundleName, 
                                                        delegate(AssetBundle ab) {
                            LoadAllBundles (chunkIndex, startingBundle + 1, started, finished, error);
                        });
                    
                    started (p);
                } else {
                    if (chunks [chunkIndex].bundleList [startingBundle].checkVersion) {
                        AssetBundleMagic.DownloadUpdatedBundle (chunks [chunkIndex].bundleList [startingBundle].bundleName,
                            delegate(AssetBundleMagic.Progress progress) {
                                started (progress);
                            },
                            delegate(AssetBundle ab) {
                                currentProgress = null;
                                LoadAllBundles (chunkIndex, startingBundle + 1, started, finished, error);
                            },
                            error
                        );
                    } else {
                        AssetBundleMagic.Progress p = 
                            AssetBundleMagic.DownloadBundle (chunks [chunkIndex].bundleList [startingBundle].bundleName,
                                delegate(AssetBundle ab) {
                                    LoadAllBundles (chunkIndex, startingBundle + 1, started, finished, error);
                                },
                                error
                            );
                        started (p);
                    }
                }
            } else
                finished (chunkIndex);
        }
    }
}