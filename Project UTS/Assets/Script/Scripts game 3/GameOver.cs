using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            Data.score = 0;
            SceneManager.LoadScene("Gameplay");
        }
    }
}
