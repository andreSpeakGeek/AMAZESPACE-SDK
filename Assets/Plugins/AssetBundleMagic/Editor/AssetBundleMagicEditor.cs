using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Jacovone.AssetBundleMagic.Util;

namespace Jacovone.AssetBundleMagic
{
    [CustomEditor (typeof(AssetBundleMagic))]
    public class AssetBundleMagicEditor : Editor
    {

        // A style for the main "Build" button
        private GUIStyle buildButtonStyle;

        // A style for headers
        private GUIStyle headerStyle;

        private Dictionary<string,bool> bundleFoldouts;

        private Dictionary<string,string> bundleSearchTerms;

        public override void  OnInspectorGUI ()
        {
            if (bundleFoldouts == null)
                bundleFoldouts = new Dictionary<string,bool> ();
            if (bundleSearchTerms == null)
                bundleSearchTerms = new Dictionary<string,string> ();

            buildButtonStyle = new GUIStyle ("Button");
            buildButtonStyle.fontSize = 14;
            buildButtonStyle.fontStyle = FontStyle.Bold;

            headerStyle = new GUIStyle ("Box");
            headerStyle.fontSize = 12;
            headerStyle.fontStyle = FontStyle.Bold;

            if (EditorGUIUtility.isProSkin) {
                headerStyle.normal.textColor = Color.white;
            } else {
                headerStyle.normal.textColor = Color.black;
            }

            serializedObject.Update ();

            AssetBundleMagic man = (AssetBundleMagic)target;

            EditorGUIUtility.labelWidth = 100;

            EditorGUI.BeginChangeCheck ();

            EditorGUILayout.LabelField ("AssetBundleMagic Version: " + Version.VersionString);
            EditorGUILayout.Space ();
            //EditorGUILayout.LabelField ("General", headerStyle);

            //EditorGUILayout.PropertyField (serializedObject.FindProperty ("BundlesBaseUrl"), new GUIContent ("Bundles Url", "The base url for all bundles to download."));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("BundlesBasePath"), new GUIContent ("Bundles Path", "The complete path for all local bundles. Should include Bundles Directory."));

            //EditorGUIUtility.labelWidth = 200;
            //EditorGUILayout.PropertyField (serializedObject.FindProperty ("DisableHTTPServerCache"), new GUIContent ("Disable HTTP Server Cache", "Tells the HTTP server to not use cache for bundles and versions file."));
            //EditorGUILayout.PropertyField (serializedObject.FindProperty ("TestMode"), new GUIContent ("Work in Test Mode", "When enabled, Test Mode will load bundles locally instead of download them from the network."));
            //EditorGUIUtility.labelWidth = 100;

            EditorGUILayout.LabelField ("Platforms", headerStyle);

            if (!Directory.Exists (man.BundlesBasePath) || new DirectoryInfo (man.BundlesBasePath).GetDirectories ().Length == 0) {
                EditorGUILayout.HelpBox ("Warning: No Bundles. Please Perform a login then press the \"Build All Asset Bundles\" button.", MessageType.Warning);
            }


            //EditorGUILayout.BeginHorizontal ();
            //EditorGUILayout.PropertyField (serializedObject.FindProperty ("BuildIosBundle"), new GUIContent ("iOS Build", "Generate iOS bundles at next build."));
            //EditorGUILayout.PropertyField (serializedObject.FindProperty ("BuildAndroidBundle"), new GUIContent ("Android Build", "Generate iOS bundles at next build."));
            //EditorGUILayout.EndHorizontal ();

            EditorGUILayout.BeginHorizontal ();
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("BuildOSXBundle"), new GUIContent ("macOS Build", "Generate macOS Intel bundles at next build."));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("BuildWindowsBundle"), new GUIContent ("Windows Build", "Generate Windows bundles at next build."));
            EditorGUILayout.EndHorizontal ();

            EditorGUILayout.LabelField ("Bundles", headerStyle);

            string[] allBundles = AssetDatabase.GetAllAssetBundleNames ();

