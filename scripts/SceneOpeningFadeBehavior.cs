using System.Collections;
using UnityEngine;

public class SceneOpeningFadeBehavior : MonoBehaviour
{
    public CanvasGroup titleCanvasGroup;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(Fade(1f, 0f, fadeDuration));
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
