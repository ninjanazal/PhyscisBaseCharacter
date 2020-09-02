using UnityEngine;
using UnityEditor;

// Custom editor for the physics character controller
[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharacterControllerEditor : Editor
{
    // serializedProperties
    SerializedProperty partList, baseMass;
    SerializedProperty playerGravity, collisionMask;
    SerializedProperty groundCheckThreshold;

    //ON enable
    private void OnEnable()
    {
        // set the serialized properties
        partList = serializedObject.FindProperty("bodyParts");              // Exposed body parts
        baseMass = serializedObject.FindProperty("BaseMassScale");          // Base character mass
        playerGravity = serializedObject.FindProperty("CharacterGravity");  // Gravity value
        collisionMask = serializedObject.FindProperty("collisionMask");     // collision Mask
        groundCheckThreshold = serializedObject.FindProperty("groundTestsize"); // value for the ground detection threshold
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
        EditorGUILayout.Space(20f);  // spaec


        // Base StandUp mass Scale
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Base StandUp Joint Mass Scale", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(baseMass);
        EditorGUILayout.Space(5f);

        // Ground check value
        EditorGUILayout.LabelField("Ground Check Threshold", EditorStyles.boldLabel);
        EditorGUILayout.Slider(groundCheckThreshold, .1f, .3f, new GUIContent());
        EditorGUILayout.Space(5f);


        // Gravity value
        EditorGUILayout.LabelField("Character gravity value", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playerGravity, new GUIContent());
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10f);

        // Colision mask
        EditorGUILayout.LabelField("Collision Mask", EditorStyles.boldLabel);
        // display the dropDown, no lable
        EditorGUILayout.PropertyField(collisionMask, new GUIContent());
        EditorGUILayout.Space(10f);

        // Info label
        EditorGUILayout.LabelField("Reference all the body parts", EditorStyles.boldLabel);

        // draw the list of parts
        EditorGUILayout.BeginVertical("helpBox");
        EditorGUILayout.PropertyField(partList);
        EditorGUILayout.EndVertical();
    }
}
