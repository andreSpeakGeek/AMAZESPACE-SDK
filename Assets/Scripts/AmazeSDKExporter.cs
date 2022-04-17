using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AmazeSDKExporter : MonoBehaviour
{
#if UNITY_EDITOR
    [EasyButtons.Button]
    public void ExportBoothPrefabAsPackage()
    {
        if (PlayerPrefs.HasKey("prefabpath") && PlayerPrefs.HasKey("assetbundlename"))
        {

            var exportedPackageAssetList = new List<string>();
            exportedPackageAssetList.Add(PlayerPrefs.GetString("prefabpath"));
            //Export Shaders and Prefabs with their dependencies into a .unitypackage
            AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), $"Assets/_Export/{PlayerPrefs.GetString("assetbundlename")}.unitypackage",
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
            Debug.LogFormat("<color=green>|Export Package|</color>: Exported your Prefab and all associated assets as the Package <color=fuchsia><b>{0}</b></color>", $"./Assets/_Export/{PlayerPrefs.GetString("assetbundlename")}.unitypackage");
        }
        else
        {
            Debug.LogFormat("<color=red>|Export Package|</color>: Failed to export Package");
        }
    }
#endif
}
