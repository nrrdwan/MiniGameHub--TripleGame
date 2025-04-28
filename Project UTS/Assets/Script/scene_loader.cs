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

        // Delay sebelum pindah scene
        yield return new WaitForSeconds(0.5f);
        loading.allowSceneActivation = true;
    }
}
