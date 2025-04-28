using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PageManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneButtonPair
    {
        public Button button;

#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif
        [HideInInspector] public string sceneName;
    }

    public SceneButtonPair[] buttons;
    public CanvasGroup fadePanel; // Fade panel opsional
    public float fadeDuration = 1f; // <--- Tambahkan ini, durasi dalam detik

    void Awake()
    {
#if UNITY_EDITOR
        foreach (var pair in buttons)
        {
            if (pair.sceneAsset != null)
            {
                pair.sceneName = pair.sceneAsset.name;
            }
        }
#endif

        foreach (var pair in buttons)
        {
            string sceneToLoad = pair.sceneName;
            pair.button.onClick.AddListener(() => StartCoroutine(FadeAndLoadScene(sceneToLoad)));
        }
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadePanel.alpha = t / fadeDuration; // <-- gunakan pembagian supaya fade tepat waktu
                yield return null;
            }
            fadePanel.alpha = 1f; // Pastikan alpha 1 setelah selesai
        }

        SceneManager.LoadScene(sceneName);
    }
}
