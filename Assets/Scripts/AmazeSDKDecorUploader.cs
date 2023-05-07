using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using EasyButtons;
using System;

public class AmazeSDKDecorUploader : MonoBehaviour
{
#if UNITY_EDITOR
    [Button]
    public void ZipUpDecorAssets()
    {
        string ExportPath = Application.dataPath + @"/_Export";
        if (PlayerPrefs.HasKey("decorassetbundlename"))
        {
            string ZipPath = Application.dataPath + "/" + PlayerPrefs.GetString("decorassetbundlename") + ".zip";
            if (!File.Exists(ZipPath))
            {
                Debug.LogFormat("<color=yellow>|Zipping up decor assets|</color>: Started Creating Zip at: <color=fuchsia><b>{0}</b></color>", ZipPath);
                ZipFile.CreateFromDirectory(ExportPath, ZipPath);
                PlayerPrefs.SetString("decorzippath", ZipPath);
                Debug.LogFormat("<color=lime>|Zipped up decor assets|</color>: Created Zip at: <color=fuchsia><b>{0}</b></color>", ZipPath);
            }
            else
            {
                Debug.LogFormat("<color=red>|Zipped up decor assets|</color>: Failed Creating Zip at: <color=fuchsia><b>{0}</b></color> as the " +
                    "file already exists \n <color=yellow>(To Replace the Zip with a new one please manually delete the current one, potentially " +
                    "after making backups)</color>", ZipPath);
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
    [Button]
    public void UploadDecorZip()
    {
        if (PlayerPrefs.HasKey("decorassetbundlename") && PlayerPrefs.HasKey("decorzippath"))
        {
            Debug.LogFormat("<color=yellow>|File Uploading|</color>: Started uploading zip for review as : <color=olive><b>{0}</b></color>", $"{PlayerPrefs.GetString("decorassetbundlename")}.zip");

            WWWForm form = new WWWForm();

            form.AddField("assetbundlename", PlayerPrefs.GetString("decorName"));
            form.AddBinaryData("file", 
                                File.ReadAllBytes(PlayerPrefs.GetString("decorzippath")), 
                                $"{PlayerPrefs.GetString("decorassetbundlename")}.zip", 
                                "file");
            Utility.StartBackgroundTask(
                    Utility.PostForumRequest(
                        "https://api.amaze-space.com/upload-decor",
                        form,
                        UploadResponse
                    ));

        }
        else
        {
            Debug.LogFormat("<color=red>|File Upload|</color>: Failed to upload zip because: {0}", "No AssetbundleName or Zip Path Specified");
        }
    }

    public void UploadResponse(string s)
    {
        UploadDecorResponse responseModel = JsonConvert.DeserializeObject<UploadDecorResponse>(s);
        if (responseModel.Status != 200)
        {
            Debug.LogFormat("<color=red>|File Upload|</color>: Failed to upload zip because: {0}", responseModel.Msg);
        }
        else
        {
            Debug.LogFormat("<color=lime>|File Upload|</color>: Successfully uploaded decor zip for review as : <color=olive><b>{0}</b></color>", responseModel.Data.Url);

        }
    }
#endif
}
[System.Serializable]
public class UploadDecorResponse
{
    [JsonProperty("status")]
    public long Status { get; set; }

    [JsonProperty("msg")]
    public string Msg { get; set; }

    [JsonProperty("data")]
    public DecorData? Data { get; set; }
}
[System.Serializable]
public class DecorData 
{
    [JsonProperty("decorid")]
    public long Decorid { get; set; }

    [JsonProperty("uuid")]
    public string Uuid { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }
}