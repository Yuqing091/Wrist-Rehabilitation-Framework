using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Application Exited");
        Application.Quit();
    }
}

