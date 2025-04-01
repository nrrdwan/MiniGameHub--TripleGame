using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public float zoomSpeed = 40f; // Kecepatan zoom
    public float minScale = 10f; // Skala minimum planet
    public float maxScale = 90f; // Skala maksimum planet

    private Vector3 lastMousePosition;

    void Update()
    {
        // Zoom in/out dengan scroll wheel (mengubah skala planet)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Vector3 newScale = transform.localScale + Vector3.one * scroll * zoomSpeed;
            newScale = Vector3.Max(newScale, Vector3.one * minScale); // Batas bawah
            newScale = Vector3.Min(newScale, Vector3.one * maxScale); // Batas atas
            transform.localScale = newScale;
        }
    }
}