using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeteksiSampah : MonoBehaviour
{
    public string nameTag;
    public AudioClip audioBenar;
    public AudioClip audioSalah;
    public Text textScore;

    private AudioSource mediaPlayerBenar;
    private AudioSource mediaPlayerSalah;

    private int score = 0;

    void Start()
    {
        UINyawa.nyawaTersisa = 3;

        mediaPlayerBenar = gameObject.AddComponent<AudioSource>();
        mediaPlayerBenar.clip = audioBenar;

        mediaPlayerSalah = gameObject.AddComponent<AudioSource>();
        mediaPlayerSalah.clip = audioSalah;

        score = 0;
        textScore.text = score.ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameTag))
        {
            score += 25;
            mediaPlayerBenar.Play();
        }
        else
        {
            score -= 5;
            mediaPlayerSalah.Play();
        }

        textScore.text = Mathf.Max(0, score).ToString();
        PlayerPrefs.SetInt("LastScore", Mathf.Max(0, score));

        Destroy(collision.gameObject);
    }
}
