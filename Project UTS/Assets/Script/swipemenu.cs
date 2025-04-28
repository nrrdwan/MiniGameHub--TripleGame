using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swipemenu : MonoBehaviour
{
    public ScrollRect scrollRect; // Drag komponen ScrollRect ke sini via Inspector
    float scroll_pos = 0;
    float[] pos;

    void Start()
    {
        int middleIndex = 1; // Index tombol ke-2
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }

        scroll_pos = pos[middleIndex];
        scrollRect.horizontalNormalizedPosition = scroll_pos;

        // Set scale awal biar langsung tampil benar
        for (int i = 0; i < transform.childCount; i++) {
            if (i == middleIndex) {
                transform.GetChild(i).localScale = new Vector2(1f, 1f);
            } else {
                transform.GetChild(i).localScale = new Vector2(0.8f, 0.8f);
            }
        }
    }

    void Update()
    {
        float distance = 1f / (pos.Length - 1f);

        if (Input.GetMouseButton(0)) {
            scroll_pos = scrollRect.horizontalNormalizedPosition;
        } else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++) {
                    if (a != i) {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
