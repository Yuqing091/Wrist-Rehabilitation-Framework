using UnityEngine;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{
    //public GameObject storageArea;

    public GameObject reward;
    public float popupDuration = 2.0f;
    [SerializeField]
    private Animator animator;

    private bool rewardShown = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void ShowReward()
    {
        if (!rewardShown)
        {
            animator.Play("RewardsPopUp");
            rewardShown = true;
            Invoke("HideReward", popupDuration);
        }
    }

    private void HideReward()
    {
        reward.SetActive(false);
        //storageArea.SetActive(true);
    }
}
