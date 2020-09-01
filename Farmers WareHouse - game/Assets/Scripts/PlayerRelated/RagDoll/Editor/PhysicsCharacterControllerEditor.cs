using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;

// Custom editor for the physics character controller
[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharacterControllerEditor : Editor
{
    // serializedProperties
    SerializedProperty partList, baseMass;
    SerializedProperty playerGravity, collisionMask;

    //ON enable
    private void OnEnable()
    {
        // set the serialized properties
        partList = serializedObject.FindProperty("bodyParts");              // Exposed body parts
        baseMass = serializedObject.FindProperty("BaseMassScale");          // Base character mass
        playerGravity = serializedObject.FindProperty("CharacterGravity");  // Gravity value
        collisionMask = serializedObject.FindProperty("collisionMask");     // collision Mask
    }

    // on inspector GUIO
    public override void OnInspectorGUI()
    {
        // update the serialized object
        serializedObject.Update();

        // draw UI
        DrawUI();

        // Apply changes to serialized obj
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draw the custum interface
    /// </summary>
    private void DrawUI()
    {
        // Draw the name of the Script
        EditorGUILayout.LabelField("Physics Base Controller!", EditorStyles.boldLabel);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), Color.black); // Draw line based on with
        EditorGUILayout.Space(5f);  // spaec


        // Base StandUp mass Scale
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Base StandUp Joint Mass Scale", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(baseMass);
        EditorGUILayout.Space(10f);

        // Colision mask
        EditorGUILayout.LabelField("Collision Mask", EditorStyles.boldLabel);
        // display the dropDown, no lable
        EditorGUILayout.PropertyField(collisionMask,new GUIContent());  
        EditorGUILayout.Space(10f);

        // Gravity value
        EditorGUILayout.LabelField("Character gravity value", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playerGravity, new GUIContent());
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10f);

        // Info label
        EditorGUILayout.LabelField("Reference all the body parts", EditorStyles.boldLabel);

        // draw the list of parts
        EditorGUILayout.BeginVertical("helpBox");
        EditorGUILayout.PropertyField(partList);
        EditorGUILayout.EndVertical();
    }
}
