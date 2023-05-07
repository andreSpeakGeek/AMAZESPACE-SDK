using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using EasyButtons;
using UnityEngine.Networking;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif
using Newtonsoft.Json;
public class AmazeSDKLogin:MonoBehaviour {
    [Header("Login")]
    public string Email;    
    public string Password;
    public bool HasLoggedIn = false;
    public static LoginResponseModel LoginResponse;
    private const string version = "0.0.4";

#if UNITY_EDITOR
    [Button]
    public void Login()
    {
        Utility.StartBackgroundTask(
            Utility.PostRequest(
                "https://api.amaze-space.com/login",
                @$"{{""email"":""{Email}"",""password"":""{Password}"",""tags"":""sdk|v{version}""}}",
                LoginCallback
            ));
    }
    void LoginCallback(string response)
    {
        LoginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(response);
        if (LoginResponse.Status == 200)
        {
            HasLoggedIn = true;
            Debug.LogFormat("<color=lime>|Login Response|</color>: {0}", LoginResponse.Msg);
            PlayerPrefs.SetString("token", LoginResponse.Token);
            PlayerPrefs.SetInt("Loggedin", 0);
        }
        else
        {
            Debug.LogFormat("<color=red>|Login Response|</color>: Something went wrong : {0}", LoginResponse.Msg);
        }
        //if (LoginResponse.Naviagation == "exhibits")
        //{
        //    EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/AssetbundleCreation.unity", OpenSceneMode.Single);
        //}
    }

    [ContextMenu("ResetSDK")]
    public void ResetSDK() {
        PlayerPrefs.DeleteAll(); 
        Directory.Delete(Application.dataPath+ "/_Export", true);
        Directory.CreateDirectory(Application.dataPath + "/_Export");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }

#endif
}