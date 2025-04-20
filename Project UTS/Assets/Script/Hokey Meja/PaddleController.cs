using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float batasAtas;
    public float batasBawah;
    public float batasKiri;
    public float batasKanan;
    public float kecepatan;
    public string axisVertikal;  // Menambahkan axis untuk vertikal
    public string axisHorizontal;  // Menambahkan axis untuk horizontal

    // Update is called once per frame 
    void Update()
    {
        // Gerakan vertikal (atas/bawah)
        float gerakVertikal = Input.GetAxis(axisVertikal) * kecepatan * Time.deltaTime;
        float nextPosVertikal = transform.position.y + gerakVertikal;
        nextPosVertikal = Mathf.Clamp(nextPosVertikal, batasBawah, batasAtas);

        // Gerakan horizontal (kiri/kanan)
        float gerakHorizontal = Input.GetAxis(axisHorizontal) * kecepatan * Time.deltaTime;
        float nextPosHorizontal = transform.position.x + gerakHorizontal;
        nextPosHorizontal = Mathf.Clamp(nextPosHorizontal, batasKiri, batasKanan);

        // Set posisi pemukul
        transform.position = new Vector3(nextPosHorizontal, nextPosVertikal, transform.position.z);
    }
}
