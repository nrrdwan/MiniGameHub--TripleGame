using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UINyawa : MonoBehaviour
{
    [Header("Drag 3 UI Images (Heart1, Heart2, Heart3) here:")]
    public Image[] hearts;

    [Header("Drag your sprites here:")]
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    [Header("Audio Clips")]
    public AudioClip damageSound;

    private AudioSource audioSource;

    public static int nyawaTersisa = 3;
    private int nyawaSebelumnya = 3;
    private bool gameOverPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Deteksi jika nyawa berkurang
        if (nyawaTersisa < nyawaSebelumnya)
        {
            PlayDamageSound();
            nyawaSebelumnya = nyawaTersisa;
        }

        // Deteksi jika nyawa habis dan belum masuk Game Over
        if (nyawaTersisa <= 0 && !gameOverPlayed)
        {
            gameOverPlayed = true;
            StartCoroutine(HandleGameOver());
        }

        UpdateUI();
    }

    void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    IEnumerator HandleGameOver()
    {
        // Delay sedikit untuk efek
        yield return new WaitForSeconds(0.5f);

        // Pindah ke scene GameOver, audio akan dimainkan di sana
        SceneManager.LoadScene("GameOver");
    }

    void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (nyawaTersisa >= 3)
            {
                hearts[i].sprite = heartFull;
            }
            else if (nyawaTersisa == 2)
            {
                if (i == 0) hearts[i].sprite = heartFull;
                else if (i == 1) hearts[i].sprite = heartHalf;
                else hearts[i].sprite = heartEmpty;
            }
            else if (nyawaTersisa == 1)
            {
                if (i == 0) hearts[i].sprite = heartFull;
                else hearts[i].sprite = heartEmpty;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }
}
