using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class referente ao elemento do osso, representa todas as informaçoes que um osso pode ter
public class Bone
{
    // nome do osso
    private string bone_name;

    // variaveis privadas
    // objecto referenciado como osso activo
    private GameObject bone_GO;

    // componentes presentes no osso
    private CharacterJoint bone_joint;
    private Rigidbody bone_rb;

    /// <summary>
    /// COnstrutor da classe de oso
    /// </summary>
    /// <param name="go">GameObject referenciado como um osso activo</param>
    public Bone(GameObject go)
    {
        // guarda referencia para os elementos passados
        bone_GO = go;
        // guarda o nome do objecto como nome do osso
        bone_name = go.name;
        // referencia para o componente de joint atribuido
        bone_joint = bone_GO.GetComponent<CharacterJoint>();
        // referencia para o componente de rigidbody
        bone_rb = bone_GO.GetComponent<Rigidbody>();
    }



    #region Getters
    /// <summary>
    /// Retorna o nome do osso
    /// </summary>
    public string Name { get { return bone_name; } }

    #endregion
}
