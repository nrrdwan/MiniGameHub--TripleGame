using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetInfo : MonoBehaviour
{
    public GameObject infoImage;
    private bool isImageVisible = false;
    public PlanetController planetController;
    public Material glitchMaterial;

    private Vector3 originalScale;
    private Material originalMaterial;

    public AudioSource audioSource;
    public AudioClip glitchSound;

    public AudioClip[] descriptionSounds;
    private int currentDescriptionIndex = 0;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private bool isClosing = false; // Untuk mencegah play suara saat sedang close

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
            float currentTime = Time.time;

            if (currentTime - lastClickTime < doubleClickThreshold)
            {
                // Klik dua kali - Tutup deskripsi + glitch
                if (isImageVisible)
                {
                    isClosing = true;
                    audioSource.Stop(); // Stop semua suara aktif
                    StartCoroutine(ApplyGlitchEffect(infoImage, false));
                    isImageVisible = false;
                }
            }
            else
            {
                if (!isImageVisible)
                {
                    // Buka pertama kali - glitch + play suara pertama
                    isClosing = false;
                    isImageVisible = true;
                    infoImage.SetActive(true);
                    StartCoroutine(ApplyGlitchEffect(infoImage, true));
                }
                else
                {
                    // Ganti suara (tanpa glitch)
                    isClosing = false;
                    PlayDescriptionSound();
                }
            }

            lastClickTime = currentTime;
        }

        UpdateImageScale();
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

        glitchMaterial.SetFloat("_GlitchIntensity", 0f);
        imgComponent.material = originalMaterial;

        if (!isAppearing)
        {
            yield return new WaitForSeconds(0.1f);
            imgObj.SetActive(false);
        }

        if (isAppearing && descriptionSounds.Length > 0 && !isClosing)
        {
            yield return new WaitForSeconds(glitchSound.length);
            PlayDescriptionSound();
        }

        if (audioSource.isPlaying && !isAppearing)
        {
            audioSource.Stop();
        }
    }

    void PlayDescriptionSound()
    {
        if (descriptionSounds.Length == 0 || audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = descriptionSounds[currentDescriptionIndex];
        audioSource.Play();

        currentDescriptionIndex = (currentDescriptionIndex + 1) % descriptionSounds.Length;
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
