using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    private int force = 150;
    private float maxSpeed = 10f; // Batas kecepatan maksimum
    private Rigidbody2D rigid;
    private int scoreP1;
    private int scoreP2;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreUITextP1;
    public TextMeshProUGUI scoreUITextP2;
    public GameObject panelSelesai;
    public GameObject astronotWinDisplay;
    public GameObject alienWinDisplay;
    public GameObject panelSeri;

    [Header("Timer UI")]
    public TextMeshProUGUI timerTextUI;

    [Header("Timer Settings")]
    public float totalWaktu = 120f;
    private float sisaWaktu;
    private bool bolaBergerak = false;

    [Header("Sound")]
    public AudioClip paddleHitSound;
    public AudioClip goalSound;
    private AudioSource audioSource;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            PhysicsMaterial2D material = new PhysicsMaterial2D();
            material.bounciness = 0.9f; // Dikurangi dari 1f
            material.friction = 0f;
            collider.sharedMaterial = material;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource belum ditambahkan ke GameObject bola!");
        }

        scoreP1 = 0;
        scoreP2 = 0;

        if (scoreUITextP1 == null || scoreUITextP2 == null)
            Debug.LogError("Score UI Text belum di-drag ke Inspector!");

        if (panelSelesai == null)
            Debug.LogError("Panel selesai belum di-drag ke Inspector!");
        else
            panelSelesai.SetActive(false);

        if (panelSeri == null)
            Debug.LogError("Panel Seri belum di-drag ke Inspector!");
        else
            panelSeri.SetActive(false);

        if (astronotWinDisplay == null)
            Debug.LogError("Astronot Win Display belum di-drag ke Inspector!");

        if (alienWinDisplay == null)
            Debug.LogError("Alien Win Display belum di-drag ke Inspector!");

        if (timerTextUI == null)
        {
            GameObject go = GameObject.Find("Time");
            if (go != null)
                timerTextUI = go.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogError("GameObject bernama 'Time' tidak ditemukan di scene!");
        }

        sisaWaktu = totalWaktu;
        MulaiBola();
    }

    void Update()
    {
        bolaBergerak = rigid.velocity.magnitude > 0.1f;

        if (bolaBergerak && sisaWaktu > 0f)
        {
            sisaWaktu -= Time.deltaTime;
            if (sisaWaktu < 0f)
                sisaWaktu = 0f;

            if (timerTextUI != null)
                timerTextUI.text = FormatWaktu(sisaWaktu);

            if (sisaWaktu <= 0f)
            {
                Debug.Log("Waktu habis!");

                if (scoreP1 > scoreP2)
                    TampilkanPemenang(true);
                else if (scoreP2 > scoreP1)
                    TampilkanPemenang(false);
                else
                    TampilkanSeri();
            }
        }

        // Batasi kecepatan maksimum
        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }
    }

    void ResetBall()
    {
        transform.localPosition = Vector2.zero;
        rigid.velocity = Vector2.zero;

        Invoke(nameof(MulaiBola), 1f);
    }

    void MulaiBola()
    {
        float randomY = Random.Range(-1f, 1f);
        float randomDirection = Random.value < 0.5f ? -1f : 1f;
        Vector2 arah = new Vector2(randomDirection, randomY).normalized;
        rigid.velocity = arah * force;
    }

    void TampilkanScore()
    {
        if (scoreUITextP1 != null)
            scoreUITextP1.text = scoreP1.ToString();

        if (scoreUITextP2 != null)
            scoreUITextP2.text = scoreP2.ToString();

        Debug.Log($"Score P1: {scoreP1} Score P2: {scoreP2}");
    }

    void TampilkanPemenang(bool isAstronotMenang)
    {
        if (panelSelesai != null)
            panelSelesai.SetActive(true);

        if (isAstronotMenang)
        {
            if (astronotWinDisplay != null)
                astronotWinDisplay.SetActive(true);
            if (alienWinDisplay != null)
                alienWinDisplay.SetActive(false);

            Debug.Log("Astronot menang!");
        }
        else
        {
            if (alienWinDisplay != null)
                alienWinDisplay.SetActive(true);
            if (astronotWinDisplay != null)
                astronotWinDisplay.SetActive(false);

            Debug.Log("Alien menang!");
        }

        Destroy(gameObject);
    }

    void TampilkanSeri()
    {
        if (panelSelesai != null)
            panelSelesai.SetActive(true);

        if (panelSeri != null)
            panelSeri.SetActive(true);

        if (astronotWinDisplay != null)
            astronotWinDisplay.SetActive(false);

        if (alienWinDisplay != null)
            alienWinDisplay.SetActive(false);

        Debug.Log("Pertandingan Seri!");

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (goalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(goalSound);
        }

        if (other.gameObject.name == "TepiKanan")
        {
            scoreP1++;
            TampilkanScore();

            if (scoreP1 == 5)
            {
                Debug.Log("Skor P1 mencapai 5 - Alien menang");
                TampilkanPemenang(true);
                return;
            }

            ResetBall();
        }

        if (other.gameObject.name == "TepiKiri")
        {
            scoreP2++;
            TampilkanScore();

            if (scoreP2 == 5)
            {
                Debug.Log("Skor P2 mencapai 5 - Astronot menang");
                TampilkanPemenang(false);
                return;
            }

            ResetBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigid.velocity.magnitude < 0.5f)
        {
            Debug.Log("Bola hampir berhenti, mengatur ulang kecepatan!");
            Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rigid.velocity = newDirection * force * 0.5f; // Ganti dari AddForce ke velocity langsung
        }

        if (collision.gameObject.CompareTag("Paddle") && paddleHitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(paddleHitSound);
        }
    }

    string FormatWaktu(float waktu)
    {
        int menit = Mathf.FloorToInt(waktu / 60f);
        int detik = Mathf.FloorToInt(waktu % 60f);
        return string.Format("{0:00}:{1:00}", menit, detik);
    }
}
