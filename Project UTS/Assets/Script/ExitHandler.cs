using UnityEngine;
using UnityEngine.UI;

public class ExitHandler : MonoBehaviour
{
    [Header("Panel Konfirmasi")]
    public GameObject panelKeluar;

    [Header("Tombol UI")]
    public Button tombolExit; // Tombol utama untuk buka panel keluar
    public Button tombolYa;   // Tombol konfirmasi keluar
    public Button tombolTidak; // Tombol batal keluar

    void Start()
    {
        if (panelKeluar != null)
            panelKeluar.SetActive(false);

        if (tombolExit != null)
            tombolExit.onClick.AddListener(TampilkanPanelKeluar);

        if (tombolYa != null)
            tombolYa.onClick.AddListener(ExitGame);

        if (tombolTidak != null)
            tombolTidak.onClick.AddListener(BatalKeluar);
    }

    public void TampilkanPanelKeluar()
    {
        if (panelKeluar != null)
            panelKeluar.SetActive(true);
    }

    public void BatalKeluar()
    {
        if (panelKeluar != null)
            panelKeluar.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Keluar dari permainan...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
