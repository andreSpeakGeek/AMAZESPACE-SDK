using EasyButtons;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class AmazeSDKChooseOption : MonoBehaviour
{
#if UNITY_EDITOR
    [Button]
    public void CreateAnExhibits()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/AssetbundleCreation.unity", OpenSceneMode.Single);
    }
    [Button]
    public void CreateDecor()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/AmazeDecorCreation.unity", OpenSceneMode.Single);
    }
    [Button]
    public void CreateDecorSprite()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/AmazeSpriteCreation.unity", OpenSceneMode.Single);
    }
    [Button]
    public void CreateAScene(string name)
    {
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        string scenepath = $"{Application.dataPath}/_Export/{name}_PortalScene.unity";
        EditorSceneManager.SaveScene(newScene, scenepath);
        PlayerPrefs.SetString("scenepath", scenepath);

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

        var importer = UnityEditor.AssetImporter.GetAtPath($"Assets/_Export/{name}_PortalScene.unity");
        string sceneassetbundlename = $"{name}_PortalScene";
        PlayerPrefs.SetString("scenename", sceneassetbundlename);
        importer.assetBundleName = sceneassetbundlename.ToLower().Trim().Replace(' ', '-');
        PlayerPrefs.SetString("assetbundlename", importer.assetBundleName);

        Debug.LogFormat("<color=lime>|Chose Scene Creation|</color>: Created your New Scene Called: <color=aqua>{0}</color> in the /_Export folder.",$"{name}_PortalScene");
        Debug.LogFormat("<color=lime>|Create Your Scene|</color>: The scene <color=aqua>{0}</color> has been opened, build your environment here. Afterwards Return the the SDK Scene", $"{name}_PortalScene");

    }
#endif
}
