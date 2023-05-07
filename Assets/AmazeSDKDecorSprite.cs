using EasyButtons;
using UnityEngine.Rendering.PostProcessing;
using Newtonsoft.Json;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using Debug = UnityEngine.Debug;
#endif
using UnityEngine;

public class AmazeSDKDecorSprite : MonoBehaviour
{
#if UNITY_EDITOR

    [Tooltip("Try give your sprite a descriptive name")]
    public string decorSpriteName;

    [Tooltip("The x-coordinate of the pivot point of this sprite.")]
    public float xPivot;

    [Tooltip("The x-coordinate of the pivot point of this sprite.")]
    public float yPivot;

    [InspectorName("Pixels Per Unit (PPU)")]
    public float PixelsPerUnit;

    private string youtubeSpriteTutorialURL = "https://www.youtube.com/watch?v=O2P3WRdtUuQ&ab_channel=DALAB";
    private Sprite userSelectedSprite;

    [Button(Spacing = ButtonSpacing.Before | ButtonSpacing.After)]
    public void InitializeDecorSprite(Sprite sprite)
    {
        userSelectedSprite = sprite;
        if (sprite == null)
        {
            Debug.Log("<color=red>Error:</color> The sprite value is null try drag in a sprite from the project folder if your not sure how " +
                "to set up a sprite check out this short video: " + youtubeSpriteTutorialURL);
            return;
        }

        decorSpriteName = sprite.name;
        xPivot = sprite.pivot.x / sprite.rect.width;
        yPivot = sprite.pivot.y / sprite.rect.height;
        PixelsPerUnit = sprite.pixelsPerUnit;

        GameObject amazeRenderer = GameObject.FindGameObjectWithTag("AmazeDecorSprite");
        
        if (amazeRenderer != null && amazeRenderer.GetComponent<SpriteRenderer>() != null)
        {
            amazeRenderer.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            UnityEngine.Object amazeDemoDecor = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AmazeSpriteDecor.prefab", typeof(GameObject));
            GameObject objSource = PrefabUtility.InstantiatePrefab(amazeDemoDecor) as GameObject;
            objSource.transform.SetAsLastSibling();
            objSource.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        string spriteDataPath = AssetDatabase.GetAssetPath(sprite);
        PlayerPrefs.SetString("decorspritepath", spriteDataPath);
        Debug.Log(spriteDataPath);

        Debug.Log("<color=lime>|Initialized Decor Sprite|</color>: Created your Decor Sprite Template, " +
            "The prefab called <color=aqua><b>AmazeSpriteDecor</b></color> in the Hierarchy will give you a live " +
            "preview of what your sprite will look like in the amaze space, All image settings are saved on " +
            "this script. If you would like to modify the values you can <b><i>Select</i></b> your chosen sprite " +
            "in the project folder and <b><i>Select</i></b> the <b><i>Sprite Editor</i></b> and change the settings of the sprite " +
            "as you wish, Once you are happy with the changes in the sprite editor hit the <b><i>Apply</i></b> button and come back " +
            "here and select the <b><i>Update Image Settings</i></b> button" +
            "\n <color=yellow>(Selecting this option again will override your sprite of choice with a new sprite)</color>");
    }

    [Button(Spacing = ButtonSpacing.Before | ButtonSpacing.After)]
    public bool UpdateImageSettings()
    {
        if (userSelectedSprite != null)
        {
            decorSpriteName = userSelectedSprite.name;
            xPivot = userSelectedSprite.pivot.x / userSelectedSprite.rect.width;
            yPivot = userSelectedSprite.pivot.y / userSelectedSprite.rect.height;
            PixelsPerUnit = userSelectedSprite.pixelsPerUnit;
            return true;
        }
        else
        {
            Debug.Log("<color=red>Error:</color> You have not initialized an image use the <b><i>Initialize Decor Sprite</i></b> button to initialize and image");
            return false;
        }
    }

    [Button(Spacing = ButtonSpacing.Before | ButtonSpacing.After)]
    public void uploadDecorSprite()
    {
        if (!UpdateImageSettings())
            return;

        if (PlayerPrefs.HasKey("decorspritepath"))
        {
            Debug.LogFormat("<color=yellow>|File Uploading|</color>: Started uploading sprite for review as : <color=olive><b>{0}</b></color>", $"{PlayerPrefs.GetString("decorspritepath")}");

            WWWForm form = new WWWForm();

            form.AddField("name", decorSpriteName);
            form.AddField("x", xPivot.ToString());
            form.AddField("y", yPivot.ToString());
            form.AddField("ppu", PixelsPerUnit.ToString());
            form.AddBinaryData("photo",
                                File.ReadAllBytes(PlayerPrefs.GetString("decorspritepath")),
                                $"{GetLastItemInUrl(PlayerPrefs.GetString("decorspritepath"))}",
                                "file");

            Utility.StartBackgroundTask(
                    Utility.PostForumRequest(
                        "https://api.amaze-space.com/upload-decor-sprite",
                        form,
                        UploadResponse
                    ));

        }
        else
        {
            Debug.LogFormat("<color=red>|File Upload|</color>: Failed to upload sprite because: {0}", "No Sprite Path Specified");
        }
    }

    public void UploadResponse(string s)
    {
        UploadDecorResponse responseModel = JsonConvert.DeserializeObject<UploadDecorResponse>(s);
        if (responseModel.Status != 200)
        {
            Debug.LogFormat("<color=red>|File Upload|</color>: Failed to upload sprite because: {0}", responseModel.Msg);
        }
        else
        {
            Debug.LogFormat("<color=lime>|File Upload|</color>: Successfully uploaded decor sprite for review as : <color=olive><b>{0}</b></color>", responseModel.Data.Url);

        }
    }

    string GetLastItemInUrl(string url)
    {
        // Split the URL by slashes
        string[] splitUrl = url.Split('/');

        // Get the last item in the split URL
        string lastItem = splitUrl[splitUrl.Length - 1];

        return lastItem;
    }
#endif
}

public class DecorSpriteUploadResponse
{
    [JsonProperty("status")]
    public long Status { get; set; }

    [JsonProperty("msg")]
    public string Msg { get; set; }

    [JsonProperty("data")]
    public DecorSpriteData? Data { get; set; }
}

public class DecorSpriteData
{
    [JsonProperty("decorspriteid")]
    public long Decorspriteid { get; set; }

    [JsonProperty("uuid")]
    public string Uuid { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }
}
