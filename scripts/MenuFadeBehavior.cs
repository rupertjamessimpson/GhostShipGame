using System.Collections;
using UnityEngine;

public class MenuFadeBehavior : MonoBehaviour
{
    public CanvasGroup titleCanvasGroup;
    public float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(Fade(0f, 1f, fadeDuration));
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;

        titleCanvasGroup.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            titleCanvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        titleCanvasGroup.alpha = end;
    }
}
