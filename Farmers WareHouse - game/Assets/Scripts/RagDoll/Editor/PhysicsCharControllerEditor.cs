using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharControllerEditor : Editor
{
    // Vista de editor personalizada para o controlo fisico do personagem
    // referencia para o target (Script original)
    private PhysicsCharacterController Currenttarget;

    public override void OnInspectorGUI()
    {
        // guarda a referencia do target script
        Currenttarget = (PhysicsCharacterController)target;

        // desenha o editor padrao
        base.OnInspectorGUI();

        // Info
        EditorGUILayout.LabelField("Physics Controller", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Physics Character controller, fill the fields needed", MessageType.Info);

        // Referencia para o objecto de raiz do controlador

    }
}
