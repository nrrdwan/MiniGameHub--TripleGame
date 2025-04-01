using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetInfo : MonoBehaviour
{
    public GameObject infoImage; // Satu gambar saja
    private bool isImageVisible = false; // Status apakah gambar ditampilkan
    public PlanetController planetController; // Referensi ke PlanetController
    public Material glitchMaterial; // Material shader glitch

    private Vector3 originalScale;
    private Material originalMaterial;

    public AudioSource audioSource; // AudioSource dari planet
    public AudioClip glitchSound;   // Sound effect glitch (MP3)

    void Start()
    {
        originalScale = infoImage.transform.localScale / transform.localScale.x;
        infoImage.SetActive(false);

        Image imgComponent = infoImage.GetComponent<Image>();
        if (imgComponent != null)
        {
            originalMaterial = imgComponent.material;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    ToggleImage();
                }
            }
        }

        UpdateImageScale();
    }

    void ToggleImage()
    {
        isImageVisible = !isImageVisible;

        if (isImageVisible)
        {
            infoImage.SetActive(true);
            StartCoroutine(ApplyGlitchEffect(infoImage, true));
        }
        else
        {
            StartCoroutine(ApplyGlitchEffect(infoImage, false));
        }
    }

    void UpdateImageScale()
    {
        Vector3 newScale = originalScale * transform.localScale.x;
        Vector3 maxImageScale = new Vector3(19.2f, 10.60774f, 10.60774f);

        newScale.x = Mathf.Min(newScale.x, maxImageScale.x);
        newScale.y = Mathf.Min(newScale.y, maxImageScale.y);
        newScale.z = Mathf.Min(newScale.z, maxImageScale.z);

        infoImage.transform.localScale = newScale;
    }

    IEnumerator ApplyGlitchEffect(GameObject imgObj, bool isAppearing)
    {
        Image imgComponent = imgObj.GetComponent<Image>();
        if (imgComponent == null) yield break;

        imgComponent.material = glitchMaterial;

        if (audioSource != null && glitchSound != null)
        {
            audioSource.PlayOneShot(glitchSound);
        }

        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            float randomIntensity = Random.Range(0.001f, 0.015f);
            glitchMaterial.SetFloat("_GlitchIntensity", randomIntensity);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        glitchMaterial.SetFloat("_GlitchIntensity", 0f); // Paksa glitch jadi 0 saat selesai
        imgComponent.material = originalMaterial;

        if (!isAppearing)
        {
            yield return new WaitForSeconds(0.1f);
            imgObj.SetActive(false);
        }

        // Paksa hentikan suara jika masih berjalan
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ForceHideInfoImage()
    {
        if (isImageVisible)
        {
            isImageVisible = false;
            infoImage.SetActive(false);
        }
    }
}