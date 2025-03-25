using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public GameObject[] infoImages; // Array untuk 9 gambar
    private bool isImagesVisible = false; // Status apakah gambar ditampilkan

    void Start()
    {
        // Sembunyikan semua gambar di awal
        foreach (GameObject img in infoImages)
        {
            img.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Klik kanan
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Jika yang diklik adalah planet ini
                {
                    ToggleImages();
                }
            }
        }
    }

    void ToggleImages()
    {
        isImagesVisible = !isImagesVisible; // Ubah status tampilan

        // Tampilkan/sembunyikan semua gambar sekaligus
        foreach (GameObject img in infoImages)
        {
            img.SetActive(isImagesVisible);
        }
    }
}
