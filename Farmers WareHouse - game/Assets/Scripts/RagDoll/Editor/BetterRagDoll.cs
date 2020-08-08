using UnityEngine;
using UnityEditor;
using System;

public class BetterRagDoll : EditorWindow
{
    // elementos necessários para a criaçao de uma ragdoll dinamica
    public GameObject Head, MidleSpine, Hips, LArm, LElbow, RArm, RElbom, LLeg, LKnee, RLeg, RKnee;

    /// <summary>
    /// Custom Tool for creating ragdolls with some configurable joints
    /// </summary>
    [MenuItem("Tools/BrokenLamp/BetterRagDoll")]
    // Inicia a janela
    public static void Init()
    {
        // Cria uma janela ao inicial e guarda da mesma
        var thisWindow = EditorWindow.CreateInstance<BetterRagDoll>();

        thisWindow.titleContent.text = "Better RagDolls";   // define o nome a mostrar no titulo
        thisWindow.minSize = new Vector2(380f, 400f);        // define o tamanho minimo da janela        
        thisWindow.Show();                                  // mostra a janela definida

        // debug do nome
        Debug.Log(thisWindow.titleContent.text + "  " + thisWindow.position.width + "  " + thisWindow.position.height);
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
        // verifica se todos os elementos estao preenchidos
        CheckNull();

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
