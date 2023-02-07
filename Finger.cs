using System;
using System.Threading;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class Finger : MonoBehaviour
{

    [SerializeField]
    private AnimationStateController stateController;
    [SerializeField]
    private GameObject stopExerciseButton;
    [SerializeField]
    private GameObject startExerciseButton;
    [SerializeField]
    private SliderScript slider;
    [SerializeField]
    private Text currentExerciseText;
    [SerializeField]
    private GameObject leftIndex;
    [SerializeField]
    private GameObject leftIndex2;
    [SerializeField]
    private GameObject leftIndex3;
    [SerializeField]
    private GameObject leftMiddle;
    [SerializeField]
    private GameObject leftMiddle2;
    [SerializeField]
    private GameObject leftMiddle3;
    [SerializeField]
    private GameObject leftPinky;
    [SerializeField]
    private GameObject leftPinky2;
    [SerializeField]
    private GameObject leftPinky3;
    [SerializeField]
    private GameObject leftRing;
    [SerializeField]
    private GameObject leftRing2;
    [SerializeField]
    private GameObject leftRing3;
    [SerializeField]
    private GameObject leftThumb;
    [SerializeField]
    private GameObject leftThumb2;
    [SerializeField]
    private GameObject leftThumb3;

    //for daily goal
    public int currentFinger;

    public bool isStart = false;
    // TCP Bluetooth Object
    public static Server _bluetoothobj;

    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];
    private float leftIndexAngle;
    private float leftMiddleAngle;
    private float leftPinkyAngle;
    private float leftRingAngle;
    private bool isStartExercise = false;
    private bool isIndex = false;
    private bool isMiddle = false;
    private bool isPinky = false;
    private bool isRing = false;
    private int reps;

    private static bool hasRun = false;

    private char[] _delimiter = {'R','r','o','l','P','p','i','t','c','h','a','w','Y','x','y','z',',', ':', '{', '}', '[', ']', '\"', ' ', '|' };


    private void Start()
    {

        if (!hasRun)
        {
            hasRun = true;
            _bluetoothobj = new Server();
        }
        
        Debug.Log("Starting New Server");
        stopExerciseButton.SetActive(false);
        startExerciseButton.SetActive(true);
        currentFinger = 0;


    }
    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            
            _lineread1 = _bluetoothobj.GetSensor1();


            _splitter1 = _lineread1.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _splitter1.Length; i++)
            {
                storeSplitter1[i] = _splitter1[i];
                

            }
            
            //sensor 1 data
            leftIndexAngle = float.Parse(storeSplitter1[0]);
            leftMiddleAngle = float.Parse(storeSplitter1[1]);
            leftRingAngle = float.Parse(storeSplitter1[2]);
            leftPinkyAngle = float.Parse(storeSplitter1[3]);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopBluetooth();
            }

            if (leftIndexAngle < 20)
            {
                leftThumb.transform.localRotation = Quaternion.Euler(25.467f, -13.145f, -29.084f);
                leftThumb.transform.localPosition = new Vector3(0.028677f, 0.03935843f, 0.01188317f);
                leftThumb2.transform.localRotation = Quaternion.Euler(3.036f, 5.026f, 22.882f);
                leftThumb2.transform.localPosition = new Vector3(0.004054086f, 0.03989584f, 1.862643e-09f);
                leftThumb3.transform.localRotation = Quaternion.Euler(-0.867f, 4.81f, -8.245f);
                leftThumb3.transform.localPosition = new Vector3(0.0003730685f, 0.0359991f, 4.284078e-08f);

                leftIndex.transform.localRotation = Quaternion.Euler(57.577f, -14.412f, -13.331f);
                leftIndex.transform.localPosition = new Vector3(0.0348425f, 0.1175789f, 0.001130867f);
                leftIndex2.transform.localRotation = Quaternion.Euler(22.07f, -7.16f, -24.476f);
                leftIndex2.transform.localPosition = new Vector3(-0.0002311451f, 0.03298396f, 5.820761e-09f);
                leftIndex3.transform.localRotation = Quaternion.Euler(57.938f, 32.54f, 19.399f);
                leftIndex3.transform.localPosition = new Vector3(6.815176e-05f, 0.03081052f, 3.44589e-08f);
            }
            else if (leftMiddleAngle < 20)
            {
                leftThumb.transform.localRotation = Quaternion.Euler(25.467f, -13.145f, -29.084f);
                leftThumb.transform.localPosition = new Vector3(0.028677f, 0.03935843f, 0.01188317f);
                leftThumb2.transform.localRotation = Quaternion.Euler(3.036f, 5.026f, 22.882f);
                leftThumb2.transform.localPosition = new Vector3(0.004054086f, 0.03989584f, 1.862643e-09f);
                leftThumb3.transform.localRotation = Quaternion.Euler(-0.867f, 4.81f, -8.245f);
                leftThumb3.transform.localPosition = new Vector3(0.0003730685f, 0.0359991f, 4.284078e-08f);

                leftMiddle.transform.localRotation = Quaternion.Euler(56.116f, -5.815f, -2.969f);
                leftMiddle.transform.localPosition = new Vector3(0.01029283f, 0.1234713f, -0.003158068f);
                leftMiddle2.transform.localRotation = Quaternion.Euler(40.652f, -6.493f, -41.89f);
                leftMiddle2.transform.localPosition = new Vector3(-9.939037e-05f, 0.03432157f, -3.352757e-08f);
                leftMiddle3.transform.localRotation = Quaternion.Euler(20.02f, 2.234f, -2.253f);
                leftMiddle3.transform.localPosition = new Vector3(9.976031e-05f, 0.0316092f, -4.004681e-08f);
            }
            else if (leftRingAngle < 20)
            {
                leftThumb.transform.localRotation = Quaternion.Euler(23.19f, -13.398f, -40.915f);
                leftThumb.transform.localPosition = new Vector3(0.028677f, 0.03935843f, 0.01188317f);
                leftThumb2.transform.localRotation = Quaternion.Euler(3.413f, 10.33f, 51.371f);
                leftThumb2.transform.localPosition = new Vector3(0.004054086f, 0.03989584f, 1.862643e-09f);
                leftThumb3.transform.localRotation = Quaternion.Euler(-0.867f, 4.81f, -8.245f);
                leftThumb3.transform.localPosition = new Vector3(0.0003730685f, 0.0359991f, 4.284078e-08f);

                leftRing.transform.localRotation = Quaternion.Euler(77.262f, 42.549f, 9.203f);
                leftRing.transform.localPosition = new Vector3(-0.01266741f, 0.1250595f, -0.002770633f);
                leftRing2.transform.localRotation = Quaternion.Euler(20.74f, 3.258f, -1.918f);
                leftRing2.transform.localPosition = new Vector3(6.585555e-05f, 0.02726052f, -3.003511e-08f);
                leftRing3.transform.localRotation = Quaternion.Euler(18.061f, 3.516f, -0.86f);
                leftRing3.transform.localPosition = new Vector3(-3.413593e-05f, 0.02620304f, -1.396982e-08f);
            }
            else if (leftPinkyAngle < 20)
            {
                leftThumb.transform.localRotation = Quaternion.Euler(26.998f, -24.918f, -9.077f);
                leftThumb.transform.localPosition = new Vector3(0.028677f, 0.03935843f, 0.01188317f);
                leftThumb2.transform.localRotation = Quaternion.Euler(3.036f, 5.026f, 22.882f);
                leftThumb2.transform.localPosition = new Vector3(0.004054086f, 0.03989584f, 1.862643e-09f);
                leftThumb3.transform.localRotation = Quaternion.Euler(-0.867f, 4.81f, -8.245f);
                leftThumb3.transform.localPosition = new Vector3(0.0003730685f, 0.0359991f, 4.284078e-08f);

                leftPinky.transform.localRotation = Quaternion.Euler(46.147f, 72.669f, 44.444f);
                leftPinky.transform.localPosition = new Vector3(-0.03246783f, 0.1110198f, -0.001128237f);
                leftPinky2.transform.localRotation = Quaternion.Euler(15.105f, 1.972f, -0.409f);
                leftPinky2.transform.localPosition = new Vector3(-8.419491e-05f, 0.027346f, 5.84404e-08f);
                leftPinky3.transform.localRotation = Quaternion.Euler(23.788f, 1.589f, -2.018f);
                leftPinky3.transform.localPosition = new Vector3(2.90715e-05f, 0.02193952f, -1.187434e-08f);
            }
            else
            {
                leftThumb.transform.localRotation = Quaternion.Euler(26.593f, -12.509f, -32.257f);
                leftThumb.transform.localPosition = new Vector3(0.028677f, 0.03935843f, 0.01188317f);
                leftThumb2.transform.localRotation = Quaternion.Euler(3.036f, 5.026f, 22.882f);
                leftThumb2.transform.localPosition = new Vector3(0.004054086f, 0.03989584f, 1.862643e-09f);
                leftThumb3.transform.localRotation = Quaternion.Euler(-0.867f, 4.81f, -8.245f);
                leftThumb3.transform.localPosition = new Vector3(0.0003730685f, 0.0359991f, 4.284078e-08f);

                leftIndex.transform.localRotation = Quaternion.Euler(53.419f, -7.32f, -2.899f);
                leftIndex.transform.localPosition = new Vector3(0.0348425f, 0.1175789f, 0.001130867f);
                leftIndex2.transform.localRotation = Quaternion.Euler(30.973f, 7.084f, -0.808f);
                leftIndex2.transform.localPosition = new Vector3(-0.0002311451f, 0.03298396f, 5.820761e-09f);
                leftIndex3.transform.localRotation = Quaternion.Euler(12.41f, 6.896f, 0.833f);
                leftIndex3.transform.localPosition = new Vector3(6.815176e-05f, 0.03081052f, 3.44589e-08f);

                leftMiddle.transform.localRotation = Quaternion.Euler(42.926f, -0.889f, -0.383f);
                leftMiddle.transform.localPosition = new Vector3(0.01029283f, 0.1234713f, -0.003158068f);
                leftMiddle2.transform.localRotation = Quaternion.Euler(30.333f, 2.103f, -2.399f);
                leftMiddle2.transform.localPosition = new Vector3(-9.939037e-05f, 0.03432157f, -3.352757e-08f);
                leftMiddle3.transform.localRotation = Quaternion.Euler(20.02f, 2.234f, -2.253f);
                leftMiddle3.transform.localPosition = new Vector3(9.976031e-05f, 0.0316092f, -4.004681e-08f);

                leftRing.transform.localRotation = Quaternion.Euler(36.057f, 4.693f, 7.425f);
                leftRing.transform.localPosition = new Vector3(-0.01266741f, 0.1250595f, -0.002770633f);
                leftRing2.transform.localRotation = Quaternion.Euler(20.74f, 3.258f, -1.918f);
                leftRing2.transform.localPosition = new Vector3(6.585555e-05f, 0.02726052f, -3.003511e-08f);
                leftRing3.transform.localRotation = Quaternion.Euler(18.061f, 3.516f, -0.86f);
                leftRing3.transform.localPosition = new Vector3(-3.413593e-05f, 0.02620304f, -1.396982e-08f);


                leftPinky.transform.localRotation = Quaternion.Euler(25.574f, 5.579f, 6.207f);
                leftPinky.transform.localPosition = new Vector3(-0.03246783f, 0.1110198f, -0.001128237f);
                leftPinky2.transform.localRotation = Quaternion.Euler(15.105f, 1.972f, -0.409f);
                leftPinky2.transform.localPosition = new Vector3(-8.419491e-05f, 0.027346f, 5.84404e-08f);
                leftPinky3.transform.localRotation = Quaternion.Euler(23.788f, 1.589f, -2.018f);
                leftPinky3.transform.localPosition = new Vector3(2.90715e-05f, 0.02193952f, -1.187434e-08f);
            }

            if (isStartExercise)
            {
                StartExercise();
            }
        }
    }

    public void Initialise()
    {
        Debug.Log("Created");
        _bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        // Wait to ensure bluetooth communication begins before calibration
        Thread.Sleep(200);
        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }

        /*leftIndex.transform.localRotation = Quaternion.Euler(15.024f, 4.143f, 1.494f);
        leftMiddle.transform.localRotation = Quaternion.Euler(17.745f, 5.219f, 0.504f);
        leftPinky.transform.localRotation = Quaternion.Euler(42.552f, 1.756f, 6.117f);
        leftRing.transform.localRotation = Quaternion.Euler(36.057f, 4.693f, 7.425f);
        leftIndex.transform.localPosition = new Vector3(0.03484f, 0.117578f, 0.001136f);
        leftMiddle.transform.localPosition = new Vector3(0.010292f, 0.12347f, -0.00315f);
        leftPinky.transform.localPosition = new Vector3(-0.003246f, 0.111019f, -0.00112f);
        leftRing.transform.localPosition = new Vector3(-0.01266f, 0.125059f, -0.00277f);*/
        isStart = true;
        
    }

    public void IsStart()
    {
        reps = slider.numRepsFinger;
        isStartExercise = true;
        stopExerciseButton.SetActive(true);
        startExerciseButton.SetActive(false);
    }

    public void IsStop()
    {
        stopExerciseButton.SetActive(false);
        startExerciseButton.SetActive(true);
        isStartExercise = false;
        currentExerciseText.text = "Exercise Stopped";
        isIndex = true;
        isMiddle = false;
        isRing = false;
        isPinky = false;
    }

    public void StartExercise()
    {
        
        isIndex = true;
        if (reps > 0)
        {
            


            if (isIndex)
            {
                currentExerciseText.text = "Touch Index Finger " + reps + " reps to go.";
                if (leftIndexAngle < 8)
                {

                    isIndex = false;
                    isMiddle = true;
                }
            }
            if (isMiddle)
            {
                currentExerciseText.text = "Touch Middle Finger";
                if (leftMiddleAngle < 8)
                {
                    isMiddle = false;
                    isRing = true;
                }
            }

            if (isRing)
            {
                currentExerciseText.text = "Touch Ring Finger";
                if (leftRingAngle < 8)
                {
                    isRing = false;
                    isPinky = true;
                }
            }

            if (isPinky)
            {
                currentExerciseText.text = "Touch Pinky";
                if (leftPinkyAngle < 8)
                {
                    isPinky = false;
                    isIndex = true;
                    reps--;
                }
            }
        }
        else
        {
            currentExerciseText.text = "Exercise Done";
            isStartExercise = false;
            stopExerciseButton.SetActive(false);
            startExerciseButton.SetActive(true);
            currentFinger++;
        }
    }
    public void StopBluetooth()
    {
        _bluetoothobj.Stop();
        Debug.Log("Stopped bluetooth");
    }
}