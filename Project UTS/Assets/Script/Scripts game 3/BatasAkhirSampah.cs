using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatasAkhirsampah : MonoBehaviour
{
    public int nyawa = 3; // jumlah nyawa awal

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject); // Hapus sampah yang menabrak

        nyawa--; // Kurangi nyawa 1

        if (nyawa <= 0)
        {
            // Kalau nyawa habis, pindah ke scene Gameover
            SceneManager.LoadScene("Gameover");
        }
    }
}
