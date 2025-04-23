using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeteksiSampah : MonoBehaviour
{
    public string nameTag;
    public AudioClip audioBenar;
    public AudioClip audioSalah;
    public Text textScore;

    private AudioSource mediaPlayerBenar;
    private AudioSource mediaPlayerSalah;

    // Start is called before the first frame update
    void Start()
    {
        // Menambahkan AudioSource untuk audioBenar
        mediaPlayerBenar = gameObject.AddComponent<AudioSource>();
        mediaPlayerBenar.clip = audioBenar;

        // Menambahkan AudioSource untuk audioSalah
        mediaPlayerSalah = gameObject.AddComponent<AudioSource>();
        mediaPlayerSalah.clip = audioSalah;
    }

    // Update is called once per frame
    void Update()
    {
        // Tidak ada logika dalam Update saat ini
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(nameTag))
        {
            Data.score += 25;
            textScore.text = Data.score.ToString();
            mediaPlayerBenar.Play();
        }
        else
        {
            Data.score -= 5;
            textScore.text = Data.score.ToString();
            mediaPlayerSalah.Play();
        }

        Destroy(collision.gameObject);
    }
}
