using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

// Custom editor para a class de comportamento fisico
[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharControllerEditor : Editor
{
    // override para o default inspector
    public override void OnInspectorGUI()
    {

    }

}
