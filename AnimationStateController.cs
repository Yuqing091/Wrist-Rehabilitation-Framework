using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private UIManager manager;

    [SerializeField]
    private Finger finger;

    [SerializeField]
    private CinemachineVirtualCamera fullBodyCamera;

    [SerializeField]
    private CinemachineVirtualCamera HeadCamera;

    [SerializeField]
    private CinemachineVirtualCamera LeftArmCamera;

    [SerializeField]
    private CinemachineVirtualCamera LeftHandCamera;

    [SerializeField]
    private CinemachineVirtualCamera RightArmCamera;
    // Start is called before the first frame update

    [SerializeField]
    private Character character;


    public bool isLeftExtension = false;
    public bool isLeftRadial = false;
    public bool isLeftPronation = false;
    public bool isLeftHand = false;
    public static bool gameIsLeftExtension = false;
    public static bool gameIsLeftRadial = false;
    public static bool gameIsLeftPronation = false;
    public bool isHead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        fullBodyCamera.Priority = 1;
        LeftArmCamera.Priority = 0;
        RightArmCamera.Priority = 0;
        HeadCamera.Priority = 0;
        LeftHandCamera.Priority = 0;
    }

    

    public void LeftWrist()
    {
        animator.enabled = true;
        //change camera priority
        fullBodyCamera.Priority = 0;
        LeftArmCamera.Priority = 1;
        LeftHandCamera.Priority = 0;
        //Animation
        animator.SetBool("isSittingLeftArm", true);
        manager.EnterLeftArm();
        isLeftExtension = false;
        isLeftRadial = false;
        isLeftPronation = false;
        isLeftHand = false;
        character.isStart = false;
        character.resetTimer = true;
        finger.isStart = false;
        character.isBluetooth = false;


    }

    public void LeftHand()
    {
        isLeftHand = true;
        animator.enabled = false;
        LeftArmCamera.Priority = 0;
        LeftHandCamera.Priority = 1;
        manager.EnterLeftHand();
    }

    public void LeftExtension()
    {
        
        manager.EnterLeftExtension();
        animator.enabled = false;
        isLeftExtension = true; // startstreaming
        character.resetTimer = false;
        gameIsLeftRadial = false;
        gameIsLeftPronation = false;
        gameIsLeftExtension = true;
    }

    public void LeftRadial()
    {
        manager.EnterLeftRadial();
        animator.enabled = false;
        isLeftRadial = true;
        character.resetTimer = false;
        gameIsLeftExtension = false;
        gameIsLeftPronation = false;
        gameIsLeftRadial = true;
    }

    public void LeftPronation()
    {
        manager.EnterLeftPronation();
        animator.enabled = false;
        isLeftPronation = true;
        character.resetTimer = false;
        gameIsLeftExtension = false;
        gameIsLeftRadial = false;
        gameIsLeftPronation = true;
    }


    public void RightWrist()
    {
        animator.enabled = true;

        fullBodyCamera.Priority = 0;
        RightArmCamera.Priority = 1;
        animator.SetBool("isSittingRightArm", true);
        manager.EnterRightArm();
    }

    public void Head()
    {
        fullBodyCamera.Priority = 0;
        HeadCamera.Priority = 1;
        animator.enabled = false;
        manager.EnterHead();
        isHead = true;
    }

    public void MainBody()
    {
        //change camera priority
        animator.enabled = true;
        fullBodyCamera.Priority = 1;
        LeftArmCamera.Priority = 0;
        RightArmCamera.Priority = 0;
        animator.SetBool("isSittingLeftArm", false);
        animator.SetBool("isSittingRightArm", false);
        manager.ReturntoMain();
        isLeftExtension = false;
        isHead = false;
        
    }

}
