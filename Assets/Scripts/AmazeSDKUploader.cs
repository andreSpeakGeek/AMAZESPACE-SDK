using UnityEngine;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;

public class AmazeSDKUploader : MonoBehaviour
{
#if UNITY_EDITOR
    [EasyButtons.Button]
    public void ZipUpExportFolder()
    {
        string ExportPath = Application.dataPath + @"/_Export";
        if (PlayerPrefs.HasKey("assetbundlename"))
        {
            string ZipPath = Application.dataPath + "/" + PlayerPrefs.GetString("assetbundlename") + ".zip";
            if (!File.Exists(ZipPath))
            {

                ZipFile.CreateFromDirectory(ExportPath, ZipPath);
                PlayerPrefs.SetString("zippath", ZipPath);
                Debug.LogFormat("<color=green>|Zipped up _Export folder|</color>: Created Zip at: <color=fuchsia><b>{0}</b></color>", ZipPath);
            }
            else
            { 
                Debug.LogFormat("<color=red>|Zipped up _Export folder|</color>: Failed Creating Zip at: <color=fuchsia><b>{0}</b></color> as the file already exists \n <color=yellow>(To Replace the Zip with a new one please manually delete the current one, potentially after making backups)</color>", ZipPath);
            }

        }
    }
    [EasyButtons.Button]
    public void UploadZip() 
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData("assetbundle", File.ReadAllBytes(PlayerPrefs.GetString("zippath")), $"{ PlayerPrefs.GetString("assetbundlename")}.zip", "file");
        Utility.StartBackgroundTask(
                Utility.PostForumRequest(
                    "https://api.amaze-space.com/UploadAssetBundle",
                    form,
                    UploadResponse
                ));
    }
    public void UploadResponse(string s) 
    {
        FileUploadResponseModel responseModel = JsonConvert.DeserializeObject<FileUploadResponseModel>(s);
        if (responseModel.Status != 200)
        {
            Debug.LogFormat("<color=red>|File Upload|</color>: Failed to upload zip because: {0}", responseModel.Msg);
        }
        else 
        {
            Debug.LogFormat("<color=green>|File Upload|</color>: Successfully uploaded zip for review as : <color=olive><b>{0}</b></color>", responseModel.data.uploadedAs);

        }
    }
#endif
}
#pragma warning disable CS8632
[System.Serializable]
public class FileUploadResponseModel
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("msg")]
    public string? Msg;

    [JsonProperty("data")]
    public FileUploadDataModel? data;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
[System.Serializable]
public class FileUploadDataModel
{
    [JsonProperty("uploadedAs")]
    public string? uploadedAs;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
#pragma warning restore CS8632