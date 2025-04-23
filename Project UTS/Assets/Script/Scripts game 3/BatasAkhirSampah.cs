using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatasAkhirsampah : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Tidak ada inisialisasi khusus
    }

    // Update is called once per frame
    void Update()
    {
        // Tidak ada logika per frame saat ini
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        SceneManager.LoadScene("Gameover");
    }
}
