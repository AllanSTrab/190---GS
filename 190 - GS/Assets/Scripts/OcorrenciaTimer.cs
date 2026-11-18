using TMPro;
using UnityEngine;

public class OcorrenciaTimer : MonoBehaviour
{
    [SerializeField] private float tempoRestante = 20f;
    [SerializeField] private TextMeshPro textoTempo;
    public GameManager gameManager;
    private bool acabou = false;

    private void Update()
    {
        if (acabou)
            return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            acabou = true;

            textoTempo.text = "0";

            PerderJogo();
            return;
        }

        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        if (tempoRestante >= 10f)
        {
            textoTempo.text =
                Mathf.CeilToInt(tempoRestante).ToString();
        }
        else
        {
            textoTempo.text =
                tempoRestante.ToString("F2");
        }

        if (tempoRestante > 10f)
        {
            textoTempo.color = Color.green;
        }
        else if (tempoRestante > 5f)
        {
            textoTempo.color = Color.yellow;
        }
        else
        {
            textoTempo.color = Color.red;
        }
    }

    void PerderJogo()
    {
        gameManager.GameOver();
    }
}
