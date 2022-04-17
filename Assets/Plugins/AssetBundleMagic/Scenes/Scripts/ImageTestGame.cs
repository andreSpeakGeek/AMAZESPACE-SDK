using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Jacovone.AssetBundleMagic;
using UnityEngine.UI;

public class ImageTestGame : MonoBehaviour
{
    public Image progress;
    public Text percentageText;
    public Text errorText;
    public Button clearCacheButton;
    public Button loadSDButton;
    public Button loadHDButton;

    private AssetBundleMagic.Progress p = null;

    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = 60;

        AssetBundleMagic.UnloadBundle ("images.hd", true);
        AssetBundleMagic.UnloadBundle ("images.sd", true);
        AssetBundleMagic.UnloadBundle ("scenes", true);

        clearCacheButton.enabled = false;
        loadHDButton.enabled = false;
        loadSDButton.enabled = false;
        percentageText.enabled = false;
        errorText.enabled = false;

        AssetBundleMagic.DownloadVersions (delegate(string versions) {
            Debug.Log ("Received versions:\n" + versions);

            clearCacheButton.enabled = true;
            loadHDButton.enabled = true;
            loadSDButton.enabled = true;

        }, delegate(string error) {
            errorText.enabled = true;
            errorText.text = "Error: " + error;
            Debug.LogError (error);
        });

    }

    public void ClearCache ()
    {
        AssetBundleMagic.CleanBundlesCache ();
    }

    public void LoadSD ()
    {
        LoadBundles (false);
    }

    public void LoadHD ()
    {
        LoadBundles (true);
    }

    void LoadBundles (bool hd)
    {
        percentageText.enabled = true;
        errorText.enabled = false;

        string imagesBundleName = hd ? "images.hd" : "images.sd";

        p = AssetBundleMagic.DownloadBundle (
            imagesBundleName,
            delegate(AssetBundle ab) {

                p = AssetBundleMagic.DownloadBundle (
                    "scenes",
                    delegate(AssetBundle ab2) {
                        SceneManager.LoadScene ("ImagesTest2");    
                    },
                    delegate(string error) {
                        errorText.enabled = true;
                        errorText.text = "Error: " + error;
                        Debug.LogError (error);
                    }
                );

            },
            delegate(string error) {
                errorText.enabled = true;
                errorText.text = "Error: " + error;
                Debug.LogError (error);
            }
        );
    }

    // Update is called once per frame
    void Update ()
    {
        if (p != null) {

            float pr = p.GetProgress ();
            if (pr < 0)
                pr = 0;
            progress.fillAmount = pr;
            percentageText.text = ((int)(100 * pr)).ToString () + "%";
        }
    }
}
