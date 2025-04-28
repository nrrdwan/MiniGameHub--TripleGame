using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fade_out : MonoBehaviour
{
    public GameObject[] levelButtons;
    private bool isFadingOut = false;

    // Menambahkan variabel durasi untuk fade-out yang bisa diatur dari Inspector
    public float fadeOutDuration = 0.5f;

    void Start()
    {
        foreach (var btn in levelButtons)
        {
            btn.SetActive(true);

            // Tambahkan komponen CanvasGroup jika belum ada
            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg == null) btn.AddComponent<CanvasGroup>();

            // Tambahkan listener ke tombol
            Button button = btn.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnButtonPressed(btn));
            }
        }
    }

    public void OnButtonPressed(GameObject clickedButton)
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutAllButtons(clickedButton));
        }
    }

    IEnumerator FadeOutAllButtons(GameObject clickedButton)
    {
        isFadingOut = true;

        float time = 0f;

        CanvasGroup[] canvasGroups = new CanvasGroup[levelButtons.Length];
        for (int i = 0; i < levelButtons.Length; i++)
        {
            canvasGroups[i] = levelButtons[i].GetComponent<CanvasGroup>();
        }

        // Proses fade-out dengan durasi yang bisa diatur
        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / fadeOutDuration);

            foreach (var cg in canvasGroups)
            {
                cg.alpha = alpha;
            }

            yield return null;
        }

        // Semua tombol disembunyikan setelah fade-out selesai
        foreach (var btn in levelButtons)
        {
            btn.SetActive(false);
        }

        Debug.Log("Tombol ditekan: " + clickedButton.name);

        // TODO: Lanjutkan ke aksi setelah fade-out, misalnya pindah scene
        // SceneManager.LoadScene("NextScene");
    }
}
