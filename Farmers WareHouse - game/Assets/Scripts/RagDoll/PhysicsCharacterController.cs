using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PhysicsCharacterController : MonoBehaviour
{
    // Variaveis publicas
    // Base do modelo
    public GameObject SkeletonBase;

    // Lista de todas as joints definidas no modelo
    private Bone[] bones;

    // Start is called before the first frame update
    void Start()
    {
        // inicia o comportamento do componente
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region Private Methods
    /// <summary>
    /// Inicia o componente fisico do controlador do personagem,
    /// procura por ossos elementais e retem a sua referencia
    /// </summary>
    private void Init()
    {
        // precorre todos os filhos do objecto referenciado como base
        var childs = SkeletonBase.GetComponentsInChildren<Rigidbody>();
        // inicia o array de bones
        bones = new Bone[childs.Length];

        // debug para o controlador
        InformationPanel.DebugConsoleInput($"Found {childs.Length} Useful bones");

        // para cada um dos rigidbodys encontrados, cria um bone util e atribui á lista interna
        for (int i = 0; i < childs.Length; i++)
        {
            // avalia o rigidbody e extrai a informaçao para o bone
            bones[i] = new Bone(childs[i].gameObject);
            // adiciona ao controlador de debug
            InformationPanel.DebugConsoleInput(bones[i].Name);
        }
    }
    #endregion
}
