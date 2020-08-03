using UnityEngine;
using TMPro;


/// <summary>
/// Contrador para o painel de informações no ecra
/// </summary>
public class InformationPanel : MonoBehaviour
{
    // Referencia para o valor onde é mostrado o valor de fps
    public TMP_Text fpsvalue;

    //  Valores internos
    // valor aproximado para o contador de fps
    private float fps_timer = 0f;

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
}
