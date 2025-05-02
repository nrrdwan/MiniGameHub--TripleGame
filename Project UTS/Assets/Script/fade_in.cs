using System.Collections;
using UnityEngine;

public class fade_in : MonoBehaviour
{
    public CanvasGroup fadePanel;  // Panel yang akan di-fade-in
    public float fadeInDuration = 0.6f;  // Durasi fade-in yang bisa diatur

    void Start()
    {
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        float time = 0f;
        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, time / fadeInDuration);  // Fade-out dari 1 ke 0
            yield return null;
        }

        // Pastikan alpha 0 di akhir
        fadePanel.alpha = 0f;
    }
}
