using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int ocorrenciasResolvidas;

    [SerializeField] private TextMeshProUGUI textoProgresso;

    private void Awake()
    {
        Instance = this;
    }

    public void RegistrarOcorrenciaResolvida()
    {
        ocorrenciasResolvidas++;

        textoProgresso.text = ocorrenciasResolvidas + "/10";

        if (ocorrenciasResolvidas >= 10)
        {
            VencerJogo();
        }
    }

    void VencerJogo()
    {
        Debug.Log("VOCÊ GANHOU!");

        StopAllCoroutines();

        Time.timeScale = 0f;

        // Futuramente:
        // telaVitoria.SetActive(true);
    }

    public void GameOver()
    {
        Debug.Log("VOCÊ PERDEU");

        StopAllCoroutines();

        Time.timeScale = 0f;
    }

}