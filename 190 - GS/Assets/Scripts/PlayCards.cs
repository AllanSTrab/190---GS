using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayCards : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private OcorrenciaManager ocorrenciaManager;

    private RectTransform rectTransform;
    private Canvas canvas;

    [SerializeField] private GameObject ghostPrefab;
    private GameObject ghostCard;

    private bool emMovimento = false;
    [SerializeField] private float velocidadeMovimento;
    private float tempoFantasma;

    private Vector3 escalaOriginal;
    private Vector3 escalaFantasmaOriginal;

    [SerializeField] private TipoEquipe tipoEquipe;

    private bool resolvendo = false;

    private HouseScript casaAtual;

    [SerializeField] private TextMeshProUGUI timerText;

    private OcorrenciaManager OcorrenciaManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        escalaOriginal = transform.localScale;

        timerText.gameObject.SetActive(false);
    }

    private void Start()
    {
        ocorrenciaManager = FindObjectOfType<OcorrenciaManager>();
    }

    private void Update()
    {
        if (ghostCard == null)
            return;

        tempoFantasma += Time.deltaTime;

        RectTransform ghostRect =
            ghostCard.GetComponent<RectTransform>();

        float rotacao =
            Mathf.Sin(tempoFantasma * 6f) * 5f;

        ghostRect.rotation =
            Quaternion.Euler(0, 0, rotacao);

        float escala =
            1f + Mathf.Sin(tempoFantasma * 5f) * 0.08f;

        ghostRect.localScale =
            escalaFantasmaOriginal * escala;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempoFantasma = 0f;

        if (emMovimento || resolvendo)
            return;

        transform.SetAsLastSibling();

        transform.localScale = escalaOriginal * 1.1f;

        ghostCard = Instantiate(gameObject, transform.parent);
        escalaFantasmaOriginal = ghostCard.transform.localScale;

        Destroy(ghostCard.GetComponent<PlayCards>());

        CanvasGroup cg = ghostCard.AddComponent<CanvasGroup>();
        cg.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostCard == null)
            return;

        RectTransform ghostRect =
            ghostCard.GetComponent<RectTransform>();

        ghostRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostCard == null)
            return;

        Vector3 destino = ghostCard.transform.position;

        ghostCard.transform.rotation = Quaternion.identity;
        ghostCard.transform.localScale = Vector3.one;

        Destroy(ghostCard);

        StartCoroutine(MoverCarta(destino));

        transform.localScale = escalaOriginal;
    }

    private System.Collections.IEnumerator MoverCarta(Vector3 destino)
    {
        emMovimento = true;

        while (Vector3.Distance(transform.position, destino) > 1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                velocidadeMovimento * Time.deltaTime
            );

            yield return null;
        }

        transform.position = destino;

        emMovimento = false;

        VerificarCasa();
    }

    private void VerificarCasa()
    {
        HouseScript[] todasCasas =
            FindObjectsOfType<HouseScript>();

        foreach (HouseScript casa in todasCasas)
        {
            float distancia =
                Vector2.Distance(
                    transform.position,
                    casa.transform.position
                );

            if (distancia < 1f)
            {
                bool correto = false;

                switch (casa.tipoAtual)
                {
                    case TipoOcorrencia.Incendio:

                        correto =
                            tipoEquipe ==
                            TipoEquipe.Bombeiro;
                        break;

                    case TipoOcorrencia.Crime:

                        correto =
                            tipoEquipe ==
                            TipoEquipe.Policia;
                        break;

                    case TipoOcorrencia.Medica:

                        correto =
                            tipoEquipe ==
                            TipoEquipe.Paramedico;
                        break;
                }

                if (correto && casa.ocupada)
                {
                    StartCoroutine(ResolverOcorrencia(casa));
                }

                break;
            }
        }
    }

    private System.Collections.IEnumerator ResolverOcorrencia(HouseScript casa)
    {
        resolvendo = true;
        emMovimento = true;

        timerText.gameObject.SetActive(true);

        float tempo = 3f;

        while (tempo > 0f)
        {
            timerText.text = tempo.ToString("F2");

            float porcentagem = tempo / 3f;

            timerText.color = Color.Lerp(Color.blue,Color.red,porcentagem);
            tempo -= Time.deltaTime;

            yield return null;
        }

        timerText.text = "0.00";

        Destroy(casa.sinalizacaoAtual);

        TipoOcorrencia tipoResolvido = casa.tipoAtual;

        casa.ocupada = false;
        casa.sinalizacaoAtual = null;

        ocorrenciaManager.RemoverOcorrencia(tipoResolvido);

        GameManager.Instance.RegistrarOcorrenciaResolvida();

        timerText.gameObject.SetActive(false);

        resolvendo = false;
        emMovimento = false;
    }


}

public enum TipoEquipe
{
    Bombeiro,
    Policia,
    Paramedico
}