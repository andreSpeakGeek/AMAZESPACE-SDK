using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Newtonsoft.Json;

public class portal : MonoBehaviour
{
    public List<ScenesAvailableDataModel> ScenesAvailable = new List<ScenesAvailableDataModel>();
    public int ID;
#if UNITY_EDITOR
    [Button]
    public void FetchAvailableScenes()
    {
        Utility.StartBackgroundTask(
               Utility.GetRequest(
                   "https://api.amaze-space.com/scene-list", 
                   FetchBoothCallbacks
               ));
    }
    void FetchBoothCallbacks(string response)
    {
        ScenesAvailabletResponseModel ScenesAvailableresponse = JsonConvert.DeserializeObject<ScenesAvailabletResponseModel>(response);
        if (ScenesAvailableresponse.Status == 200)
        {
            ScenesAvailable = ScenesAvailableresponse.data;
            Debug.LogFormat("<color=lime>|Fetch Scene List Response|</color>: Fetched allt he scenes available for portals you can select a destination scene, by entering an index into the ID. Leave the ID as -1 to make the portal go back to the A MAZE ./ SPACE");
        }
        else
        {
            Debug.LogFormat("<color=red>|Fetch Scene List Response|</color>: Something went wrong : {0}", ScenesAvailableresponse.Msg);
        }
    }
#endif
}
#pragma warning disable CS8632
[System.Serializable]
public class ScenesAvailableDataModel
{
    [JsonProperty("title")]
    public string? title;

    [JsonProperty("id")]
    public int ID;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
[System.Serializable]
public class ScenesAvailabletResponseModel
{
    [JsonProperty("status")]
    public int Status;

    [JsonProperty("msg")]
    public string? Msg;

    [JsonProperty("data")]
    public List<ScenesAvailableDataModel>? data;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
#pragma warning restore CS8632