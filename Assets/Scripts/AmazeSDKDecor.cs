using UnityEngine;
using EasyButtons;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AmazeSDKDecor : MonoBehaviour
{
#if UNITY_EDITOR
    [Button]
    public void InitializeDecor(string decorName)
    {
        if (string.IsNullOrEmpty(decorName))
        {
            Debug.Log("Decor name can not be empty");
            return;
        }

        string prefabname = $"Assets/Prefabs/{decorName.ToLower().Trim().Replace(' ', '-')}_decor.prefab";
        PlayerPrefs.SetString("decorName", decorName);
        PlayerPrefs.SetString("decorprefabpath", prefabname);

        UnityEngine.Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AmazeDecor.prefab", typeof(GameObject));
        GameObject objSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
        GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objSource, prefabname);

        foreach (string assetBundle in AssetDatabase.GetAllAssetBundleNames())
        {
            AssetDatabase.RemoveAssetBundleName(assetBundle, true);
        }
        AssetDatabase.Refresh();

        string DecorAssetBundleName = $"{decorName.ToLower().Trim().Replace(' ', '-')}_decor";
        var importer = UnityEditor.AssetImporter.GetAtPath(prefabname);
        importer.assetBundleName = DecorAssetBundleName;
        PlayerPrefs.SetString("decorassetbundlename", DecorAssetBundleName);

        DestroyImmediate(objSource);
        GameObject NewVariantInstance = PrefabUtility.InstantiatePrefab(prefabVariant) as GameObject;
        NewVariantInstance.transform.SetAsLastSibling();

        Debug.LogFormat("<color=lime>|Initialized Decor|</color>: Created your Decor Template, " +
            "<b><i>Select</i></b> the prefab called <color=aqua><b>{0}</b></color> in the Hierarchy, " +
            "Click <b><i>Open</i></b> in the Inspector and Start Building!" +
            "\n <color=yellow>(Selecting this option again will override your prefab with a fresh template)</color>", DecorAssetBundleName);
    }
#endif
}
