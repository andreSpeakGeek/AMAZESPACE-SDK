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
        bool GeneratedPackage = false;
        if (PlayerPrefs.HasKey("prefabpath") && PlayerPrefs.HasKey("assetbundlename"))
        {
            var exportedPackageAssetList = new List<string>();
            exportedPackageAssetList.Add(PlayerPrefs.GetString("prefabpath"));
            Debug.Log(exportedPackageAssetList[0].ToString());
            //Export Shaders and Prefabs with their dependencies into a.unitypackage
            AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), $"Assets/_Export/{PlayerPrefs.GetString("assetbundlename")}.unitypackage",
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
            Debug.LogFormat("<color=lime>|Export Package|</color>: Exported your Prefab and all associated assets as the Package <color=fuchsia><b>{0}</b></color>", $"./Assets/_Export/{PlayerPrefs.GetString("assetbundlename")}.unitypackage");
            GeneratedPackage = true;
        }

        if (PlayerPrefs.HasKey("scenename"))
        {
            var exportedPackageAssetList = new List<string>();
            exportedPackageAssetList.Add($"Assets/_Export/{PlayerPrefs.GetString("scenename")}.unity");
            //Export Scene with it's dependencies into a .unitypackage
            AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), $"Assets/_Export/{PlayerPrefs.GetString("scenename")}_package.unitypackage",
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
            Debug.LogFormat("<color=lime>|Export Package|</color>: Exported your Prefab and all associated assets as the Package <color=fuchsia><b>{0}</b></color>", $"Assets/_Export/{PlayerPrefs.GetString("scenename")}.unity");
            GeneratedPackage = true;
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        if (!GeneratedPackage)
        {
            Debug.LogFormat("<color=red>|Export Package|</color>: Failed to export Package");
        }
    }
#endif
}
