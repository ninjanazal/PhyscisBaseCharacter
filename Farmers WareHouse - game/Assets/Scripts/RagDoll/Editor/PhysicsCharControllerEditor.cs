using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharControllerEditor : Editor
{
    // Vista de editor personalizada para o controlo fisico do personagem
    // referencia para o target (Script original)
    private PhysicsCharacterController Currenttarget;
    // referencia para o objecto de raiz do esqueleto
    private SerializedProperty skeletonBase;
    // referencia para oarray de ossos uteis
    private SerializedProperty usefulBones;

    public override void OnInspectorGUI()
    {
        // Referencia para o objecto de raiz do controlador
        GUIReferences();
    }

    #region Private Methods
    // comporta todos a atualizaçao das referencia para objectos
    private void GUIReferences()
    {
        // guarda a referencia do target script
        var serializedObject = new SerializedObject(target);


        // link entre as propriedades do custom editor e o componente
        skeletonBase = serializedObject.FindProperty("SkeletonBase");   // base do objecto
        usefulBones = serializedObject.FindProperty("Bones");           // array de ossos

        // actualiza todos os objectos referenciados
        serializedObject.Update();
        // chama comportamento de layout
        Layout();
        // aplica as modificaçoes feitas no editor ao componente
        serializedObject.ApplyModifiedProperties();
    }

    // retem todo o layout para o editor
    private void Layout()
    {
        #region Information Region
        // Info
        //GUI.backgroundColor = Color.grey; // define a cor de fundo para o bloco
        // grupo de informação
        EditorGUILayout.BeginVertical(GUI.skin.label);
        EditorGUILayout.LabelField("Physics Controller", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Physics Character controller, fill the fields needed", MessageType.Info);
        //Campo para referencias
        // Base do esqueleto
        EditorGUILayout.PropertyField(skeletonBase);
        EditorGUILayout.EndVertical();
        #endregion

        EditorGUILayout.Space();

        #region Useful Bones
        EditorGUILayout.BeginVertical("Box");
        // Information
        EditorGUILayout.LabelField("Bone Structure", EditorStyles.boldLabel);
        // avalia se já estao definidos os ossos no componente
        if (usefulBones != null)
            // array de objectos
            EditorGUILayout.ObjectField(usefulBones);
        else
            // caso seja null, informa que deve ser determinado os ossos uteis
            EditorGUILayout.HelpBox("Useful Bones not defined!", MessageType.Error);

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        #endregion

    }
    #endregion
}
