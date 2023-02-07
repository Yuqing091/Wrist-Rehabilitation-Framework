using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGoal : MonoBehaviour
{
    [SerializeField]
    private Text extensionGoalText;
    [SerializeField]
    private Text radialGoalText;
    [SerializeField]
    private Text pronationGoalText;
    [SerializeField]
    private Text fingerGoalText;

    [SerializeField]
    private GameObject badge1;
    [SerializeField]
    private GameObject badge2;
    [SerializeField]
    private GameObject badge3;
    [SerializeField]
    private GameObject badge4;


    public Character characterScript;
    public Finger fingerScript;

    private int extensionTarget;
    private int currentExtensionCompleted;
    private int radialTarget;
    private int currentRadialCompleted;
    private int pronationTarget;
    private int currentPronationCompleted;
    private int fingerTarget;
    private int currentFingerCompleted;
    // Start is called before the first frame update
    void Start()
    {
        extensionTarget = 2;
        currentExtensionCompleted = 0;
        radialTarget = 2;
        currentRadialCompleted = 0;
        pronationTarget = 2;
        currentPronationCompleted = 0;
        fingerTarget = 2;
        currentFingerCompleted = 0;
        badge1.SetActive(false);
        badge2.SetActive(false);
        badge3.SetActive(false);
        badge4.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (characterScript.currentExtension >= extensionTarget)
        {
            extensionGoalText.text = "Extension/Flexion    " + extensionTarget + "/" + extensionTarget;
            badge1.SetActive(true);
        }
        else
        {
            currentExtensionCompleted = characterScript.currentExtension;
            extensionGoalText.text = "Extension/Flexion    " + currentExtensionCompleted + "/" + extensionTarget;
        }

        if (characterScript.currentRadial >= radialTarget)
        {
            radialGoalText.text = "Radial/Ulnar    " + radialTarget + "/" + radialTarget;
            badge2.SetActive(true);
        }
        else
        {
            currentRadialCompleted = characterScript.currentRadial;
            radialGoalText.text = "Radial/Ulnar    " + currentRadialCompleted + "/" + radialTarget;
        }

        if (characterScript.currentPronation >= pronationTarget)
        {
            pronationGoalText.text = "Pronation/Supination    " + pronationTarget + "/" + pronationTarget;
            badge3.SetActive(true);
        }
        else
        {
            currentPronationCompleted = characterScript.currentPronation;
            pronationGoalText.text = "Pronation/Supination    " + currentPronationCompleted + "/" + pronationTarget;
        }

        if (fingerScript.currentFinger >= fingerTarget)
        {
            fingerGoalText.text = "Fingers    " + fingerTarget + "/" + fingerTarget;
            badge4.SetActive(true);
        }
        else
        {
            currentFingerCompleted = fingerScript.currentFinger;
            fingerGoalText.text = "Fingers    " + currentFingerCompleted + "/" + fingerTarget;
        }
    }
}
