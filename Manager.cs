using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private GameObject gameWonCanvas;
    [SerializeField]
    private GameObject inGameCanvas;
    [SerializeField]
    private GameObject pauseCanvas;


    private void Start()
    {
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Horror");
        Time.timeScale = 1;

    }
    public void Pause()
    {
        pauseCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void GameOver()
    {

        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void GameWon()
    {
        pauseCanvas.SetActive(false);
        gameWonCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
    }

    public void InGame()
    {

        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        Time.timeScale = 1;
    }


}