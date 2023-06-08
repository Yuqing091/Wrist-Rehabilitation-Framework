using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyGoal : MonoBehaviour
{
    DateTime currentDate = DateTime.Now.Date;

    [SerializeField]
    private LoadRoutine loadRoutine;

    [SerializeField]
    private Text extensionGoalText;
    [SerializeField]
    private Text radialGoalText;
    [SerializeField]
    private Text pronationGoalText;
    [SerializeField]
    private Text fingerGoalText;

    

    public Character characterScript;
    public Finger fingerScript;
    public StreakSystem streakSystem;

    public bool exerciseComplete;
    public int extensionTarget;
    private int currentExtensionCompleted;
    public int radialTarget;
    private int currentRadialCompleted;
    public int pronationTarget;
    private int currentPronationCompleted;
    public int fingerTarget;
    private int currentFingerCompleted;

    DateTime lastDate = DateTime.MinValue;


    // Start is called before the first frame update
    void Start()
    {

        exerciseComplete = false;
        //reset current exercises done and implement streak
        string lastSavedDate = PlayerPrefs.GetString("LastSavedDate", "");
        PlayerPrefs.SetString("LastSavedDate", currentDate.ToString());

        if (!string.IsNullOrEmpty(lastSavedDate))
        {
            lastDate = DateTime.Parse(lastSavedDate);
        }

        if (currentDate > lastDate)
        {
            // Reset PlayerPrefs value
            PlayerPrefs.SetInt("CurrentExtension", 0);
            PlayerPrefs.SetInt("CurrentRadial", 0);
            PlayerPrefs.SetInt("CurrentPronation", 0);
            PlayerPrefs.SetInt("CurrentFinger", 0);

            // Save the new date to PlayerPrefs
            PlayerPrefs.SetString("LastSavedDate", currentDate.ToString());
            streakSystem.hasRun = false;
        }


        Debug.Log("exercisecomplete" + exerciseComplete);
        

    }

    // Check the completion of exercise sets for the day
    void Update()
    {
        if (characterScript.currentExtension >= extensionTarget)
        {
            extensionGoalText.text = "Extension/Flexion    " + extensionTarget + "/" + extensionTarget;
        }
        else
        {
            currentExtensionCompleted = characterScript.currentExtension;
            extensionGoalText.text = "Extension/Flexion    " + currentExtensionCompleted + "/" + extensionTarget;
        }

        if (characterScript.currentRadial >= radialTarget)
        {
            radialGoalText.text = "Radial/Ulnar    " + radialTarget + "/" + radialTarget;
        }
        else
        {
            currentRadialCompleted = characterScript.currentRadial;
            radialGoalText.text = "Radial/Ulnar    " + currentRadialCompleted + "/" + radialTarget;
        }

        if (characterScript.currentPronation >= pronationTarget)
        {
            pronationGoalText.text = "Pronation/Supination    " + pronationTarget + "/" + pronationTarget;
        }
        else
        {
            currentPronationCompleted = characterScript.currentPronation;
            pronationGoalText.text = "Pronation/Supination    " + currentPronationCompleted + "/" + pronationTarget;
        }

        if (fingerScript.currentFinger >= fingerTarget)
        {
            fingerGoalText.text = "Fingers    " + fingerTarget + "/" + fingerTarget;
        }
        else
        {
            currentFingerCompleted = fingerScript.currentFinger;
            fingerGoalText.text = "Fingers    " + currentFingerCompleted + "/" + fingerTarget;
        }

        if(characterScript.currentExtension >= extensionTarget && characterScript.currentRadial >= radialTarget && characterScript.currentPronation >= pronationTarget && fingerScript.currentFinger >= fingerTarget)
        {
            exerciseComplete = true;
        }
        else
        {
            exerciseComplete = false;
        }
    }
}
