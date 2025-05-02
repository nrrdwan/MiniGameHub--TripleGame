using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class scene_loader : MonoBehaviour
{
    public Image loadingfill;
    public float loadingSpeed = 0.5f; // Bisa kamu atur kecepatannya di Inspector
    public CanvasGroup fadeCanvasGroup; // Tambahkan CanvasGroup untuk efek fade-out
    public float fadeOutDuration = 0.5f;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneToLoad;
#endif

    private string sceneName;

    void Awake()
    {
#if UNITY_EDITOR
        if (sceneToLoad != null)
        {
            sceneName = sceneToLoad.name;
        }
#endif
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(Loading());
        }
        else
        {
            Debug.LogError("Scene name is not set!");
        }
    }

    IEnumerator Loading()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;

        float currentProgress = 0f;

        while (currentProgress < 1f)
        {
            float targetProgress = Mathf.Clamp01(loading.progress / 0.9f);
            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime * loadingSpeed);
            loadingfill.fillAmount = currentProgress;

            yield return null;
        }

        // Loading selesai, sekarang fade-out
        yield return StartCoroutine(FadeOut());

        // Setelah fade-out selesai, pindah scene
        loading.allowSceneActivation = true;
    }

    IEnumerator FadeOut()
    {
        float time = 0f;
        fadeCanvasGroup.gameObject.SetActive(true); // Pastikan aktif

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeOutDuration);
            yield return null;
        }
    }
}
