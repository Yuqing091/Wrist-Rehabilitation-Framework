using System;
using System.Threading;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using System.IO;

public class Character : MonoBehaviour
{

    [SerializeField]
    private AnimationStateController stateController;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject leftForeArm;
    [SerializeField]
    private SliderScript sliderScript;

    [SerializeField]
    private GameObject startExerciseExtensionButton;
    [SerializeField]
    private GameObject stopExerciseExtensionButton;
    [SerializeField]
    private GameObject startExerciseRadialButton;
    [SerializeField]
    private GameObject stopExerciseRadialButton;
    [SerializeField]
    private GameObject startExercisePronationButton;
    [SerializeField]
    private GameObject stopExercisePronationButton;

    [SerializeField]
    private Text highestExtensionText;
    [SerializeField]
    private Text highestFlexionText;
    [SerializeField]
    private Text highestRadialText;
    [SerializeField]
    private Text highestUlnarText;
    [SerializeField]
    private Text highestPronationText;
    [SerializeField]
    private Text highestSupinationText;

    //get patient name
    private string patientName;

    //Current progress for goals
    public int currentExtension;
    public int currentRadial;
    public int currentPronation;

    //Saved angles
    float savedExtension;
    float savedFlexion;
    float savedRadial;
    float savedUlnar;
    float savedPronation;
    float savedSupination;

    //Recording data to csv
    private FileStream streamFile;
    private StreamWriter writeStream;
    private string timeStamp;
    private string timeStampPrint;
    private DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    private DateTime dateTimeNow;
    private TimeSpan timeElapsed;
    private double timeElapsedInseconds;


    private bool isExtension = true;
    private bool isRadial = true;
    private bool isPronation = true;
    public bool isStart = false;
    public bool resetTimer = false;
    private int reps = 1;
    public bool isBluetooth = false;

    //store current angle temporarily.
    private float tempXAngle = 0.0f;
    private float tempYAngle = 0.0f;
    private float tempZAngle = 0.0f;
    private float tempXAngleLF = 0.0f;
    private float tempYAngleLF = 0.0f;
    private float tempZAngleLF = 0.0f;

    //actual angles of head
    private float actualXAngleH;
    private float actualYAngleH;
    private float actualZAngleH;
    //left wrist joint angle
    private float leftJointAngle = 0.0f;
    private float leftRadialAngle = 0.0f;
    private float leftPronationAngle = 0.0f;
    //Left wrist angles
    private float actualXAngle;
    private float actualYAngle;
    private float actualZAngle;

    //left forearm angles
    private float actualXAngleLF;
    private float actualYAngleLF;
    private float actualZAngleLF;

    //Display Joint angle
    [SerializeField]
    private Text jointAngle;
    [SerializeField]
    private Text RadialAngle;
    [SerializeField]
    private Text PronationAngle;
    [SerializeField]
    private Text countDownTimer;
    [SerializeField]
    private Text countDownRadialTimer;
    [SerializeField]
    private Text countDownPronationTimer;

    //Timer
    private float currentTime = 0.0f;
    private float storeCurrentTime = 0.0f;
    private float currentRadialTime = 0.0f;
    private float currentPronationTime = 0.0f;

