using System.Collections;
using UnityEngine;

public class fade_in : MonoBehaviour
{
    public GameObject[] levelButtons;
    public CanvasGroup fadePanel;  // Panel yang akan di-fade-in
    public float fadeInDuration = 0.6f;  // Durasi fade-in yang bisa diatur
    public float buttonDelay = 0.3f; // Waktu delay sebelum tombol muncul

    void Start()
    {
        foreach (var btn in levelButtons)
        {
            btn.SetActive(false);
        }

        StartCoroutine(FadeInAllButtons());
    }

    IEnumerator FadeInAllButtons()
    {
        // Fade panel terlebih dahulu
        float time = 0f;
        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, time / fadeInDuration);  // Panel fade-out

            yield return null;
        }
        fadePanel.alpha = 0f;  // Pastikan panel selesai fade-out

        // Delay sebelum tombol muncul
        yield return new WaitForSeconds(buttonDelay);

        // Aktifkan tombol dan lakukan fade-in
        foreach (var btn in levelButtons)
        {
            btn.SetActive(true);
            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg == null) cg = btn.AddComponent<CanvasGroup>();
            cg.alpha = 0f;  // Mulai dengan alpha 0
        }

        // Lakukan fade-in tombol satu per satu
        float buttonTime = 0f;
        while (buttonTime < fadeInDuration)
        {
            buttonTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, buttonTime / fadeInDuration);

            foreach (var btn in levelButtons)
            {
                btn.GetComponent<CanvasGroup>().alpha = alpha;
            }

            yield return null;
        }

        // Pastikan tombol benar-benar terlihat
        foreach (var btn in levelButtons)
        {
            btn.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
}
