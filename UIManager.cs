using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas;

    [SerializeField]
    private GameObject statusCanvas;

    [SerializeField]
    private GameObject clearStatusCanvas;

    [SerializeField]
    private GameObject setThresholdCanvas;

    [SerializeField]
    private GameObject rewardCanvas;

    [SerializeField]
    private GameObject headCanvas;

    [SerializeField]
    private GameObject leftArmCanvas;

    [SerializeField]
    private GameObject rightArmCanvas;

    [SerializeField]
    private GameObject leftHandCanvas;
    [SerializeField]
    private GameObject leftHandSelectGame;

    [SerializeField]
    private GameObject leftExtension;

    [SerializeField]
    private GameObject leftExtensionSelectGame;

    [SerializeField]
    private GameObject leftRadialSelectGame;

    [SerializeField]
    private GameObject leftPronationSelectGame;

    [SerializeField]
    private GameObject leftRadial;

    [SerializeField]
    private GameObject leftPronation;

    [SerializeField]
    private GameObject mainBody;


    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.SetActive(true);
        headCanvas.SetActive(false);
        leftArmCanvas.SetActive(false);
        leftExtension.SetActive(false);
        leftRadial.SetActive(false);
        leftPronation.SetActive(false);
        rightArmCanvas.SetActive(false);
        leftExtensionSelectGame.SetActive(false);
        setThresholdCanvas.SetActive(false);
        statusCanvas.SetActive(false);
        leftHandCanvas.SetActive(false);
        leftHandSelectGame.SetActive(false);
        clearStatusCanvas.SetActive(false);
        rewardCanvas.SetActive(false);
    }

    public void EnterSetThreshold()
    {
        mainCanvas.SetActive(false);
        setThresholdCanvas.SetActive(true);
        mainBody.SetActive(false);
    }

    public void EnterStatus()
    {
        mainCanvas.SetActive(false);
        statusCanvas.SetActive(true);
        mainBody.SetActive(false);
        clearStatusCanvas.SetActive(false);
    }

    public void EnterReward()
    {
        mainCanvas.SetActive(false);
        rewardCanvas.SetActive(true);
        mainBody.SetActive(false);
    }
    public void EnterClearStatus()
    {
        clearStatusCanvas.SetActive(true);
    }

    public void EnterLeftArm()
    {
        mainCanvas.SetActive(false);
        leftArmCanvas.SetActive(true);
        leftHandCanvas.SetActive(false);
        leftExtension.SetActive(false);
        leftRadial.SetActive(false);
        leftPronation.SetActive(false);
    }

    public void EnterLeftHand()
    {
        mainBody.SetActive(true);
        leftArmCanvas.SetActive(false);
        leftHandCanvas.SetActive(true);
        leftHandSelectGame.SetActive(false);
    }

    public void EnterLeftExtension()
    {
        mainBody.SetActive(true); 
        leftArmCanvas.SetActive(false);
        leftExtension.SetActive(true);
        leftExtensionSelectGame.SetActive(false);
    }




    public void EnterLeftRadial()
    {
        mainBody.SetActive(true);
        leftArmCanvas.SetActive(false);
        leftRadial.SetActive(true);
        leftRadialSelectGame.SetActive(false);
    }

    public void EnterLeftPronation()
    {
        mainBody.SetActive(true);
        leftArmCanvas.SetActive(false);
        leftPronation.SetActive(true);
        leftPronationSelectGame.SetActive(false);


    }

    public void EnterLeftFingerSelectGame()
    {
        leftHandCanvas.SetActive(false);
        leftHandSelectGame.SetActive(true);
        mainBody.SetActive(false);
    }

    public void EnterLeftExtensionSelectGame()
    {
        leftExtension.SetActive(false);
        leftExtensionSelectGame.SetActive(true);
        mainBody.SetActive(false);
    }
    public void EnterLeftRadialSelectGame()
    {
        leftRadial.SetActive(false);
        leftRadialSelectGame.SetActive(true);
        mainBody.SetActive(false);
    }

    public void EnterLeftPronationSelectGame()
    {
        leftPronation.SetActive(false);
        leftPronationSelectGame.SetActive(true);
        mainBody.SetActive(false);
    }

    public void EnterRightArm()
    {
        mainCanvas.SetActive(false);

        rightArmCanvas.SetActive(true);
    }

    public void EnterHead()
    {
        mainCanvas.SetActive(false);

        headCanvas.SetActive(true);
    }

    

    public void ReturntoMain()
    {
        mainCanvas.SetActive(true);
        headCanvas.SetActive(false);
        leftArmCanvas.SetActive(false);
        leftExtension.SetActive(false);
        rightArmCanvas.SetActive(false);
        setThresholdCanvas.SetActive(false);
        statusCanvas.SetActive(false);
        mainBody.SetActive(true);
        rewardCanvas.SetActive(false);
    }
}
