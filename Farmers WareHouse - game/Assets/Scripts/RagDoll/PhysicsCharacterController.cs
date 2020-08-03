using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCharacterController : MonoBehaviour
{
    // Variaveis publicas
    // Base do modelo
    public GameObject SkeletonBase;

    // Lista de todas as joints definidas no modelo
    private Bone[] Bones;

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

    }

    #endregion
}
