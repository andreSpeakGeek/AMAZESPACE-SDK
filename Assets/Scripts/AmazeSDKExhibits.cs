using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using EasyButtons;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif
using Newtonsoft.Json;

public class AmazeSDKExhibits : MonoBehaviour
{

    public List<exhibitNameDataModel> MyExhibits = new List<exhibitNameDataModel>();
#if UNITY_EDITOR
    [Button]
    public void FetchMyExhibits() 
    {
        Utility.StartBackgroundTask(
               Utility.GetRequest(
                   "https://api.amaze-space.com/exhbitsForUserArtist",
                   FetchBoothCallbacks
               ));
    }
    void FetchBoothCallbacks(string response) 
    {
        ExhibitByUserArtistResponseModel exhibitByUserArtist = JsonConvert.DeserializeObject<ExhibitByUserArtistResponseModel>(response);
        if (exhibitByUserArtist.Status == 200)
        {
            MyExhibits = exhibitByUserArtist.data;  
            Debug.LogFormat("<color=lime>|Fetch Exhibits Response|</color>: Fetched your exhibits, You can create a template for any of your available Booths, by selecting an index and clicking invoke");
        }
        else
        {
            Debug.LogFormat("<color=red>|Fetch Exhibits  Response|</color>: Something went wrong : {0}", exhibitByUserArtist.Msg);
        }
    }
    [Button]
    public void InitialiseBooth(int index) {
        string prefabname = $"Assets/Prefabs/{MyExhibits[index].Name.ToLower().Trim().Replace(' ','-')}_booth.prefab";
        PlayerPrefs.SetString("prefabpath", prefabname);
        
        UnityEngine.Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/DefaultBooth/BoothPrefabs/AmazeBooth.prefab", typeof(GameObject));
        GameObject objSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
        GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objSource, prefabname);

        string AssetBundleName = MyExhibits[index].Name.ToLower().Trim().Replace(' ', '-');
        var importer = UnityEditor.AssetImporter.GetAtPath(prefabname);
        importer.assetBundleName = AssetBundleName;
        PlayerPrefs.SetString("assetbundlename", AssetBundleName);

        DestroyImmediate(objSource);
        GameObject NewVariantInstance = PrefabUtility.InstantiatePrefab(prefabVariant) as GameObject;
        NewVariantInstance.transform.SetAsLastSibling();

        Debug.LogFormat("<color=lime>|Initialised Booth|</color>: Created your Booth Template, <b><i>Select</i></b> the prefab called <color=aqua><b>{0}</b></color> in the Hierarchy, Click <b><i>Open</i></b> in the Inspector and Start Building! \n <color=yellow>(Selecting this option again will override your prefab with a fresh template)</color>", AssetBundleName+ "_booth");
        string BoothURL = "https://dashboard.amaze-space.com/games?game=" + MyExhibits[index].ID;
        Application.OpenURL(BoothURL);
        Debug.LogFormat("<color=lime>|Opened Game On Dashboard|</color>: Opened your Browser at: <color=aqua><b>{0}</b></color>", BoothURL);
    }
#endif
}
#if UNITY_EDITOR
public static class Utility
{
    public static void StartBackgroundTask(IEnumerator update, Action end = null)
    {
        EditorApplication.CallbackFunction closureCallback = null;

        closureCallback = () =>
        {
            try
            {
                if (update.MoveNext() == false)
                {
                    if (end != null)
                        end();
                    EditorApplication.update -= closureCallback;
                }
            }
            catch (Exception ex)
            {
                if (end != null)
                    end();
                Debug.LogException(ex);
                EditorApplication.update -= closureCallback;
            }
        };

        EditorApplication.update += closureCallback;
    }
    public static IEnumerator PostRequest(string url, string body, System.Action<string> TextContentCallback)
    {
        using (UnityWebRequest w = new UnityWebRequest(url,"POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);

            w.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            w.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            w.SetRequestHeader("Content-Type", "application/json");
            yield return w.SendWebRequest();

            while (w.isDone == false)
                yield return null;

            TextContentCallback(w.downloadHandler.text);
        }
    }
    public static IEnumerator PostForumRequest(string url, WWWForm body, System.Action<string> TextContentCallback)
    {
        using (var w = UnityWebRequest.Post(url, body))
        {
            w.SetRequestHeader("x-access-token", PlayerPrefs.GetString("token"));
            yield return w.SendWebRequest();

            while (w.isDone == false)
                yield return null;

            TextContentCallback(w.downloadHandler.text);
        }
    }
    public static IEnumerator GetRequest(string url, System.Action<string> TextContentCallback)
    {
        using (UnityWebRequest w = new UnityWebRequest(url, "GET"))
        {
            w.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            w.SetRequestHeader("Content-Type", "application/json");
            if (PlayerPrefs.HasKey("token"))
            {
                w.SetRequestHeader("x-access-token", PlayerPrefs.GetString("token"));
            }
            yield return w.SendWebRequest();

            while (w.isDone == false)
                yield return null;

            TextContentCallback(w.downloadHandler.text);
        }
    }
    public static userTokenModel DecodeToken(string s)
    {
        var parts = s.Split('.');
        if (parts.Length > 2)
        {
            var decode = parts[1];
            var padLength = 4 - decode.Length % 4;
            if (padLength < 4)
            {
                decode += new string('=', padLength);
            }
            var bytes = System.Convert.FromBase64String(decode);
            string userInfo = Encoding.ASCII.GetString(bytes);

            return JsonConvert.DeserializeObject<userTokenModel>(userInfo);
        }
        else return null;
    }
}
#endif
#pragma warning disable CS8632
[System.Serializable]
public class LoginResponseModel
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("msg")]
    public string? Msg;

    [JsonProperty("token")]
    public string? Token;

    [JsonProperty("naviagation")]
    public string? Naviagation;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
[System.Serializable]
public class RegisterResponseModel
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("msg")]
    public string? Msg;

    [JsonProperty("id")]
    public string? id;

    [JsonProperty("token")]
    public string? Token;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
public class userTokenModel
{
    [JsonProperty("userid")]
    public int Userid { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("userType")]
    public int UserType { get; set; }

    [JsonProperty("userRestriction")]
    public int UserRestriction { get; set; }
    [JsonProperty("teamID")]
    public int? teamID { get; set; }
    [JsonProperty("spaceID")]
    public int? spaceid { get; set; }

    [JsonProperty("iat")]
    public long Iat { get; set; }

    [JsonProperty("exp")]
    public long Exp { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
[System.Serializable]
public class ExhibitByUserArtistResponseModel
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("msg")]
    public string? Msg;

    [JsonProperty("data")]
    public List<exhibitNameDataModel>? data;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
[System.Serializable]
public class exhibitNameDataModel 
{
    [JsonProperty("name")]
    public string? Name;

    [JsonProperty("id")]
    public int ID;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
#pragma warning restore CS8632