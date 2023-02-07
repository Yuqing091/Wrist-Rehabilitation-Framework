using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField]
    private Toggle extensionToggle;
    [SerializeField]
    private Toggle radialToggle;
    [SerializeField]
    private Toggle pronationToggle;

    void Start()
    {
        extensionToggle.isOn = true;
        radialToggle.isOn = false;
        pronationToggle.isOn = false;
    }

    void Update()
    {
        if (extensionToggle.isOn == true)
        {
            /*radialToggle.isOn = false;
            pronationToggle.isOn = false;*/
        }

        if(radialToggle.isOn == true)
        {
            /*extensionToggle.isOn = false;
            pronationToggle.isOn = false;*/
        }

        if (pronationToggle.isOn == true)
        {
            /*radialToggle.isOn = false;
            extensionToggle.isOn = false;*/
        }
    }

    public void ExtensionToggleOn()
    {
        extensionToggle.isOn = true;
        radialToggle.isOn = false;
        pronationToggle.isOn = false;
    }

    public void RadialToggleOn()
    {
        extensionToggle.isOn = false;
        radialToggle.isOn = true;
        pronationToggle.isOn = false;
    }

    public void PronationToggleOn()
    {
        extensionToggle.isOn = false;
        radialToggle.isOn = false;
        pronationToggle.isOn = true;
    }
}   
