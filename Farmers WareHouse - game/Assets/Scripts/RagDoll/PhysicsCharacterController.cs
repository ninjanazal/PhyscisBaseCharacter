using System;
using UnityEngine;

public class PhysicsCharacterController : MonoBehaviour
{
    // Variaveis publicas
    // Base do modelo
    public GameObject SkeletonBase;
    // Lista de todas as joints definidas no modelo
    [SerializeField] private Bone[] _bones;

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
        // array de elementos que contenham rigidbodyes e character joints
        var childs = Array.FindAll(SkeletonBase.GetComponentsInChildren<Rigidbody>(),
            item => item.GetComponent<CharacterJoint>());
        // inicia o array de bones
        _bones = new Bone[childs.Length];

        // debug para o controlador
        InformationPanel.DebugConsoleInput($"Found {childs.Length} Useful bones");

        // para cada um dos rigidbodys encontrados, cria um bone util e atribui á lista interna
        for (int i = 0; i < childs.Length; i++)
        {
            // avalia o rigidbody e caso seja um osso valido extrai a informaçao para o bone
            _bones[i] = new Bone(childs[i].gameObject);
            
            // adiciona ao controlador de debug
            InformationPanel.DebugConsoleInput(_bones[i].Name);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Retorna o numero de ossos detectados
    /// </summary>
    public int GetBoneCount { get { if (_bones != null) return _bones.Length; else return 0; } }
    /// <summary>
    /// Retorna o elemento do array de acordo com o indice passado
    /// </summary>
    /// <param name="index">Posiçao do osso no array</param>
    /// <returns></returns>
    public Bone GetBoneByIndex(int index) { if (GetBoneCount != 0) return _bones[index]; else return null; }
    /// <summary>
    /// Inicia o array de ossos
    /// </summary>
    public void CalculateBones() => Init();
    #endregion
}
