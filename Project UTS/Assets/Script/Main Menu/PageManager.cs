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

        [Tooltip("Nama scene yang akan dibuka saat di-build. Harus sesuai dengan nama di Build Settings.")]
        public string sceneName; // Manual input scene name
    }

    public SceneButtonPair[] buttons;
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    void Awake()
    {
#if UNITY_EDITOR
        // Isi otomatis saat di Editor
        foreach (var pair in buttons)
        {
            if (pair.sceneAsset != null)
            {
                pair.sceneName = pair.sceneAsset.name;
            }
        }
#endif

        // Pastikan sceneName dipakai saat runtime
        foreach (var pair in buttons)
        {
            string sceneToLoad = pair.sceneName;

            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogWarning("Scene name kosong! Pastikan diisi di Inspector.");
                continue;
            }

            pair.button.onClick.AddListener(() => StartCoroutine(FadeAndLoadScene(sceneToLoad)));
        }
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            float t = 0f;
            fadePanel.gameObject.SetActive(true);

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadePanel.alpha = t / fadeDuration;
                yield return null;
            }

            fadePanel.alpha = 1f;
        }

        SceneManager.LoadScene(sceneName);
    }
}
