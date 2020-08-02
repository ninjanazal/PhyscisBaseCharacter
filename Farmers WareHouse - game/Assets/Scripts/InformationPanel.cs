using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using System;

/// <summary>
/// Contrador para o painel de informações no ecra
/// </summary>
public class InformationPanel : MonoBehaviour
{
    // Referencia para o valor onde é mostrado o valor de fps
    public TMP_Text fpsvalue;

    //  Valores internos
    // valor aproximado para o contador de fps
    private float current_fps = 0f;

    // task para o determinar o valor de fps
    private Task fpsCalculation_task;

    void Start()
    {   // inicia o metodo async com um token para cancelamento
        fpsCalculation_task = FpsCalculationAsync();
    }

    private void Update()
    {
        // verifica se o valor escrito no texto é diferente do valor determinado ao momento
        if (fpsvalue.text != ((int)(current_fps)).ToString())
        {
            Debug.Log("New Value");
            // caso sejam diferentes
            // atribui o mesmo valor
            fpsvalue.text = ((int)(current_fps)).ToString();
            // atribui a cor do texto de acordo com o valor de fps
            fpsvalue.color = current_fps >= 60.0f ? Color.green : Color.red;
        }
    }

    #region Async methods
    private async Task FpsCalculationAsync()
    {
        // esta metodo deve correr em backgraound enquanto activo
        while (true)
        {
            // determina aproximadamente a quantos fps está a correr o programa
            current_fps = 60f / Time.deltaTime;
            // aguarda 1s para rever novamente
            await Task.Delay(1000);
        }
    }


    #endregion
}
