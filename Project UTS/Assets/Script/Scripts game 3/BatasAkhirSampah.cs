using UnityEngine;
using UnityEngine.SceneManagement;

public class BatasAkhirsampah : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);    // Hapus objek yang menabrak

        // Mengurangi nyawa yang ada di UINyawa
        UINyawa.nyawaTersisa--;           // Nyawa berkurang 1

        // Cek apakah nyawa habis
        if (UINyawa.nyawaTersisa <= 0)
        {
            SceneManager.LoadScene("Gameover");  // Pindah ke scene Gameover
        }
    }
}
