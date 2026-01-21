using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TextFadeBehavior : MonoBehaviour
{
    public bool isLocation = false;
    public TMP_Text text;
    public CanvasGroup titleCanvasGroup;
    private float fadeDuration = 1.5f;
    private float displayTime = 2f;

    private void Start()
    {
        if (isLocation)
        {
            text.SetText(FormatName(SceneManager.GetActiveScene().name));
        }
        StartCoroutine(FadeTitle());
    }

    IEnumerator FadeTitle()
    {
        titleCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayTime);
        yield return Fade(1f, 0f, fadeDuration);
        gameObject.SetActive(false);
    }


    IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            titleCanvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        titleCanvasGroup.alpha = end;
    }

    string FormatName(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        System.Text.StringBuilder newText = new System.Text.StringBuilder();
        newText.Append(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]) && char.IsLower(input[i - 1]))
            {
                newText.Append(' ');
            }
            newText.Append(input[i]);
        }

        return newText.ToString();
    }
}