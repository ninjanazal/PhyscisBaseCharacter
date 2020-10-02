using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Contrador para o painel de informações no ecra
/// </summary>
public class InformationPanel : MonoBehaviour
{
    // Referencia para o valor onde é mostrado o valor de fps
    public TMP_Text fpsvalue, debug_panel;

    // debug information
    private static TMP_Text debug_field;

    // linhas de texto na consola
    private static List<string> debug_lines;
    private static int max_lines = 10;

    //  Valores internos
    // valor aproximado para o contador de fps
    private float fps_timer = 0f;

    // monobehavior start
    private void Awake()
    {
        // inicia a consola de debug
        InitConsole(debug_panel);
    }

    private void Update()
    {
        // executa o metodo que determina uma aproximação de fps
        CalculateFps();
    }

    #region Private methods
    /// <summary>
    /// Determina um valor apriximado do numero de imagens estao a ser geradas por segundo
    /// </summary>
    private void CalculateFps()
    {
        // avalia se passou 1s para que seja gerado um novo valor para os fps
        if (fps_timer > 1.0f)
        {
            // reenicia o contador
            fps_timer = 0f;
            // valor estipulado para o tempo que demorou a produzir a ultima imagem
            var value = ((int)(1f / Time.unscaledDeltaTime));

            // atribui cor ao contador de acordo com o valor de fps
            fpsvalue.color = value >= 60 ? Color.green :
                value >= 40 ? Color.yellow : Color.red;
            // atribui o valor ao texto
            fpsvalue.text = value.ToString();
        }
        // caso nao tenha ocorrido o tempo necessário
        else
        {
            // incrementa o tempo passado pela ultima iteraçao
            fps_timer += Time.deltaTime;
        }
    }
    #endregion

    #region Debug output
    /// <summary>
    /// Inicia a consola de debug on screen
    /// </summary>
    private void InitConsole(TMP_Text panel)
    {
        // inicia a queue de linhjas
        debug_lines = new List<string>();

        // limpa todo o texto presente na consola, caso exista referencia
        if (!panel) { Debug.LogError("debug output not setted"); return; }
        // atribui ao painel definido
        debug_field = panel;
        debug_field.text = String.Empty;

        // aprenseta que o debug foi iniciado
        InformationPanel.DebugConsoleInput("Started debug info");
    }

    /// <summary>
    /// Insere informaçao na consola de debug
    /// </summary>
    public static void DebugConsoleInput(string input)
    {
        // avalia se existe uma referencia para o campo de input
        if (!debug_field)
            return;

        // caso exista uma referencia para o input
        // adiciona o input para a lista de linhas
        debug_lines.Add($"-> {input}\n");
        // verifica se foi ultrapassado o limite de linhas atribuido, remove o ultimo
        if (debug_lines.Count > max_lines) debug_lines.RemoveAt(0);

        // cria a string para imprimir na consola
        debug_field.text = "";
        // atribui ao campo a informaçao da lista de debug
        debug_lines.ForEach(value => { debug_field.text += value; });
    }

    #endregion

}
