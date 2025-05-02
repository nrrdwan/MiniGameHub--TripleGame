using UnityEngine;
using UnityEngine.SceneManagement; // Kalau mau reload scene saat Game Over

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Supaya bisa dipanggil dari mana saja
    public int nyawa = 3;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // pastikan hanya 1 GameManager
    }

    public void KurangiNyawa()
    {
        nyawa--;

        if (nyawa <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Contoh: reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
