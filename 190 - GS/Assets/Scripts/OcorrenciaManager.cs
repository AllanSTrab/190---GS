using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcorrenciaManager : MonoBehaviour
{
    public HouseScript[] casas;

    public GameObject incendioPrefab;
    public GameObject crimePrefab;
    public GameObject medicaPrefab;

    private int incendiosAtivos;
    private int crimesAtivos;
    private int medicasAtivas;

    private void Start()
    {
        StartCoroutine(GerarOcorrencias());
    }

    IEnumerator GerarOcorrencias()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(3f, 8f)
            );

            CriarOcorrencia();
        }
    }

    void CriarOcorrencia()
    {
        List<TipoOcorrencia> tiposDisponiveis =
            new List<TipoOcorrencia>();

        if (incendiosAtivos < 2)
            tiposDisponiveis.Add(TipoOcorrencia.Incendio);

        if (crimesAtivos < 2)
            tiposDisponiveis.Add(TipoOcorrencia.Crime);

        if (medicasAtivas < 2)
            tiposDisponiveis.Add(TipoOcorrencia.Medica);

        if (tiposDisponiveis.Count == 0)
            return;

        TipoOcorrencia tipo =
            tiposDisponiveis[
                Random.Range(0, tiposDisponiveis.Count)
            ];

        List<HouseScript> casasLivres =
            new List<HouseScript>();

        foreach (HouseScript casa in casas)
        {
            if (!casa.ocupada)
                casasLivres.Add(casa);
        }

        if (casasLivres.Count == 0)
            return;

        HouseScript escolhida =
            casasLivres[
                Random.Range(0, casasLivres.Count)
            ];

        GameObject prefab = null;

        switch (tipo)
        {
            case TipoOcorrencia.Incendio:
                prefab = incendioPrefab;
                incendiosAtivos++;
                break;

            case TipoOcorrencia.Crime:
                prefab = crimePrefab;
                crimesAtivos++;
                break;

            case TipoOcorrencia.Medica:
                prefab = medicaPrefab;
                medicasAtivas++;
                break;
        }

        GameObject sinal =
            Instantiate(
                prefab,
                escolhida.transform.position,
                Quaternion.identity
            );

        escolhida.ocupada = true;
        escolhida.tipoAtual = tipo;
        escolhida.sinalizacaoAtual = sinal;
    }
    public void RemoverOcorrencia(TipoOcorrencia tipo)
    {
        switch (tipo)
        {
            case TipoOcorrencia.Incendio:
                incendiosAtivos--;
                break;

            case TipoOcorrencia.Crime:
                crimesAtivos--;
                break;

            case TipoOcorrencia.Medica:
                medicasAtivas--;
                break;
        }
    }
}