using UnityEngine;
using UnityEditor;


public class BetterRagDoll : EditorWindow
{
    // campos publicos necessários
    public GameObject Head, MidleSpine, Hips, LArm, LElbow, RArm, RElbom, LLeg, LKnee, RLeg, RKnee;

    /// <summary>
    /// Custom Tool for creating ragdolls with some configurable joints
    /// </summary>
    [MenuItem("Tools/BrokenLamp/BetterRagDoll")]
    // Inicia a janela
    public static void Init()
    {
        // Cria uma janela ao inicial e guarda da mesma
        //var thisWindow = EditorWindow.GetWindow(typeof(BetterRagDoll), true, "Better RagDolls");
        var thisWindow = EditorWindow.CreateInstance<BetterRagDoll>();

        // define o nome a mostrar no titulo
        thisWindow.titleContent.text = "Better RagDolls";
        // mostra a janela definida
        thisWindow.Show();

        // debug do nome
        Debug.Log(thisWindow.titleContent.text + "  " + thisWindow.position.width + "  " + thisWindow.position.height);
    }

    // ao desenha
    void OnGUI()
    {
        // inicia o wrapper vertical inicial
        EditorGUILayout.BeginVertical("BOX");

        // mostra uma label
        EditorGUILayout.LabelField("Better RagDolls by BronkeLamp", EditorStyles.boldLabel);
        // descriçao 
        EditorGUILayout.LabelField("Select all the parts needed to create a base structure for the doll");
        // informa o tamanho actual da janela
        if (GUILayout.Button("Get Current Size"))
        { Debug.Log(EditorGUIUtility.currentViewWidth+ "  " + position.height); }

        // posicionamento dos elementos
        #region Elements Layout


        #endregion
        // termina o wrapper vertical
        EditorGUILayout.EndVertical();
    }
}
