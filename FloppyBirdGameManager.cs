using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class FloppyBirdGameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public GameObject inGameCanvas;
    public GameObject pauseCanvas;


    private void Start()
    {
        gameOverCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

    }
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        //inGameCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void Replay()
    {

        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pauseCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        Time.timeScale = 1;
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }


}