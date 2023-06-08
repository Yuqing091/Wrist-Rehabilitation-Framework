using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;

    [SerializeField]
    private Character characterScript;

    private float transitionTime = 1f;



    public void LoadMain()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(0));
        
    }

    public void LoadFirstGame()
    {
        StartCoroutine(LoadGame(1));
        Time.timeScale = 1;
    }

    public void LoadSecondGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(2));
        
    }

    public void LoadThirdGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(3));
    }

    public void LoadForthGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(4));
        
    }

    public void LoadFifthGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(5));

    }

    //Load the scene with index of 6
    public void LoadSixthGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGame(6));

    }

    //Coroutine method to load scenes
    IEnumerator LoadGame(int gameIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(gameIndex);
    }
}
