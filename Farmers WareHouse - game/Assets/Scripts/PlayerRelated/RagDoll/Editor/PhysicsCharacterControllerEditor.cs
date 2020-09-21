using UnityEngine;
using UnityEditor;

// Custom editor for the physics character controller
[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharacterControllerEditor : Editor
{
    // serializedProperties
    SerializedProperty partList, baseMass, characterMass;
    SerializedProperty playerGravity, collisionMask;
    SerializedProperty groundCheckThreshold;
    SerializedProperty driftPrevention;
    SerializedProperty stepSearchDistance, stepMaxSize;

    //ON enable
    private void OnEnable()
    {
        // set the serialized properties
        partList = serializedObject.FindProperty("bodyParts");              // Exposed body parts
        characterMass = serializedObject.FindProperty("CharacterMass"); // base mass influence
        baseMass = serializedObject.FindProperty("BaseMassScale");          // Base character mass
        playerGravity = serializedObject.FindProperty("CharacterGravity");  // Gravity value
        collisionMask = serializedObject.FindProperty("collisionMask");     // collision Mask
        groundCheckThreshold = serializedObject.FindProperty("groundTestsize"); // value for the ground detection threshold
        driftPrevention = serializedObject.FindProperty("driftPrevention"); // value for the drift prevention
        stepSearchDistance = serializedObject.FindProperty("stepSearchDistance"); // distance to check for step
        stepMaxSize = serializedObject.FindProperty("maxStepSize");     // step max size

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


        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // Base StandUp mass Scale
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Base StandUp Joint Mass Scale", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(baseMass, new GUIContent());
        EditorGUILayout.Space(5f);
        EditorGUILayout.EndVertical();

        // Base mass influence
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Character Mass", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(characterMass, new GUIContent());
        EditorGUILayout.Space(5f);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        // Ground check value
        EditorGUILayout.LabelField("Ground Check Threshold", EditorStyles.boldLabel);
        EditorGUILayout.Slider(groundCheckThreshold, .1f, .3f, new GUIContent());
        EditorGUILayout.Space(5f);

        // step detection
        // set search distance
        EditorGUILayout.LabelField("Set Search distance", EditorStyles.boldLabel);
        EditorGUILayout.Slider(stepSearchDistance, 0.1f, 1f, new GUIContent());
        //max step size
        EditorGUILayout.LabelField("Max Step Size", EditorStyles.boldLabel);
        EditorGUILayout.Slider(stepMaxSize, .1f, 3.5f, new GUIContent());
        EditorGUILayout.Space(5f);

        // Drift prevention
        EditorGUILayout.LabelField("Drift prevention value", EditorStyles.boldLabel);
        EditorGUILayout.Slider(driftPrevention, 0f, 1f, new GUIContent());
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
