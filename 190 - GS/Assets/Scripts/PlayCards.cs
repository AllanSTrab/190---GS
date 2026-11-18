using UnityEngine;
using UnityEngine.EventSystems;

public class PlayCards : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    [SerializeField] private GameObject ghostPrefab;
    private GameObject ghostCard;

    private bool emMovimento = false;
    [SerializeField] private float velocidadeMovimento;
    private float tempoFantasma;

    private Vector3 escalaOriginal;
    private Vector3 escalaFantasmaOriginal;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        escalaOriginal = transform.localScale;
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

        if (emMovimento)
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
    }
}
