using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsCharacterController))]
public class PhysicsCharControllerEditor : Editor
{
    // Vista de editor personalizada para o controlo fisico do personagem
    // referencia para o objecto de raiz do esqueleto
    // referencia para oarray de ossos uteis
    private SerializedProperty skeletonBase;
    private PhysicsCharacterController physicsCtrl_target;

    public override void OnInspectorGUI()
    {
        // Referencia para o objecto de raiz do controlador
        GUIReferences();
    }

    #region Private Methods
    // comporta todos a atualizaçao das referencia para objectos
    private void GUIReferences()
    {
        // guarda a referencia para o script
        physicsCtrl_target = (PhysicsCharacterController)serializedObject.targetObject;

        // link entre as propriedades do custom editor e o componente
        skeletonBase = serializedObject.FindProperty("SkeletonBase");   // base do objecto

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
        if (physicsCtrl_target.GetBoneCount == 0)
        {
            // caso nao existam elementos
            EditorGUILayout.HelpBox("Useful Bones not defined!", MessageType.Error);
            // Botao que calcula determina os ossos
            if (GUILayout.Button("Validate Bones")) physicsCtrl_target.CalculateBones();
        }
        else
        {
            // precorre todos os elementos presentes
            for (int i = 0; i < physicsCtrl_target.GetBoneCount; i++)
            {
                // extrai o objecto do elemento a ser trabalhado, e transforma num objecto do tipo "Bone"
                Bone value = physicsCtrl_target.GetBoneByIndex(i);
                // Mostra a informação do osso 
                EditorGUILayout.LabelField(value.Name, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Joint Rotation", value.BoneTransform.localRotation.eulerAngles.ToString());

            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        #endregion

    }
    #endregion
}
