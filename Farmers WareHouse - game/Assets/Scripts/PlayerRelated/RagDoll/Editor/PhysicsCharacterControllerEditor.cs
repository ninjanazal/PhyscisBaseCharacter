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

    //ON enable
    private void OnEnable()
    {
        // set the serialized properties
        partList = serializedObject.FindProperty("bodyParts");
        baseMass = serializedObject.FindProperty("BaseMassScale");
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
        // Base StandUp mass Scale
        EditorGUILayout.LabelField("Base StandUp Joint Mass Scale");
        EditorGUILayout.PropertyField(baseMass);        

        // Info label
        EditorGUILayout.LabelField("Reference all the body parts", EditorStyles.boldLabel);
        // draw the list of parts
        EditorGUILayout.PropertyField(partList);
    }
}
