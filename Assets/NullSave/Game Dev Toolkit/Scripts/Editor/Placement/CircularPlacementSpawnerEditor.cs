using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK {
    [CustomEditor(typeof(CircularPlacementSpawner))]
    public class CircularPlacementSpawnerEditor : GDTKEditor
    {

        #region Fields

        Dictionary<int, bool> topEntries;
        Tuple<int, bool> result;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            topEntries = new Dictionary<int, bool>();
        }

        public override void OnInspectorGUI()
        {
            int delAt = -1;

            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("animateSpawn");
            if(SimpleValue<bool>("animateSpawn"))
            {
                SimpleProperty("duration");
                SimpleProperty("yCurve");
            }

            SectionHeader("Spawn Table");
            SerializedProperty list = serializedObject.FindProperty("entries");
            for(int i=0; i < list.arraySize; i++)
            {
                if(!topEntries.ContainsKey(i))
                {
                    topEntries.Add(i, false);
                }

                result = ExpandableListItem("Entry " + (i + 1), topEntries[i]);

                if(topEntries[i])
                {
                    GUILayout.BeginVertical(Styles.SubSectionBox);
                    DrawEntry(list.GetArrayElementAtIndex(i));
                    GUILayout.EndVertical();
                }

                if(result.Item1 == -1)
                {
                    topEntries[i] = false;
                }
                else if(result.Item1 == 1)
                {
                    topEntries[i] = true;
                }
                if(result.Item2)
                {
                    delAt = i;
                }

            }

            GUILayout.Space(4);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                list.arraySize++;
            }

            EditorGUI.BeginDisabledGroup(list.arraySize == 0);
            if (GUILayout.Button("Clear"))
            {
                list.arraySize = 0;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();

            if (delAt > -1)
            {
                list.DeleteArrayElementAtIndex(delAt);
            }

            SectionHeader("Broadcast Receiver");
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            SimpleProperty("respondToBroadcasts", "Listen");
            if(SimpleValue<bool>("respondToBroadcasts"))
            {
                SimpleProperty("spawnMessage");
                SimpleProperty("usePublicChannel");
                if (!SimpleValue<bool>("usePublicChannel"))
                {
                    SimpleProperty("channelName");
                }
            }
            EditorGUI.EndDisabledGroup();

            if(Application.isPlaying)
            {
                GUILayout.Space(12);
                if(GUILayout.Button("Spawn"))
                {
                    ((CircularPlacementSpawner)target).SpawnItems();
                }
            }

            MainContainerEnd();
        }

        #endregion

        #region Private Methods

        private void DrawEntry(SerializedProperty item)
        {
            int delAt = -1;
            SimpleProperty(item, "Weight");

            SubHeader("Items");
            SerializedProperty list = item.FindPropertyRelative("Items");
            SerializedProperty listItem;
            for(int i=0; i < list.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                if(GUILayout.Button("X"))
                {
                    delAt = i;
                }
                listItem = list.GetArrayElementAtIndex(i);
                GUILayout.BeginVertical();
                SimpleProperty(listItem, "Object");
                SimpleProperty(listItem, "UseRandomCount");
                if(SimpleValue<bool>(listItem, "UseRandomCount"))
                {
                    SimpleProperty(listItem, "MinCount");
                    SimpleProperty(listItem, "MaxCount");
                }
                else
                {
                    SimpleProperty(listItem, "Count");
                }
                SimpleProperty(listItem, "randomRotation");
                if (SimpleValue<bool>(listItem, "randomRotation"))
                {
                    SimpleProperty(listItem, "minRotation");
                    SimpleProperty(listItem, "maxRotation");
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(4);
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Add"))
            {
                list.arraySize++;
            }

            EditorGUI.BeginDisabledGroup(list.arraySize == 0);
            if (GUILayout.Button("Clear"))
            {
                list.arraySize = 0;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();

            if(delAt > -1)
            {
                list.DeleteArrayElementAtIndex(delAt);
            }
        }

        #endregion

    }
}