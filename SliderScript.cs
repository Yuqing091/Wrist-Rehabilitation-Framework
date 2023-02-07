using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    //Base Threshold Sliders
    [SerializeField]
    private Slider extensionSlider;
    [SerializeField]
    private Text extensionSliderText;
    [SerializeField]
    private Slider flexionSlider;
    [SerializeField]
    private Text flexionSliderText;

    [SerializeField]
    private Slider pronationSlider;
    [SerializeField]
    private Text pronationSliderText;
    [SerializeField]
    private Slider supinationSlider;
    [SerializeField]
    private Text supinationSliderText;


    [SerializeField]
    private Slider radialSlider;
    [SerializeField]
    private Text radialSliderText;
    [SerializeField]
    private Slider ulnarSlider;
    [SerializeField]
    private Text ulnarSliderText;


    //Select repetitions and holdtime
    [SerializeField]
    private InputField repetitionExtensionInput;
    [SerializeField]
    private InputField holdTimeExtentionInput;

    //Radial/Ulnar sliders
    [SerializeField]
    private InputField repetitionRadialInput;
    [SerializeField]
    private InputField holdTimeRadialInput;

    //Radial/Ulnar sliders
    [SerializeField]
    private InputField repetitionPronationInput;
    [SerializeField]
    private InputField holdTimePronationInput;

    //Fingersliders
    [SerializeField]
    private InputField repetitionFingerInput;

    [SerializeField]
    private FileReader fr;

    float savedPronationAngle;

    float savedSupinationAngle;

    float savedRadialAngle;

    float savedUlnarAngle;

    float savedExtensionAngle;

    float savedFlexionAngle;

    public float extensionBaseAngle = 0.0f;
    public static float gameExtensionBaseAngle;
    public float flexionBaseAngle = 0.0f;
    public static float gameFlexionBaseAngle;
    public float radialBaseAngle = 0.0f;
    public static float gameRadialBaseAngle;
    public float ulnarBaseAngle = 0.0f;
    public static float gameUlnarBaseAngle;
    public float pronationBaseAngle = 0.0f;
    public static float gamePronationBaseAngle;
    public float supinationBaseAngle = 0.0f;
    public static float gameSupinationBaseAngle;

    public int numReps;
    public static int gameNumRepExtension;
    public int holdTime;
    public static int gameHoldTimeExtension;
    public static int gameNumRepRadial;
    public static int gameHoldTimeRadial;
    public static int gameNumRepPronation;
    public static int gameHoldTimePronation;

    public int numRepsRadial;
    public int holdTimeRadial;
    public int numRepsPronation;
    public int holdTimePronation;
    public int numRepsFinger;

    void Start()
    {
        LoadValues();

        pronationSlider.onValueChanged.AddListener((a) =>
        {
            pronationSliderText.text = a.ToString("0.0");
            //pronationBaseAngle = a;
        });

        supinationSlider.onValueChanged.AddListener((b) =>
        {
            supinationSliderText.text = b.ToString("0.0");
            supinationBaseAngle = b;
        });

        radialSlider.onValueChanged.AddListener((c) =>
        {
            radialSliderText.text = c.ToString("0.0");
            radialBaseAngle = c;
        });

        ulnarSlider.onValueChanged.AddListener((d) =>
        {
            ulnarSliderText.text = d.ToString("0.0");
            ulnarBaseAngle = d;
        });

        extensionSlider.onValueChanged.AddListener((e) =>
        {
            extensionSliderText.text = e.ToString("0.0");
            extensionBaseAngle = e;
        });

        flexionSlider.onValueChanged.AddListener((f) =>
        {
            flexionSliderText.text = f.ToString("0.0");
            flexionBaseAngle = f;
        });


    }

    public void GetExtensionRepetitionInput(string input)
    {
        numReps = int.Parse(input);
        gameNumRepExtension = numReps;
        Debug.Log("the input is: " + input);

    }

    public void GetExtensionHoldTimeInput(string input)
    {
        holdTime = int.Parse(input);
        gameHoldTimeExtension = holdTime;
        Debug.Log("the hold time is: " + holdTime );

    }

    public void GetRadialRepetitionInput(string input)
    {
        numRepsRadial = int.Parse(input);
        gameNumRepRadial = numRepsRadial;
        Debug.Log("the input is: " + input);

    }

    public void GetRadialHoldTimeInput(string input)
    {
        holdTimeRadial = int.Parse(input);
        gameHoldTimeRadial = holdTimeRadial;
        Debug.Log("the hold time is: " + input);

    }

    public void GetPronationRepetitionInput(string input)
    {
        numRepsPronation = int.Parse(input);
        gameNumRepPronation = numRepsPronation;
        Debug.Log("the input is: " + input);

    }

    public void GetPronationHoldTimeInput(string input)
    {
        holdTimePronation = int.Parse(input);
        gameHoldTimePronation = holdTimePronation;
        Debug.Log("the hold time is: " + input);

    }

    public void GetFingerRepetitionInput(string input)
    {
        numRepsFinger = int.Parse(input);
        Debug.Log("the input is: " + input);

    }

    public void SaveSettingButton()
    {
        savedPronationAngle = pronationSlider.value;
        PlayerPrefs.SetFloat("SavedPronationAngle", savedPronationAngle);

        savedSupinationAngle = supinationSlider.value;
        PlayerPrefs.SetFloat("SavedSupinationAngle", savedSupinationAngle);

        savedRadialAngle = radialSlider.value;
        PlayerPrefs.SetFloat("SavedRadialAngle", savedRadialAngle);

        savedUlnarAngle = ulnarSlider.value;
        PlayerPrefs.SetFloat("SavedUlnarAngle", savedUlnarAngle);

        savedExtensionAngle = extensionSlider.value;
        PlayerPrefs.SetFloat("SavedExtensionAngle", savedExtensionAngle);

        savedFlexionAngle = flexionSlider.value;
        PlayerPrefs.SetFloat("SavedFlexionAngle", savedFlexionAngle);

        LoadValues();
    }

    public void LoadFromFile()
    {
        savedExtensionAngle = fr.loadedExtensionAngle;
        PlayerPrefs.SetFloat("SavedExtensionAngle", savedExtensionAngle);

        savedFlexionAngle = -fr.loadedFlexionAngle;
        PlayerPrefs.SetFloat("SavedFlexionAngle", savedFlexionAngle);

        savedRadialAngle = fr.loadedRadialAngle;
        PlayerPrefs.SetFloat("SavedRadialAngle", savedRadialAngle);

        savedUlnarAngle = -fr.loadedUlnarAngle;
        PlayerPrefs.SetFloat("SavedUlnarAngle", savedUlnarAngle);

        savedPronationAngle = fr.loadedPronationAngle;
        PlayerPrefs.SetFloat("SavedPronationAngle", savedPronationAngle);

        savedSupinationAngle = -fr.loadedSupinationAngle;
        PlayerPrefs.SetFloat("SavedSupinationAngle", savedSupinationAngle);

        LoadValues();
    }

    void LoadValues()
    {
        float savedPronationAngle = PlayerPrefs.GetFloat("SavedPronationAngle");
        pronationSlider.value = savedPronationAngle;
        pronationSliderText.text = savedPronationAngle.ToString("0.0");
        pronationBaseAngle = pronationSlider.value;

        float savedSupinationAngle = PlayerPrefs.GetFloat("SavedSupinationAngle");
        supinationSlider.value = savedSupinationAngle;
        supinationSliderText.text = savedSupinationAngle.ToString("0.0");
        supinationBaseAngle = supinationSlider.value;

        float savedRadialAngle = PlayerPrefs.GetFloat("SavedRadialAngle");
        radialSlider.value = savedRadialAngle;
        radialSliderText.text = savedRadialAngle.ToString("0.0");
        radialBaseAngle = radialSlider.value;

        float savedUlnarAngle = PlayerPrefs.GetFloat("SavedUlnarAngle");
        ulnarSlider.value = savedUlnarAngle;
        ulnarSliderText.text = savedUlnarAngle.ToString("0.0");
        ulnarBaseAngle = ulnarSlider.value;

        float savedExtensionAngle = PlayerPrefs.GetFloat("SavedExtensionAngle");
        extensionSlider.value = savedExtensionAngle;
        extensionSliderText.text = savedExtensionAngle.ToString("0.0");
        extensionBaseAngle = extensionSlider.value;

        float savedFlexionAngle = PlayerPrefs.GetFloat("SavedFlexionAngle");
        flexionSlider.value = savedFlexionAngle;
        flexionSliderText.text = savedFlexionAngle.ToString("0.0");
        flexionBaseAngle = flexionSlider.value;

        gameExtensionBaseAngle = savedExtensionAngle;
        gameFlexionBaseAngle = savedFlexionAngle;
        gameRadialBaseAngle = savedRadialAngle;
        gameUlnarBaseAngle = savedUlnarAngle;
        gamePronationBaseAngle = savedPronationAngle;
        gameSupinationBaseAngle = savedSupinationAngle;
        
    }
}
