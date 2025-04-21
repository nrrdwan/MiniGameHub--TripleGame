using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
            pair.button.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
        }
    }
}
