using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Click");
    }

    public void PlayWalk()
    {
        FindObjectOfType<AudioManager>().Play("Walk");
    }
    
}
