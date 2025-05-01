using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public bool isComputer;

    [Header("Batas Gerak")]
    public float batasAtas;
    public float batasBawah;
    public float batasKiri;
    public float batasKanan;

    [Header("Kontrol Manual")]
    public float kecepatan = 5f;
    public string axisVertikal = "Vertical";
    public string axisHorizontal = "Horizontal";

    [Header("AI / Bola")]
    public Rigidbody2D ballRb;
    public float kecepatanAI = 10f;
    public float aiSmoothing = 0.1f;
    public float jarakBolaUntukMengejar = 5f; // Jarak untuk mulai mengejar bola
    public float posisiTengah = 0f; // Posisi tengah, di mana AI harus berhenti mengejar jika bola sudah melewati

    private Rigidbody2D rb;
    private Vector3 posisiAwal;
    private bool bolaDiGoal;
    private bool bolaTelahDipantulkan; // Flag untuk memastikan bola hanya dipantulkan sekali

    void Awake()
    {
        posisiAwal = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bolaDiGoal = false;
        bolaTelahDipantulkan = false; // Bola belum dipantulkan
    }

    void Update()
    {
        if (!isComputer)
        {
            // Kontrol manual untuk pemain
            float gerakVertikal = Input.GetAxis(axisVertikal) * kecepatan * Time.deltaTime;
            float gerakHorizontal = Input.GetAxis(axisHorizontal) * kecepatan * Time.deltaTime;

            float nextY = Mathf.Clamp(transform.position.y + gerakVertikal, batasBawah, batasAtas);
            float nextX = Mathf.Clamp(transform.position.x + gerakHorizontal, batasKiri, batasKanan);

            transform.position = new Vector3(nextX, nextY, transform.position.z);
        }
        else
        {
            if (ballRb != null)
            {
                Vector3 posisi = transform.position;

                // Cek apakah bola dalam keadaan goal dan reset posisi AI jika perlu
                if (bolaDiGoal)
                {
                    transform.position = Vector3.Lerp(transform.position, posisiAwal, Time.deltaTime * kecepatanAI);
                    return; // Jangan gerak ke bola jika bola di goal
                }

                // Prediksi posisi bola
                Vector2 prediksiPosisi = ballRb.position + ballRb.velocity * Time.deltaTime;

                // Menghitung jarak antara AI dan bola
                float jarak = Vector2.Distance(prediksiPosisi, new Vector2(posisi.x, posisi.y));

                // Hanya mengejar bola jika bola cukup dekat dan bergerak menuju AI
                bool bolaMenujuAI = (ballRb.velocity.x > 0 && posisi.x < ballRb.position.x) || (ballRb.velocity.x < 0 && posisi.x > ballRb.position.x);

                // Jika bola cukup dekat dengan AI dan AI ada di sisi kiri
                if (bolaMenujuAI)
                {
                    // AI mengejar bola
                    Vector2 arah = (prediksiPosisi - new Vector2(posisi.x, posisi.y)).normalized;

                    // Penambahan sedikit random untuk menambah "kecerdikan" AI
                    arah += new Vector2(Random.Range(-aiSmoothing, aiSmoothing), Random.Range(-aiSmoothing, aiSmoothing));
                    arah.Normalize();

                    Vector2 gerakan = arah * kecepatanAI * Time.deltaTime;

                    // Batasi pergerakan AI dalam batas yang ditentukan
                    float nextX = Mathf.Clamp(posisi.x + gerakan.x, batasKiri, batasKanan);
                    float nextY = Mathf.Clamp(posisi.y + gerakan.y, batasBawah, batasAtas);

                    transform.position = new Vector3(nextX, nextY, posisi.z);
                }
                else
                {
                    // Jika bola sudah melewati area AI dan bergerak ke arah lawan, AI berhenti mengikuti bola dan kembali ke posisi awal
                    transform.position = Vector3.Lerp(transform.position, posisiAwal, Time.deltaTime * kecepatanAI * 2);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && ballRb != null && !bolaTelahDipantulkan)
        {
            Vector2 arahTabrakan = collision.contacts[0].normal;
            ballRb.AddForce(-arahTabrakan * 1f, ForceMode2D.Impulse);

            // Set bola telah dipantulkan, beri sedikit jeda sebelum bola dapat dipantulkan lagi
            bolaTelahDipantulkan = true;

            // Mengembalikan flag bolaTelahDipantulkan setelah beberapa waktu
            StartCoroutine(MulaiMengejarLagi());
        }
    }

    // Fungsi untuk memberikan jeda sebelum bola dapat dipantulkan lagi
    private IEnumerator MulaiMengejarLagi()
    {
        // Tunggu beberapa detik sebelum AI dapat memantulkan bola lagi
        yield return new WaitForSeconds(1f); // Jeda 0.5 detik (bisa disesuaikan)
        bolaTelahDipantulkan = false; // Setelah jeda, AI dapat memantulkan bola lagi
    }

    // Method untuk memanggil saat bola goal atau reset
    public void BolaDiGoal()
    {
        bolaDiGoal = true;
    }

    // Method untuk reset posisi AI setelah bola kembali ke tengah
    public void ResetAI()
    {
        bolaDiGoal = false;
        transform.position = posisiAwal;
    }
}
