using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOutManager : MonoBehaviour
{
    public bool isOver;
    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private GameObject gameWonCanvas;
    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject pauseButton;

    public Paddle paddleScript;
    // Start is called before the first frame update
    void Start()
    {
        isOver = false;
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(false);
    }

    public void GameOver()
    {
        isOver = true;
        pauseButton.SetActive(false);
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameWon()
    {
        gameWonCanvas.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        paddleScript.StopBluetooth();
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
