using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameOverCanvas;
    public GameObject pauseCanvas;
    public GameObject pauseButton;


    void Start()
    {
        gameOverCanvas.SetActive(false);
    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pauseCanvas.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
}
