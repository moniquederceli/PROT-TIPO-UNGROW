using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;

    public float fadeDuration = 0.5f;

    private void Awake()
    {
        SetAlpha(0f);
    }

    public void FadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeFromBlack()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsed / fadeDuration
            );

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(endAlpha);
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image não foi conectado!");
            return;
        }

        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}