            for (int i = 0; i < allBundles.Length; i++) {

                EditorGUILayout.BeginHorizontal ();

                bool bundleFoldout = false;
                if (!bundleFoldouts.ContainsKey (allBundles [i])) {
                    bundleFoldouts.Add (allBundles [i], false);
                }
                bundleFoldout = bundleFoldouts [allBundles [i]];

                bundleFoldouts [allBundles [i]] = EditorGUILayout.Foldout (bundleFoldout, allBundles [i]);

                uint bundleVersion = 0;
                if (man.BuildVersions.ContainsKey (allBundles [i]))
                    bundleVersion = man.BuildVersions [allBundles [i]];

                EditorGUIUtility.labelWidth = 60;
                bundleVersion = (uint)EditorGUILayout.IntField (new GUIContent ("Version", "The version of the generated bundle."), (int)bundleVersion);
                EditorGUIUtility.labelWidth = 100;
                man.BuildVersions [allBundles [i]] = bundleVersion;

                if (GUILayout.Button (new GUIContent ("+1", "Increment bundle version"), GUILayout.Width (40))) {
                    man.BuildVersions [allBundles [i]] = man.BuildVersions [allBundles [i]] + 1;
                }

                EditorGUILayout.EndHorizontal ();

                if (bundleFoldout) {

                    if (!bundleSearchTerms.ContainsKey (allBundles [i])) {
                        bundleSearchTerms.Add (allBundles [i], "");
                    }
                    string searchTerms = bundleSearchTerms [allBundles [i]];

                    EditorGUIUtility.labelWidth = 50;

                    bundleSearchTerms [allBundles [i]] = EditorGUILayout.TextField ("Search", searchTerms);

                    EditorGUIUtility.labelWidth = 100;

                    string[] searchItems = AssetDatabase.FindAssets (searchTerms + " b:" + allBundles [i]);

                    EditorGUI.indentLevel++;
                    for (int j = 0; j < searchItems.Length; j++) {
                        EditorGUILayout.BeginHorizontal ();
                        EditorGUILayout.ObjectField (
                            AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (searchItems [j]), 
                                AssetDatabase.GetMainAssetTypeAtPath (AssetDatabase.GUIDToAssetPath (searchItems [j]))), 
                            AssetDatabase.GetMainAssetTypeAtPath (AssetDatabase.GUIDToAssetPath (searchItems [j])), false);
                        EditorGUILayout.LabelField (AssetDatabase.GUIDToAssetPath (searchItems [j]));
                        EditorGUILayout.EndHorizontal ();
                    }
                    EditorGUI.indentLevel--;

                }
            }

            if (EditorGUI.EndChangeCheck ()) {
                EditorUtility.SetDirty (target);
            }

            serializedObject.ApplyModifiedProperties ();

            if (GUILayout.Button (new GUIContent ("Build all AssetBundles", "Rebuild all asset bundles."), buildButtonStyle)) {
                BuildABs ();
            }
        }

        void BuildABs ()
        {

            AssetBundleMagic man = (AssetBundleMagic)target;
            string basePathDir = new DirectoryInfo (man.BundlesBasePath).Name;

            if (!AssetDatabase.IsValidFolder (man.BundlesBasePath)) {
                AssetDatabase.CreateFolder ("Assets", basePathDir);
            }

            if (man.BuildOSXBundle) {
                if (!AssetDatabase.IsValidFolder (man.BundlesBasePath + "/" + "macOS")) {
                    AssetDatabase.CreateFolder ("Assets" + "/" + basePathDir, "macOS");
                }
                AssetBundleManifest m = BuildPipeline.BuildAssetBundles (man.BundlesBasePath + "/" + "macOS", 
                                            BuildAssetBundleOptions.None, 
                                            BuildTarget.StandaloneOSX);

                BuildVersionsFileForPlatform (m, man.BundlesBasePath + "/" + "macOS");
            }
            if (man.BuildIosBundle) {
                if (!AssetDatabase.IsValidFolder (man.BundlesBasePath + "/" + "iOS")) {
                    AssetDatabase.CreateFolder ("Assets" + "/" + basePathDir, "iOS");
                }
                AssetBundleManifest m = BuildPipeline.BuildAssetBundles (man.BundlesBasePath + "/" + "iOS", 
                                            BuildAssetBundleOptions.None, 
                                            BuildTarget.iOS);

                BuildVersionsFileForPlatform (m, man.BundlesBasePath + "/" + "iOS");
            }
            if (man.BuildAndroidBundle) {
                if (!AssetDatabase.IsValidFolder (man.BundlesBasePath + "/" + "Android")) {
                    AssetDatabase.CreateFolder ("Assets" + "/" + basePathDir, "Android");
                }
                AssetBundleManifest m = BuildPipeline.BuildAssetBundles (man.BundlesBasePath + "/" + "Android", 
                                            BuildAssetBundleOptions.None, 
                                            BuildTarget.Android);

                BuildVersionsFileForPlatform (m, man.BundlesBasePath + "/" + "Android");
            }
            if (man.BuildWindowsBundle) {
                if (!AssetDatabase.IsValidFolder (man.BundlesBasePath + "/" + "Windows")) {
                    AssetDatabase.CreateFolder ("Assets" + "/" + basePathDir, "Windows");
                }
                AssetBundleManifest m = BuildPipeline.BuildAssetBundles (man.BundlesBasePath + "/" + "Windows", 
                                            BuildAssetBundleOptions.None, 
                                            BuildTarget.StandaloneWindows);

                BuildVersionsFileForPlatform (m, man.BundlesBasePath + "/" + "Windows");
            }
            Debug.LogFormat("<color=lime>|Built Asset Bundles|</color>: Your asset bundles have been Created in the <b><i>_Export</i></b> folder");
        }

        private void BuildVersionsFileForPlatform (AssetBundleManifest m, string basePath)
        {
            AssetBundleMagic man = (AssetBundleMagic)target;

            string[] allBundles = AssetDatabase.GetAllAssetBundleNames ();

            List<VersionData> versionDataList = new List<VersionData> ();

            for (int i = 0; i < allBundles.Length; i++) {

                VersionData vd = new VersionData ();
                vd.bundleName = allBundles [i];

                uint crc;
                BuildPipeline.GetCRCForAssetBundle (basePath + "/" + allBundles [i], out crc);

                vd.version = man.BuildVersions [allBundles [i]];
                vd.crc = crc;

                versionDataList.Add (vd);
            }

            VersionDataCollection versionDataCollection = new VersionDataCollection ();
            versionDataCollection.bundles = versionDataList.ToArray ();

            string versionsContent = JsonUtility.ToJson (versionDataCollection);

            if (versionsContent != null) {
                File.WriteAllText (basePath + "/Versions.txt", versionsContent);
                AssetDatabase.Refresh ();
            }
        }
    }
}
