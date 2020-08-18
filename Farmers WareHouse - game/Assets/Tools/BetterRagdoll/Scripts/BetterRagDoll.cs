using UnityEngine;
using UnityEditor;
using System;

public class BetterRagDoll : EditorWindow
{
    // elementos necessários para a criaçao de uma ragdoll dinamica
    private GameObject Head, MidleSpine, Hips, LArm, LElbow, RArm, RElbom, LLeg, LKnee, RLeg, RKnee;
    // referenc to the Go that hosts the physics system
    private GameObject TargetGO;

    /// <summary>
    /// Custom Tool for creating ragdolls with some configurable joints
    /// </summary>
    [MenuItem("Tools/BrokenLamp/BetterRagDoll")]
    // Inicia a janela
    public static void Init()
    {
        // serializable properties

        // Cria uma janela ao inicial e guarda da mesma
        var thisWindow = EditorWindow.CreateInstance<BetterRagDoll>();

        #region Init SerializeProp
        #endregion

        thisWindow.titleContent.text = "Better RagDolls";   // define o nome a mostrar no titulo
        thisWindow.minSize = new Vector2(380f, 400f);        // define o tamanho minimo da janela        
        thisWindow.Show();                                  // mostra a janela definida
    }

    // ao desenha
    void OnGUI()
    {
        // inicia o wrapper vertical inicial
        EditorGUILayout.BeginVertical("BOX");

        // Labels informativas
        EditorGUILayout.LabelField("Better RagDolls by BronkeLamp", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Select all the parts needed to create a base structure for the doll");

        // posicionamento dos elementos
        #region Elements Layout
        //estrutura de elementos para input
        DrawInputFields();
        #endregion

        // termina o wrapper vertical
        EditorGUILayout.EndVertical();
    }

    // desenha todo o corpo de imput
    private void DrawInputFields()
    {
        // Input target for referenc the hoster
        EditorGUILayout.BeginHorizontal();
        // fild to input the target object for  host the physcs controller
        EditorGUILayout.LabelField("Host for physics System");
        // strores the selected target
        TargetGO = (GameObject)EditorGUILayout.ObjectField(TargetGO, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

    }

    #region Private Methods
    // verifica se todos os elementos do array nao estao nulos
    private bool CheckNull()
    {
        // avalia para cada elemento se é nulo
        return Head && MidleSpine && Hips && LArm && LElbow && RArm && RElbom && LLeg && LKnee && RLeg && RKnee;
    }

    #endregion
}
