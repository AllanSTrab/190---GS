using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public bool ocupada;
    public TipoOcorrencia tipoAtual;

    public GameObject sinalizacaoAtual;

}

public enum TipoOcorrencia
{
    Incendio,
    Crime,
    Medica
}