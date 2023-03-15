using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public float fadeInDuration;
    public float fadeOutDuration;
    CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }
    public IEnumerator FadeOutIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }
    public IEnumerator FadeOut(float time)
    {
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = canvasGroup.alpha + Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while(canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        // ¼ÇµÃÉ¾³ý
        Destroy(gameObject);
    }
}
