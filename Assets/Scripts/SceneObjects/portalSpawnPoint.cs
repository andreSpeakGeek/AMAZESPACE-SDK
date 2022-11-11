using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Newtonsoft.Json;

public class portalSpawnPoint : MonoBehaviour
{
    public List<ScenesAvailableDataModel> ScenesAvailable = new List<ScenesAvailableDataModel>();

    public int PortalFrom;
    public bool y_only;
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
