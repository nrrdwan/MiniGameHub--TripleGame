using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetInfo : MonoBehaviour
{
    public GameObject[] infoImages; // Array untuk 9 gambar
    private bool isImagesVisible = false; // Status apakah gambar ditampilkan
    public PlanetController planetController; // Referensi ke PlanetController
    public Material glitchMaterial; // Material shader glitch

    private Vector3[] originalScales; // Untuk menyimpan skala awal gambar relatif terhadap planet
    private Material[] originalMaterials; // Untuk menyimpan material asli dari gambar

    void Start()
    {
        // Sembunyikan semua gambar di awal dan simpan skala awal relatif terhadap planet
        originalScales = new Vector3[infoImages.Length];
        originalMaterials = new Material[infoImages.Length];

        for (int i = 0; i < infoImages.Length; i++)
        {
            originalScales[i] = infoImages[i].transform.localScale / transform.localScale.x;
            infoImages[i].SetActive(false);

            // Simpan material asli dari UI Image
            Image imgComponent = infoImages[i].GetComponent<Image>();
            if (imgComponent != null)
            {
                originalMaterials[i] = imgComponent.material;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Klik kanan untuk menampilkan gambar
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

        // Update skala gambar mengikuti skala planet
        UpdateImageScale();
    }

    void ToggleImages()
    {
        isImagesVisible = !isImagesVisible; // Ubah status tampilan

        for (int i = 0; i < infoImages.Length; i++)
        {
            if (isImagesVisible)
            {
                infoImages[i].SetActive(true);
                StartCoroutine(ApplyGlitchEffect(infoImages[i], true)); // Efek glitch saat gambar muncul
            }
            else
            {
                StartCoroutine(ApplyGlitchEffect(infoImages[i], false)); // Efek glitch saat gambar hilang
            }
        }
    }

    void UpdateImageScale()
    {
        for (int i = 0; i < infoImages.Length; i++)
        {
            Vector3 newScale = originalScales[i] * transform.localScale.x;

            // Batasi ukuran maksimal gambar
            Vector3 maxImageScale = new Vector3(19.2f, 10.60774f, 10.60774f);
            newScale.x = Mathf.Min(newScale.x, maxImageScale.x);
            newScale.y = Mathf.Min(newScale.y, maxImageScale.y);
            newScale.z = Mathf.Min(newScale.z, maxImageScale.z);

            infoImages[i].transform.localScale = newScale;
        }
    }

    IEnumerator ApplyGlitchEffect(GameObject imgObj, bool isAppearing)
    {
        Image imgComponent = imgObj.GetComponent<Image>();
        if (imgComponent == null) yield break;

        // Terapkan material glitch ke gambar
        imgComponent.material = glitchMaterial;

        float elapsedTime = 0f;
        float duration = 0.3f; // Efek glitch berlangsung selama 0.3 detik

        while (elapsedTime < duration)
        {
            // Acak intensitas glitch setiap frame untuk animasi dinamis
            float randomIntensity = Random.Range(0.001f, 0.015f);
            glitchMaterial.SetFloat("_GlitchIntensity", randomIntensity);

            elapsedTime += Time.deltaTime;
            yield return null; // Tunggu frame berikutnya
        }

        // Set glitch intensity ke 0 sebelum kembali ke material asli
        glitchMaterial.SetFloat("_GlitchIntensity", 0);

        yield return new WaitForSeconds(0.1f); // Tunggu sebentar agar glitch 0 terlihat

        // Kembalikan material asli setelah glitch selesai
        imgComponent.material = originalMaterials[System.Array.IndexOf(infoImages, imgObj)];

        if (!isAppearing)
        {
            yield return new WaitForSeconds(0.1f); // Tunggu sebentar sebelum menonaktifkan gambar
            imgObj.SetActive(false);
        }
    }
}
