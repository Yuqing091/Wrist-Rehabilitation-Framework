using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameOverCanvas;

    void Start()
    {
        gameOverCanvas.SetActive(false);
    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}
