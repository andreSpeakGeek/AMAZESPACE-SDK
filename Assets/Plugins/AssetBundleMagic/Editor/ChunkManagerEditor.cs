using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace Jacovone.AssetBundleMagic
{
    [CustomEditor (typeof(ChunkManager))]
    public class ChunkManagerEditor : Editor
    {
        private ReorderableList chunkList;
        private ReorderableList bundleList;

        // A style for headers
        private GUIStyle headerStyle;

        private void DefineBundleList ()
        {
            bundleList = new ReorderableList (serializedObject, 
                serializedObject.FindProperty ("chunks").GetArrayElementAtIndex (chunkList.index).FindPropertyRelative ("bundleList"), 
                true, true, true, true);

            bundleList.drawElementCallback = 
                (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = bundleList.serializedProperty.GetArrayElementAtIndex (index);
                rect.y += 2;

                string[] bundleNames = AssetDatabase.GetAllAssetBundleNames ();

                if (bundleNames.Length == 0) {

                    EditorGUI.LabelField (rect, "No asset bundles defined.");
                    bundleList.displayAdd = false;

                } else {
                    List<string> bundleNamesList = new List<string> (bundleNames);
                    int currentBundleIndex = bundleNamesList.IndexOf (element.FindPropertyRelative ("bundleName").stringValue);
                    if (currentBundleIndex == -1)
                        currentBundleIndex = 0;

                    int newIndex = EditorGUI.Popup (
                                       new Rect (rect.x + 2, rect.y, rect.width - 150 - 6, EditorGUIUtility.singleLineHeight),
                                       currentBundleIndex,
                                       bundleNames
                                   );

                    element.FindPropertyRelative ("bundleName").stringValue = bundleNames [newIndex];

                    element.FindPropertyRelative ("fromFile").boolValue = !EditorGUI.ToggleLeft (new Rect (rect.x + rect.width - 150 - 2, rect.y, 60, EditorGUIUtility.singleLineHeight), 
                        new GUIContent ("Remote", "Download this bundle from network insted of a local file in StreamingAssets folder."),
                        !element.FindPropertyRelative ("fromFile").boolValue
                    );

                    if (!element.FindPropertyRelative ("fromFile").boolValue) {
                        element.FindPropertyRelative ("checkVersion").boolValue = EditorGUI.ToggleLeft (new Rect (rect.x + rect.width - 85 - 2, rect.y, 80 - 2, EditorGUIUtility.singleLineHeight), 
                            new GUIContent ("Check ver.", "Check if there is a newer version of this bundle before try to download it."),
                            element.FindPropertyRelative ("checkVersion").boolValue
                        );
                    }

                    bundleList.displayAdd = true;
                }
            };
            bundleList.drawHeaderCallback = 
                (Rect rect) => {
                EditorGUI.LabelField (rect, "Bundle list for scene " + chunkList.index.ToString ());
            };
            bundleList.displayAdd = AssetDatabase.GetAllAssetBundleNames ().Length > 0;
        }

        private void OnEnable ()
        {
            chunkList = new ReorderableList (serializedObject, 
                serializedObject.FindProperty ("chunks"), 
                true, true, true, true);
           
            chunkList.drawElementCallback = 
                (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = chunkList.serializedProperty.GetArrayElementAtIndex (index);
                rect.y += 2;

                EditorGUI.LabelField (
                    new Rect (rect.x + 2, rect.y, 50, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative ("sceneName").stringValue);
                EditorGUI.PropertyField (
                    new Rect (rect.x + 52, rect.y, rect.width - 4 - 50, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative ("center"), GUIContent.none);
            };

            chunkList.drawHeaderCallback = 
                (Rect rect) => {
                EditorGUI.LabelField (rect, "Chunks");
            };

            chunkList.onAddCallback = (ReorderableList l) => {
                var index = l.serializedProperty.arraySize;
                l.serializedProperty.arraySize++;
                l.index = index;
                var element = l.serializedProperty.GetArrayElementAtIndex (index);
                element.FindPropertyRelative ("bundleList").ClearArray ();
                serializedObject.ApplyModifiedProperties ();
            };

            chunkList.onChangedCallback = (ReorderableList l) => {
                DefineBundleList ();
            };

            chunkList.onSelectCallback = (ReorderableList l) => {
                DefineBundleList ();
            };

        }

        public override void OnInspectorGUI ()
        {
            headerStyle = new GUIStyle ("Box");
            headerStyle.fontSize = 12;
            headerStyle.fontStyle = FontStyle.Bold;

            EditorGUIUtility.labelWidth = 100;

            if (EditorGUIUtility.isProSkin) {
                headerStyle.normal.textColor = Color.white;
            } else {
                headerStyle.normal.textColor = Color.black;
            }

            serializedObject.Update ();

            EditorGUILayout.PropertyField (serializedObject.FindProperty ("subject"), new GUIContent ("Subject", "The subject to follow."));
            EditorGUILayout.PropertyField (serializedObject.FindProperty ("interval"), new GUIContent ("Interval", "The number of seconds between one check and the next."));
            EditorGUILayout.Slider(serializedObject.FindProperty("distanceBias"), .1f, 5f, new GUIContent("Distance Bias", "Adjust chunk distances with this multiply factor."));

            EditorGUIUtility.labelWidth = 200;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("downloadVersionsAtStartup"), new GUIContent("Download versions at startup", "Download bundles versions at startup through AssetBundleMagic."));
            EditorGUIUtility.labelWidth = 100;


            chunkList.DoLayoutList ();

            if (chunkList.index != -1) {
                var element = chunkList.serializedProperty.GetArrayElementAtIndex (chunkList.index);

                EditorGUILayout.LabelField ("Chunk properties of " + element.FindPropertyRelative ("sceneName").stringValue, headerStyle);

                EditorGUILayout.BeginVertical ("Box");

                EditorGUILayout.PropertyField (element.FindPropertyRelative ("sceneName"), new GUIContent ("Scene name", "The name of the scene o load."));
                EditorGUILayout.PropertyField (element.FindPropertyRelative ("loadDistance"), new GUIContent ("Load distance", "The distance from the chunk center at which load the scene."));
                EditorGUILayout.PropertyField (element.FindPropertyRelative ("unloadDistance"), new GUIContent ("Unload distance", "The distance from the chunk center at which unload the scene."));

                EditorGUILayout.PropertyField (element.FindPropertyRelative ("onLoad"), new GUIContent ("On load events", "Events fired on chunk load."));
                EditorGUILayout.PropertyField (element.FindPropertyRelative ("onUnload"), new GUIContent ("On unload events", "Events fired on chunk unload."));


                if (AssetDatabase.GetAllAssetBundleNames ().Length == 0) {
                    EditorGUILayout.HelpBox ("There are not defined asset bundles. Define one or more asset bundle name to add it to the chunk.", MessageType.Warning);
                    element.FindPropertyRelative ("bundleList").ClearArray ();
                } else {
                    bundleList.DoLayoutList ();
                }

                EditorGUILayout.EndVertical ();
            }

            serializedObject.ApplyModifiedProperties ();
            SceneView.RepaintAll ();
        }

        private void OnSceneGUI ()
        {
            ChunkManager cm = (ChunkManager)target;
            serializedObject.Update ();
	
            if (chunkList.index == -1) {
                
                for (int i = 0; i < cm.chunks.Length; i++) {
                    DrawChunkHandlers (i);
                }
            } else {
                DrawChunkHandlers (chunkList.index);
            }

            serializedObject.ApplyModifiedProperties ();
        }

        void DrawChunkHandlers (int chunkIndex)
        {
            ChunkManager cm = (ChunkManager)target;

            SerializedProperty center = serializedObject.FindProperty ("chunks").GetArrayElementAtIndex (chunkIndex)
                .FindPropertyRelative ("center");
            SerializedProperty loadDistance = serializedObject.FindProperty ("chunks").GetArrayElementAtIndex (chunkIndex)
                .FindPropertyRelative ("loadDistance");
            SerializedProperty unloadDistance = serializedObject.FindProperty ("chunks").GetArrayElementAtIndex (chunkIndex)
                .FindPropertyRelative ("unloadDistance");

            Handles.color = Color.magenta;
            Handles.DrawLine (cm.chunks [chunkIndex].center,
                cm.chunks [chunkIndex].center + cm.chunks [chunkIndex].unloadDistance * Vector3.up);

            Vector3 newPos = Handles.PositionHandle (cm.chunks [chunkIndex].center, Quaternion.identity);
            center.vector3Value = newPos;

            Handles.Label (cm.chunks [chunkIndex].center, "scene: " + cm.chunks [chunkIndex].sceneName);

            Handles.color = new Color (0f, 0.8f, 0f, 0.9f);

            float newLoadDistance = Handles.RadiusHandle (
                                        Quaternion.identity,
                                        cm.chunks [chunkIndex].center,
                                        cm.chunks [chunkIndex].loadDistance * cm.distanceBias) / cm.distanceBias;

            loadDistance.floatValue = newLoadDistance;

            if (newLoadDistance >= (unloadDistance.floatValue - 1f))
                unloadDistance.floatValue = newLoadDistance + 1f;

            Handles.color = new Color (8.0f, 0f, 0.0f, 0.9f);

            float newUnloadDistance = Handles.RadiusHandle (
                                          Quaternion.identity,
                                          cm.chunks [chunkIndex].center,
                                          cm.chunks [chunkIndex].unloadDistance * cm.distanceBias
                                      ) / cm.distanceBias;

            if (newUnloadDistance <= 2f)
                newUnloadDistance = 2f;

            unloadDistance.floatValue = newUnloadDistance;

            if (newUnloadDistance <= (loadDistance.floatValue + 1f))
                loadDistance.floatValue = newUnloadDistance - 1f;

            if (loadDistance.floatValue <= 1f) {
                loadDistance.floatValue = 1f;
            }
        }
    }
}