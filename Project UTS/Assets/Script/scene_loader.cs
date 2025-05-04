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
    public float loadingSpeed = 0.5f;
    public CanvasGroup fadeCanvasGroup;
    public float fadeOutDuration = 0.5f;

    [SerializeField] private string sceneNameBuild; // Fallback nama scene untuk runtime (build)

#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneToLoad; // Hanya digunakan di editor
#endif

    private string sceneName;

    void Awake()
    {
#if UNITY_EDITOR
        if (sceneToLoad != null)
        {
            sceneName = sceneToLoad.name;
        }
        else
        {
            sceneName = sceneNameBuild;
        }
#else
        sceneName = sceneNameBuild;
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
            Debug.LogError("Scene name is not set! Please fill in 'sceneNameBuild' in the inspector.");
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

            if (loadingfill != null)
                loadingfill.fillAmount = currentProgress;

            yield return null;
        }

        // Fade-out sebelum pindah scene
        yield return StartCoroutine(FadeOut());

        loading.allowSceneActivation = true;
    }

    IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogWarning("Fade CanvasGroup is not assigned.");
            yield break;
        }

        float time = 0f;
        fadeCanvasGroup.gameObject.SetActive(true);

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeOutDuration);
            yield return null;
        }
    }
}
