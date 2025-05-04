using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text finalScoreText;
    public Text highScoreText;

    void Start()
    {
        // Ambil skor terakhir, pastikan tidak negatif
        int lastScore = Mathf.Max(0, PlayerPrefs.GetInt("LastScore", 0));
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update high score jika perlu
        if (lastScore > highScore)
        {
            highScore = lastScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Tampilkan nilai skor, termasuk jika = 0
        finalScoreText.text = "Score: " + lastScore.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