    //IMUdata setup receiving
    // This has changed so it can receive two inputs, one for each IMU via Bluetooth
    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    public Quaternion[] quat = new Quaternion[5];
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };

    [SerializeField]
    private Finger finger;

    void Start()
    {
        //for daily goals
        currentExtension = 0;
        currentRadial = 0;
        currentPronation = 0;

        stopExerciseExtensionButton.SetActive(false);
        stopExerciseRadialButton.SetActive(false);
        stopExercisePronationButton.SetActive(false);
        
        LoadHighestAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBluetooth == true)
        {   //IMUdata
            // Receive from Bluetooth, each string it is assigned to is for each bluetooth 
            _lineread1 = Finger._bluetoothobj.GetSensor1();

            _splitter1 = _lineread1.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _splitter1.Length; i++)
            {
                storeSplitter1[i] = _splitter1[i];
            }

            //joint angle display
            jointAngle.text = "Joint Angle: " + leftJointAngle.ToString();
            RadialAngle.text = "Joint Angle: " + leftRadialAngle.ToString();
            PronationAngle.text = "Joint Angle: " + leftPronationAngle.ToString();

            //sensor 1 data
            euler.x = float.Parse(storeSplitter1[0]);
            euler.y = float.Parse(storeSplitter1[1]);
            euler.z = float.Parse(storeSplitter1[2]);



            //sensor 2 data
            euler2.x = float.Parse(storeSplitter1[3]);
            euler2.y = float.Parse(storeSplitter1[4]);
            euler2.z = float.Parse(storeSplitter1[5]);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopBluetooth();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //sensor 1 left wrist
                tempXAngle = euler.x;
                tempYAngle = euler.y;
                tempZAngle = euler.z;


                //sensor 2 left forearm
                tempXAngleLF = euler2.x;
                tempYAngleLF = euler2.y;
                tempZAngleLF = euler2.z;
            }

            //Head position
            actualXAngleH = (euler.x - tempXAngle);
            actualYAngleH = (euler.y - tempYAngle);
            actualZAngleH = (euler.z - tempZAngle);

            //Debug.Log("x: " + actualXAngleH + "y: " + actualYAngleH + "z: " + actualZAngleH);
            //left wrist postion
            actualXAngle = (euler.x - tempXAngle + 34.78f);
            actualYAngle = (euler.y - tempYAngle + 3.994f);
            actualZAngle = (euler.z - tempZAngle - 2.884f);

            //left forearm position
            actualXAngleLF = (euler2.x - tempXAngleLF - 16.145f);
            actualYAngleLF = (euler2.y - tempYAngleLF - 1.15f);
            actualZAngleLF = (euler2.z - tempZAngleLF - 15.16f);

            //Left wrist joint angle
            leftJointAngle = -(actualYAngleH);
            leftRadialAngle = -(actualZAngleH);
            leftPronationAngle = actualXAngleH;

            //covert to quaternion
            ConvertEulerToQuaternion(euler.x, euler.y, euler.z);
            //Debug.Log("x:" + quat[0].x + "y:" + quat[0].y + "z:" + quat[0].z + "w:" + quat[0].w);


            if (stateController.isHead)
            {
                head.transform.localRotation = Quaternion.Euler(actualYAngleH, -actualZAngleH, -actualXAngleH);
            }

            //Extension/Flexion
            if (stateController.isLeftExtension || stateController.isLeftRadial || stateController.isLeftPronation)
            {
                EnableControl();
                if (isStart && stateController.isLeftExtension)
                {
                    LeftWristExtension();
                }
                else if (isStart && stateController.isLeftRadial)
                {
                    LeftWristRadial();
                }
                else if (isStart && stateController.isLeftPronation)
                {
                    LeftWristPronation();
                }
                else
                {
                    isPronation = true;
                    isRadial = true;
                    isExtension = true;//set first exercise to be extension when button not pressed.

                }



            }

            if (resetTimer)
            {
                countDownTimer.text = "Timer";
                countDownRadialTimer.text = "Timer";
                countDownPronationTimer.text = "Timer";
            }

            //Record into CSV file
            setTimeStamp();
            if (isStart)
            {
                dateTimeNow = DateTime.Now;

                timeElapsed = dateTimeNow.Subtract(dateTime);

                timeElapsedInseconds = Convert.ToDouble(timeElapsed.TotalSeconds);

                writeStream.WriteLine(",,,," + timeElapsedInseconds.ToString() + "," + leftPronationAngle.ToString() + "," + leftJointAngle.ToString()
                                    + "," + leftRadialAngle.ToString()+","+ quat[0].x.ToString() + "," + quat[0].y.ToString() + "," + quat[0].z.ToString() + "," + quat[0].w.ToString());

                writeStream.Flush();
            }

        }
        //Debug.Log("Testing: "+ PlayerPrefs.GetFloat("HighestFlexion"));
    }

    public void StartBluetooth()
    {
        //IMUdata
        // Start receiving from bluetooth of both sensors instead of IP 
        Debug.Log("Starting");
      
        Finger._bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");
        // _bluetoothobj.Calibrate();

        // Wait to ensure bluetooth communication begins before calibration
        //Thread.Sleep(300);

        //set array element to "0"
        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }

        //Initializes hold time;
        currentTime = sliderScript.holdTime;
        storeCurrentTime = currentTime;
        currentRadialTime = sliderScript.holdTimeRadial;
        currentPronationTime = sliderScript.holdTimePronation;
        isBluetooth = true;
        

    }

    public void LeftWristExtension()
    {
        

        if (reps > 0)
        {
            if (isExtension)
            {
                countDownTimer.text = "Extension";
                if (leftJointAngle >= sliderScript.extensionBaseAngle && leftJointAngle <= sliderScript.extensionBaseAngle + 2)
                {
                    currentTime -= 1 * Time.deltaTime;
                    countDownTimer.text = "Time Remaining: " + currentTime.ToString("0");
                    if (currentTime <= 0)
                    {
                        isExtension = !isExtension;
                    }
                }
                else if (leftJointAngle > sliderScript.extensionBaseAngle + 2)
                {
                    countDownTimer.text = "OUCH!!";
                    currentTime = sliderScript.holdTime;
                }
                else
                {
                    countDownTimer.text = "Wrist Extension";
                    currentTime = sliderScript.holdTime;
                }
            }
            if (isExtension == false)
            {
                countDownTimer.text = "Flexion";
                if (leftJointAngle >= sliderScript.flexionBaseAngle - 2 && leftJointAngle <= sliderScript.flexionBaseAngle)
                {
                    currentTime -= 1 * Time.deltaTime;
                    countDownTimer.text = "Time Remaining: " + currentTime.ToString("0");
                    if (currentTime <= 0)
                    {
                        isExtension = !isExtension;
                        reps--;
                    }
                }
                else if (leftJointAngle < sliderScript.flexionBaseAngle - 2)
                {
                    countDownTimer.text = "OUCH!!";
                    currentTime = sliderScript.holdTime;
                }
                else
                {
                    countDownTimer.text = "Wrist Flexion";
                    currentTime = sliderScript.holdTime;
                }


            }

        }
        if (reps <= 0)
        {
            //saving the highest angle
            //Extension
            savedExtension = sliderScript.extensionBaseAngle;
            if(savedExtension > PlayerPrefs.GetFloat("SavedExtension"))
            {
                PlayerPrefs.SetFloat("SavedExtension", savedExtension);
            }
            highestExtensionText.text = "Highest Extension: "+ PlayerPrefs.GetFloat("SavedExtension").ToString("0.0");
            //Flexion
            savedFlexion = sliderScript.flexionBaseAngle;
            if (savedFlexion < PlayerPrefs.GetFloat("SavedFlexion"))
            {
                PlayerPrefs.SetFloat("SavedFlexion", savedFlexion); 
            }
            highestFlexionText.text = "Highest Flexion: " + PlayerPrefs.GetFloat("SavedFlexion").ToString("0.0");

            countDownTimer.text = "Exercise done";
            isStart = false;
            startExerciseExtensionButton.SetActive(true);
            stopExerciseExtensionButton.SetActive(false);
            currentExtension++;
        }
    }


    public void LeftWristRadial()
    {

        if (reps > 0)
        {
            if (isRadial)
            {
                countDownRadialTimer.text = "Radial";
                if (leftRadialAngle >= sliderScript.radialBaseAngle && leftRadialAngle <= sliderScript.radialBaseAngle + 2)
                {
                    currentRadialTime -= 1 * Time.deltaTime;
                    countDownRadialTimer.text = "Time Remaining: " + currentRadialTime.ToString("0");
                    if (currentRadialTime <= 0)
                    {
                        isRadial = !isRadial;
                    }
                }
                else if (leftRadialAngle > sliderScript.radialBaseAngle + 2)
                {
                    countDownRadialTimer.text = "OUCH!!";
                    currentRadialTime = sliderScript.holdTimeRadial;
                }
                else
                {
                    countDownRadialTimer.text = "Radial";
                    currentRadialTime = sliderScript.holdTimeRadial;
                }
            }
            if (isRadial == false)
            {
                countDownRadialTimer.text = "Ulnar";
                if (leftRadialAngle >= sliderScript.ulnarBaseAngle - 2 && leftRadialAngle <= sliderScript.ulnarBaseAngle)
                {
                    currentRadialTime -= 1 * Time.deltaTime;
                    countDownRadialTimer.text = "Time Remaining: " + currentRadialTime.ToString("0");
                    if (currentRadialTime <= 0)
                    {
                        isRadial = !isRadial;
                        reps--;
                    }
                }
                else if (leftJointAngle < sliderScript.ulnarBaseAngle - 2)
                {
                    countDownRadialTimer.text = "OUCH!!";
                    currentRadialTime = sliderScript.holdTimeRadial;
                }
                else
                {
                    countDownRadialTimer.text = "Ulnar";
                    currentRadialTime = sliderScript.holdTimeRadial;
                }


            }

        }
        if (reps <= 0)
        {
            //Radial
            savedRadial = sliderScript.radialBaseAngle;
            if (savedRadial > PlayerPrefs.GetFloat("SavedRadial"))
            {
                PlayerPrefs.SetFloat("SavedRadial", savedRadial);
            }
            highestRadialText.text = "Highest Radial: " + PlayerPrefs.GetFloat("SavedRadial").ToString("0.0");
            //Flexion
            savedUlnar = sliderScript.ulnarBaseAngle;
            if (savedUlnar < PlayerPrefs.GetFloat("SavedUlnar"))
            {
                PlayerPrefs.SetFloat("SavedUlnar", savedUlnar);
            }
            highestUlnarText.text = "HighestUlnar: " + PlayerPrefs.GetFloat("SavedUlnar").ToString("0.0");

            countDownRadialTimer.text = "Exercise done";
            isStart = false;
            startExerciseRadialButton.SetActive(true);
            stopExerciseRadialButton.SetActive(false);
            currentRadial++;
        }
    }

    public void LeftWristPronation()
    {

        if (reps > 0)
        {
            if (isPronation)
            {
                countDownTimer.text = "Pronation";
                if (leftPronationAngle >= sliderScript.pronationBaseAngle && leftPronationAngle <= sliderScript.pronationBaseAngle + 2)
                {
                    currentPronationTime -= 1 * Time.deltaTime;
                    countDownPronationTimer.text = "Time Remaining: " + currentPronationTime.ToString("0");
                    if (currentPronationTime <= 0)
                    {
                        isPronation = !isPronation;
                    }
                }
                else if (leftPronationAngle > sliderScript.pronationBaseAngle + 2)
                {
                    countDownPronationTimer.text = "OUCH!!";
                    currentPronationTime = sliderScript.holdTimePronation;
                }
                else
                {
                    countDownPronationTimer.text = "Pronation";
                    currentPronationTime = sliderScript.holdTimePronation;
                }
            }
            if (isPronation == false)
            {
                countDownPronationTimer.text = "Supination";
                if (leftPronationAngle >= sliderScript.supinationBaseAngle - 2 && leftPronationAngle <= sliderScript.supinationBaseAngle)
                {
                    currentPronationTime -= 1 * Time.deltaTime;
                    countDownPronationTimer.text = "Time Remaining: " + currentPronationTime.ToString("0");
                    if (currentPronationTime <= 0)
                    {
                        isPronation = !isPronation;
                        reps--;
                    }
                }
                else if (leftPronationAngle < sliderScript.supinationBaseAngle - 2)
                {
                    countDownPronationTimer.text = "OUCH!!";
                    currentPronationTime = sliderScript.holdTimePronation;
                }
                else
                {
                    countDownPronationTimer.text = "Supination";
                    currentPronationTime = sliderScript.holdTimePronation;
                }


            }

        }
        if (reps <= 0)
        {
            //Pronation
            savedPronation = sliderScript.pronationBaseAngle;
            if (savedPronation > PlayerPrefs.GetFloat("SavedPronation"))
            {
                PlayerPrefs.SetFloat("SavedPronation", savedPronation);
            }
            highestPronationText.text = "Highest Pronation: " + PlayerPrefs.GetFloat("SavedPronation").ToString("0.0");
            //Supination
            savedSupination = sliderScript.supinationBaseAngle;
            if (savedSupination < PlayerPrefs.GetFloat("SavedSupination"))
            {
                PlayerPrefs.SetFloat("SavedSupination", savedSupination);
            }
            highestSupinationText.text = "HighestSupination: " + PlayerPrefs.GetFloat("SavedSupination").ToString("0.0");

            countDownPronationTimer.text = "Exercise done";
            isStart = false;
            startExercisePronationButton.SetActive(true);
            stopExercisePronationButton.SetActive(false);
            currentPronation++;

        }
    }

    public void LoadHighestAngle()
    {
        float savedExtensionAngle = PlayerPrefs.GetFloat("SavedExtension");
        highestExtensionText.text = "Highest Extension: " + savedExtensionAngle.ToString("0.0");
        float savedFlexionAngle = PlayerPrefs.GetFloat("SavedFlexion");
        highestFlexionText.text = "Highest Flexion: " + savedFlexionAngle.ToString("0.0");
        float savedRadialAngle = PlayerPrefs.GetFloat("SavedRadial");
        highestRadialText.text = "Highest Radial: " + savedRadialAngle.ToString("0.0");
        float savedUlnarAngle = PlayerPrefs.GetFloat("SavedUlnar");
        highestUlnarText.text = "Highest Ulnar: " + savedUlnarAngle.ToString("0.0");
        float savedPronationAngle = PlayerPrefs.GetFloat("SavedPronation");
        highestPronationText.text = "Highest Pronation: " + savedPronationAngle.ToString("0.0");
        float savedSupinationAngle = PlayerPrefs.GetFloat("SavedSupination");
        highestSupinationText.text = "Highest Supination: " + savedSupinationAngle.ToString("0.0");


    }

    public void ClearStatus()
    {
        PlayerPrefs.SetFloat("SavedExtension", 0.0f);
        highestExtensionText.text = "Highest Extension: " + PlayerPrefs.GetFloat("SavedExtension").ToString("0.0");
        PlayerPrefs.SetFloat("SavedFlexion", 0.0f);
        highestFlexionText.text = "Highest Flexion: " + PlayerPrefs.GetFloat("SavedFlexion").ToString("0.0");
        PlayerPrefs.SetFloat("SavedRadial", 0.0f);
        highestRadialText.text = "Highest Radial: " + PlayerPrefs.GetFloat("SavedRadial").ToString("0.0");
        PlayerPrefs.SetFloat("SavedUlnar", 0.0f);
        highestUlnarText.text = "Highest Ulnar: " + PlayerPrefs.GetFloat("SavedUlnar").ToString("0.0");
        PlayerPrefs.SetFloat("SavedPronation", 0.0f);
        highestPronationText.text = "Highest Pronation: " + PlayerPrefs.GetFloat("SavedPronation").ToString("0.0");
        PlayerPrefs.SetFloat("SavedSupination", 0.0f);
        highestSupinationText.text = "Highest Supination: " + PlayerPrefs.GetFloat("SavedSupination").ToString("0.0");
    }

    public void EnableControl()
    {
        leftHand.transform.localRotation = Quaternion.Euler(actualYAngle, -actualXAngle, actualZAngle);
        //leftForeArm.transform.localRotation = Quaternion.Euler(actualYAngleLF, -actualXAngleLF, actualZAngleLF);
    }
    public void StartExtensionExercise()
    {

        reps = sliderScript.numReps;
        isStart = true;
        startExerciseExtensionButton.SetActive(false);
        stopExerciseExtensionButton.SetActive(true);
    }

    public void StopExtensionExercise()
    {
        startExerciseExtensionButton.SetActive(true);
        stopExerciseExtensionButton.SetActive(false);
        isStart = false;
        countDownTimer.text = "Exercise Stopped";
    }

    public void StartRadialExercise()
    {

        reps = sliderScript.numRepsRadial;
        isStart = true;
        startExerciseRadialButton.SetActive(false);
        stopExerciseRadialButton.SetActive(true);
    }

    public void StopRadialExercise()
    {
        startExerciseRadialButton.SetActive(true);
        stopExerciseRadialButton.SetActive(false);
        isStart = false;
        countDownRadialTimer.text = "Exercise Stopped";
    }

    public void StartPronationExercise()
    {

        reps = sliderScript.numRepsPronation;
        isStart = true;
        startExercisePronationButton.SetActive(false);
        stopExercisePronationButton.SetActive(true);
    }

    public void StopPronationExercise()
    {
        startExercisePronationButton.SetActive(true);
        stopExercisePronationButton.SetActive(false);
        isStart = false;
        countDownPronationTimer.text = "Exercise Stopped";
    }

    private void setTimeStamp()
    {
        string year;
        string month;
        string date;
        string hour;
        string minute;
        string second;

        year = DateTime.Now.Year.ToString("0000");
        month = DateTime.Now.Month.ToString("00");
        date = DateTime.Now.Day.ToString("00");
        hour = DateTime.Now.Hour.ToString("00");
        minute = DateTime.Now.Minute.ToString("00");
        second = DateTime.Now.Second.ToString("00");

        timeStamp = year + "-" + month + "-" + date + "-" + hour + "-" + minute + "-" + second;

        timeStampPrint = year + "/" + month + "/" + date + ":- " + hour + ":" + minute + ":" + second;
    }

    public void GetPatientName(string input)
    {
        patientName = input;
    }
    public void CreateExtensionFile()
    {
        //
        streamFile = new FileStream("C:\\Unity Projects\\Wrist Rehabilitation Framework\\DataLog\\" + patientName + "_Extension_Flexion_" + timeStamp +  ".csv", FileMode.OpenOrCreate);
        writeStream = new StreamWriter(streamFile);

        writeStream.WriteLine("{0}", timeStampPrint);
        writeStream.WriteLine("Extension/Flexion Exercise");
        writeStream.WriteLine("Number of repetition: " + sliderScript.numReps.ToString());
        writeStream.WriteLine("Hold time: " + sliderScript.holdTime.ToString());
        writeStream.WriteLine("Base extension threshold: " + sliderScript.extensionBaseAngle.ToString());
        writeStream.WriteLine("Base flexion threshold: " + sliderScript.flexionBaseAngle.ToString());
        writeStream.WriteLine(",,,,Timestamp,X_Angle,Y_Angle,Z_Angle,qx,qy,qz,qw");
    }

    public void CreateRadialFile()
    {
        streamFile = new FileStream("C:\\Unity Projects\\Wrist Rehabilitation Framework\\DataLog\\" + patientName + "_Radial_Ulnar_" + timeStamp + ".csv", FileMode.OpenOrCreate);
        writeStream = new StreamWriter(streamFile);

        writeStream.WriteLine("{0}", timeStampPrint);
        writeStream.WriteLine("Radial/Ulnar Exercise");
        writeStream.WriteLine("Number of repetition: " + sliderScript.numRepsRadial.ToString());
        writeStream.WriteLine("Hold time: " + sliderScript.holdTimeRadial.ToString());
        writeStream.WriteLine("Base radial threshold: " + sliderScript.radialBaseAngle.ToString());
        writeStream.WriteLine("Base ulnar threshold: " + sliderScript.ulnarBaseAngle.ToString());
        writeStream.WriteLine(",,,,Timestamp,X_Angle,Y_Angle,Z_Angle,qx,qy,qz,qw");
    }

    public void CreatePronationFile()
    {
        streamFile = new FileStream("C:\\Unity Projects\\Wrist Rehabilitation Framework\\DataLog\\" + patientName + "_Pronation_Supination_" + timeStamp + ".csv", FileMode.OpenOrCreate);
        writeStream = new StreamWriter(streamFile);

        writeStream.WriteLine("{0}", timeStampPrint);
        writeStream.WriteLine("Pronation/Supination Exercise");
        writeStream.WriteLine("Number of repetition: " + sliderScript.numRepsPronation.ToString());
        writeStream.WriteLine("Hold time: " + sliderScript.holdTimePronation.ToString());
        writeStream.WriteLine("Base pronation threshold: " + sliderScript.pronationBaseAngle.ToString());
        writeStream.WriteLine("Base supination threshold: " + sliderScript.supinationBaseAngle.ToString());
        writeStream.WriteLine(",,,,Timestamp,X_Angle,Y_Angle,Z_Angle,qx,qy,qz,qw");
    }

    public void ConvertEulerToQuaternion(float roll, float pitch, float yaw)
    {

        //convert angle to radians
        float radRoll = roll * Mathf.PI / 180;
        float radPitch = pitch * Mathf.PI / 180;
        float radYaw = yaw * Mathf.PI / 180;
        // Assuming the angles are in radians.
        float c1 = Mathf.Cos(radRoll/ 2);
        float s1 = Mathf.Sin(radRoll / 2);
        float c2 = Mathf.Cos(radPitch / 2);
        float s2 = Mathf.Sin(radPitch / 2);
        float c3 = Mathf.Cos(radYaw / 2);
        float s3 = Mathf.Sin(radYaw / 2);
        float c1c2 = c1 * c2;
        float s1s2 = s1 * s2;
        quat[0].x = c1c2 * s3 + s1s2 * c3;
        quat[0].y = s1 * c2 * c3 + c1 * s2 * s3;
        quat[0].z = c1 * s2 * c3 - s1 * c2 * s3;
        quat[0].w = c1c2 * c3 - s1s2 * s3;
        
    }

    public void StopBluetooth()
    {
        Finger._bluetoothobj.Stop();
        Debug.Log("Stopped bluetooth");
    }
}
