using UnityEngine;

public class PopIn : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Pop());
    }

    private System.Collections.IEnumerator Pop()
    {
        transform.localScale = Vector3.zero;

        while (transform.localScale.x < 1f)
        {
            transform.localScale +=
                Vector3.one * 5f * Time.deltaTime;

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}