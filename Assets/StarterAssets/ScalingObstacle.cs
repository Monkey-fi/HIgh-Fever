using System.Collections;
using UnityEngine;

public class SclingObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 newScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float scaleDelay = 5f;
    [SerializeField] private float scaleSpeed = 2f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool scalingToNew = true;
    private float startTime;

    private void Star()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        StartCoroutine(Scal());
    }

    private IEnumerator Scal()
    {
        while (true)
        {
            yield return new WaitForSeconds(scaleDelay);
            if (scalingToNew)
            {
                targetScale = newScale; // Set target scale to new scale
            }
            else
            {
                targetScale = originalScale; // Set target scale back to original
            }

            scalingToNew = !scalingToNew;


            while (Time.time < startTime + scaleSpeed)
            {
                float t = (Time.time - startTime) / scaleSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}