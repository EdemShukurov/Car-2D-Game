#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(SpriteShapeDisplayer))]
public class GroundEditor : Editor
{
    //Prefab rules
    private SerializedObject _spriteShapeDisplayer;

    private SerializedProperty _prefabRules;

    private List<bool> _prefabRulesExpanded;


    private static GUIContent _addPrefabButton = new GUIContent("Add Prefab +", "Add Prefab");
    private static GUIContent _removePrefabButton = new GUIContent("Delete", "Delete Prefab");

    private void OnEnable()
    {
        _spriteShapeDisplayer = new SerializedObject(target);
        _prefabRules = _spriteShapeDisplayer.FindProperty("PrefabRules");
        _prefabRulesExpanded = new List<bool>();

    }

    public override void OnInspectorGUI()
    {

        if (GUILayout.Button(_addPrefabButton))
        {
            _prefabRules.arraySize += 1;
            _prefabRules.InsertArrayElementAtIndex(_prefabRules.arraySize);
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();


        //Loop through the terrain generation rules
        for (int i = 0; i < _prefabRules.arraySize; i++)
        {
            EditorGUILayout.Space();

            SerializedProperty prefabRule = _prefabRules.GetArrayElementAtIndex(i);

            //Get the properites of each prefab
            SerializedProperty prefabToClone = prefabRule.FindPropertyRelative("PrefabToClone");

            SerializedProperty minOffset = prefabRule.FindPropertyRelative("MinOffset");
            SerializedProperty maxOffset = prefabRule.FindPropertyRelative("MaxOffset");

            SerializedProperty minRepeatDistance = prefabRule.FindPropertyRelative("MinRepeatDistance");
            SerializedProperty maxRepeatDistance = prefabRule.FindPropertyRelative("MaxRepeatDistance");
            SerializedProperty minGroupSize = prefabRule.FindPropertyRelative("MinGroupSize");
            SerializedProperty maxGroupSize = prefabRule.FindPropertyRelative("MaxGroupSize");
            SerializedProperty minGroupSpacing = prefabRule.FindPropertyRelative("MinGroupSpacing");
            SerializedProperty maxGroupSpacing = prefabRule.FindPropertyRelative("MaxGroupSpacing");


            SerializedProperty useMinDistance = prefabRule.FindPropertyRelative("UseMinDistance");
            SerializedProperty minDistance = prefabRule.FindPropertyRelative("MinDistance");
            SerializedProperty useMaxDistance = prefabRule.FindPropertyRelative("UseMaxDistance");
            SerializedProperty maxDistance = prefabRule.FindPropertyRelative("MaxDistance");


            //Determine if the rule is expanded or collapsed
            if (_prefabRulesExpanded.Count <= i)
            {
                _prefabRulesExpanded.Add(true);
            }

            _prefabRulesExpanded[i] = EditorGUILayout.Foldout(_prefabRulesExpanded[i], "Prefab Rule " + (i + 1).ToString());


            if (_prefabRulesExpanded[i])
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();


                //Delete this element if the remove button is clicked
                bool ruleDeleted = false;
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(_removePrefabButton, GUILayout.Width(50)))
                {
                    _prefabRules.DeleteArrayElementAtIndex(i);
                    ruleDeleted = true;
                }

                EditorGUILayout.EndHorizontal();

                //Don't layout anything else if we deleted the element
                if (!ruleDeleted)
                {
                    //Now allow the users to edit the rule
                    EditorGUILayout.PropertyField(prefabToClone, new GUIContent("Prefab To Clone", "The prefab you want to clone."));

                    EditorGUILayout.PropertyField(minOffset, new GUIContent("Min Offset", "Minimum Distance between prefab placement and road."));
                    EditorGUILayout.PropertyField(maxOffset, new GUIContent("Max Offset", "Maximum Distance between prefab placement and road."));


                    EditorGUILayout.PropertyField(minRepeatDistance, new GUIContent("Min Repeat Distance", "Minimum distance between prefab placement."));
                    EditorGUILayout.PropertyField(maxRepeatDistance, new GUIContent("Max Repeat Distance", "Maximum distance between prefab placement."));
                    EditorGUILayout.PropertyField(minGroupSize, new GUIContent("Min Group Size", "Minimum group size for prefabs - used if you want more than one prefab generated at a time."));
                    EditorGUILayout.PropertyField(maxGroupSize, new GUIContent("Max Group Size", "Maximum group size for prefabs - used if you want more than one prefab generated at a time."));
                    EditorGUILayout.PropertyField(minGroupSpacing, new GUIContent("Min Group Spacing", "The minimum spacing between the prefabs in your group."));
                    EditorGUILayout.PropertyField(maxGroupSpacing, new GUIContent("Max Group Spacing", "The maximum spacing between the prefabs in your group."));


                    //Set min and max distances
                    useMinDistance.boolValue = EditorGUILayout.Toggle("Use Min Distance", useMinDistance.boolValue);
                    if (useMinDistance.boolValue)
                    {
                        EditorGUILayout.PropertyField(minDistance, new GUIContent("Min Distance"));
                    }

                    useMaxDistance.boolValue = EditorGUILayout.Toggle("Use Max Distance", useMaxDistance.boolValue);
                    if (useMaxDistance.boolValue)
                    {
                        EditorGUILayout.PropertyField(maxDistance, new GUIContent("Max Distance"));
                    }

                    if (minDistance.floatValue < 0f) 
                        minDistance.floatValue = 0f; 

                    if (maxDistance.floatValue < 0f) 
                        maxDistance.floatValue = 0f; 

                    if (minRepeatDistance.floatValue > maxRepeatDistance.floatValue) 
                        maxRepeatDistance.floatValue = minRepeatDistance.floatValue; 

                    if (minGroupSize.intValue > maxGroupSize.intValue)  
                        maxGroupSize.intValue = minGroupSize.intValue; 
                        
                    if (minGroupSpacing.floatValue > maxGroupSpacing.floatValue)  
                        maxGroupSpacing.floatValue = minGroupSpacing.floatValue; 
                        
                    if (minGroupSpacing.floatValue < 1f)
                        minGroupSpacing.floatValue = 1f;
                        
                        
                    if (maxGroupSpacing.floatValue < 1f)
                        maxGroupSpacing.floatValue = 1f;
                        
                        
                    if (minRepeatDistance.floatValue < 20f)
                        minRepeatDistance.floatValue = 20f;

                    if (minOffset.floatValue > maxOffset.floatValue)
                        maxOffset.floatValue = minOffset.floatValue;


                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        



        //Now update all the modified properties
        _spriteShapeDisplayer.ApplyModifiedProperties();

        if (GUI.changed)
        {
            SpriteShapeDisplayer sp = target as SpriteShapeDisplayer;
            //sp.Setup();
            //PrefabManager prefabManager = new PrefabManager();
            //prefabManager.GeneratePrefabsInTheBeginning();
        }
    }

}
#endif