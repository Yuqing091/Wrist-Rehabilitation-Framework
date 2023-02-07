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
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameWon()
    {
        gameWonCanvas.SetActive(true);
        Time.timeScale = 0;
        paddleScript.StopBluetooth();
    }
}
