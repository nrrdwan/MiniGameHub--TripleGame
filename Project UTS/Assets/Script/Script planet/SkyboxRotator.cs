using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public Transform mainCamera; // Referensi ke Main Camera
    public Transform planet; // Referensi ke planet
    public float rotationSpeed = 5.0f; // Kecepatan rotasi manual
    public float autoRotationSpeed = 2.0f; // Kecepatan rotasi otomatis
    private Vector3 lastMousePosition;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private float distanceToPlanet; // Jarak kamera ke planet
    private float idleTime = 0f; // Waktu idle tanpa input mouse
    public float timeToResumeAutoRotation = 3.0f; // Waktu sebelum kembali otomatis

    void Start()
    {
        if (mainCamera != null && planet != null)
        {
            distanceToPlanet = Vector3.Distance(mainCamera.position, planet.position); // Simpan jarak awal
        }

        // Reset rotasi skybox ke posisi awal
        rotationX = 0f;
        rotationY = 0f;
        RenderSettings.skybox.SetFloat("_Rotation", 0f);
    }

    void Update()
    {
        bool isMouseMoving = false;

        if (Input.GetMouseButton(0)) // Jika tombol kiri mouse ditekan
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotX = -delta.y * rotationSpeed * Time.deltaTime; // Rotasi vertikal
            float rotY = delta.x * rotationSpeed * Time.deltaTime;  // Rotasi horizontal

            if (delta.magnitude > 0.1f) // Cek jika ada pergerakan mouse
            {
                isMouseMoving = true;
                idleTime = 0f; // Reset waktu idle
            }

            rotationX = Mathf.Clamp(rotationX + rotX, -85f, 85f); // Batasi rotasi vertikal
            rotationY += rotY;
        }
        else
        {
            idleTime += Time.deltaTime; // Tambah waktu idle jika tidak ada input

            if (idleTime >= timeToResumeAutoRotation) // Jika tidak ada input selama beberapa detik
            {
                rotationY += autoRotationSpeed * Time.deltaTime; // Rotasi otomatis
            }
        }

        // Hitung rotasi baru dengan quaternion
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Set posisi kamera agar tetap berjarak dari planet
        if (mainCamera != null && planet != null)
        {
            mainCamera.position = planet.position - (rotation * Vector3.forward * distanceToPlanet);
            mainCamera.LookAt(planet); // Pastikan kamera selalu menghadap ke planet
        }

        // Sinkronkan rotasi skybox
        RenderSettings.skybox.SetFloat("_Rotation", rotationY);

        lastMousePosition = Input.mousePosition; // Simpan posisi mouse terakhir
    }
}
