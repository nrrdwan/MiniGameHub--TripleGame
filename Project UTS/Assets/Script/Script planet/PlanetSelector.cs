using UnityEngine;

public class PlanetSelector : MonoBehaviour
{
    public GameObject[] planets; // Array untuk menyimpan planet
    private int currentIndex = 0; // Indeks planet yang sedang aktif
    private Vector3[] originalScales; // Simpan skala awal setiap planet

    void Start()
    {
        // Simpan skala awal dari semua planet
        originalScales = new Vector3[planets.Length];
        for (int i = 0; i < planets.Length; i++)
        {
            originalScales[i] = planets[i].transform.localScale;
        }

        UpdatePlanetVisibility();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) // Tombol D untuk ke kanan
        {
            currentIndex = (currentIndex + 1) % planets.Length;
            UpdatePlanetVisibility();
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Tombol A untuk ke kiri
        {
            currentIndex = (currentIndex - 1 + planets.Length) % planets.Length;
            UpdatePlanetVisibility();
        }
    }

    void UpdatePlanetVisibility()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetActive(i == currentIndex);

            // Reset skala planet ke ukuran awal saat diaktifkan
            if (i == currentIndex)
            {
                planets[i].transform.localScale = originalScales[i];
            }

            // Paksa sembunyikan info image
            PlanetInfo planetInfo = planets[i].GetComponent<PlanetInfo>();
            if (planetInfo != null)
            {
                planetInfo.ForceHideInfoImage();
            }
        }
    }
}
