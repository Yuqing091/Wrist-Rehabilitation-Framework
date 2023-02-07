using System;
using System.Threading;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class ChaseScript : MonoBehaviour
{
    private float speed = 5.5f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private Text highestDistanceText;
    [SerializeField]
    private Text extensionAngleText;
    [SerializeField]
    private Text holdTimeText;
    [SerializeField]
    private Text repsText;
    [SerializeField]
    private Manager UIManager;
    [SerializeField]
    private MoveEnemy enemy;
    [SerializeField]
    private Rigidbody2D myBody;

    private SpriteRenderer sr;

    private Animator anim;


    private bool isExtension = true;
    private bool isRadial = true;
    private bool isPronation = true;
    private float currentTime;
    private int initialExtensionReps;
    private int initialRadialReps;
    private int initialPronationReps;

    private float distanceTravelled = 0.0f;
    private float highestDistance;
    
    private string Walk_Animation = "Walk";


    // TCP Bluetooth Object
    private Server _bluetoothobj;


    private float jointAngleExtension = 0.0f;
    private float jointAngleRadial = 0.0f;
    private float jointAnglePronation = 0.0f;

    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //IMUdata
        // Start receiving from bluetooth of both sensors instead of IP 
        Debug.Log("Created");
        Finger._bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        // Wait to ensure bluetooth communication begins before calibration
        Thread.Sleep(300);
        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }

        highestDistanceText.text = PlayerPrefs.GetInt("highestDistance", 0).ToString();
        initialExtensionReps = SliderScript.gameNumRepExtension;
        initialRadialReps = SliderScript.gameNumRepRadial;
        initialPronationReps = SliderScript.gameNumRepPronation;
    }

    // Update is called once per frame
    void Update()
    {
        bluetoothStream();

        jointAngleExtension = -euler.x;
        jointAngleRadial = -euler.z;
        jointAnglePronation =-(euler.y - euler2.y); 
        distanceText.text = distanceTravelled.ToString("0");
        //extensionAngleText is a general text object can be used for all three exercises
        if (AnimationStateController.gameIsLeftExtension)
        {
            PlayMovementExtension();
            hand.transform.rotation = Quaternion.Euler(0f, 0f, jointAngleExtension);
            extensionAngleText.text = jointAngleExtension.ToString();
        }
        else if (AnimationStateController.gameIsLeftRadial)
        {
            PlayMovementRadial();
            extensionAngleText.text = jointAngleRadial.ToString();
        }
        else if (AnimationStateController.gameIsLeftPronation)
        {
            PlayMovementPronation();
            extensionAngleText.text = jointAnglePronation.ToString();
        }

        if (distanceTravelled > PlayerPrefs.GetInt("highestDistance", 0))
        {
            PlayerPrefs.SetFloat("highestDistance", distanceTravelled);
            highestDistanceText.text = distanceTravelled.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBluetooth();
        }
    }

    public void PlayMovementExtension()
    {
        if (initialExtensionReps > 0)
        {
            if (isExtension)
            {
                repsText.text = "Repetition Remaining: " + initialExtensionReps;
                holdTimeText.text = "Extension";
                if (jointAngleExtension >= SliderScript.gameExtensionBaseAngle && jointAngleExtension <= SliderScript.gameExtensionBaseAngle + 2)
                {
                    currentTime -= 1 * Time.deltaTime;
                    distanceTravelled += 3 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);

                    if (currentTime <= 0)
                    {
                        isExtension = !isExtension;
                    }
                }
                else if (jointAngleExtension > SliderScript.gameExtensionBaseAngle + 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";
                    anim.SetBool(Walk_Animation, false);
                    
                }
                else
                {
                    holdTimeText.text = "Wrist Extension";
                    currentTime = SliderScript.gameHoldTimeExtension;
                    anim.SetBool(Walk_Animation, false);
                }
            }
            if (isExtension == false)
            {
                holdTimeText.text = "Flexion";
                if (jointAngleExtension >= SliderScript.gameFlexionBaseAngle - 2 && jointAngleExtension <= SliderScript.gameFlexionBaseAngle)
                {
                    currentTime -= 1 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);
                    distanceTravelled += 3 * Time.deltaTime;

                    if (currentTime <= 0)
                    {
                        isExtension = !isExtension;
                        initialExtensionReps--;
                    }
                }
                else if (jointAngleExtension < SliderScript.gameFlexionBaseAngle - 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";
                    
                }
                else
                {
                    holdTimeText.text = "Wrist Flexion";
                    currentTime = SliderScript.gameHoldTimeExtension;
                    anim.SetBool(Walk_Animation, false);

                }
            }
                

        }
        if (initialExtensionReps <= 0)
        {
            holdTimeText.text = "Exercise done";
            speed = 0;
            enemy.speed = 0;
            UIManager.GameWon();
            StopBluetooth();
        }
        Debug.Log("reps left: " + initialExtensionReps);
        //player.transform.position += Vector3.right * speed * Time.deltaTime;
    }

    public void PlayMovementRadial()
    {
        if (initialRadialReps > 0)
        {
            if (isRadial)
            {
                repsText.text = "Repetition Remaining: " + initialRadialReps;
                holdTimeText.text = "Radial";
                if (jointAngleRadial >= SliderScript.gameRadialBaseAngle && jointAngleRadial <= SliderScript.gameRadialBaseAngle + 2)
                {
                    currentTime -= 1 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);
                    distanceTravelled += 3 * Time.deltaTime;

                    if (currentTime <= 0)
                    {
                        isRadial = !isRadial;
                    }
                }
                else if (jointAngleRadial > SliderScript.gameRadialBaseAngle + 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";
                    anim.SetBool(Walk_Animation, false);

                }
                else
                {
                    holdTimeText.text = "Wrist Radial";
                    currentTime = SliderScript.gameHoldTimeRadial;
                    anim.SetBool(Walk_Animation, false);
                }
            }
            if (isRadial == false)
            {
                holdTimeText.text = " Wrist Ulnar";
                if (jointAngleRadial >= SliderScript.gameUlnarBaseAngle - 2 && jointAngleRadial <= SliderScript.gameUlnarBaseAngle)
                {
                    currentTime -= 1 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);
                    distanceTravelled += 3 * Time.deltaTime;

                    if (currentTime <= 0)
                    {
                        isRadial = !isRadial;
                        initialRadialReps--;
                    }
                }
                else if (jointAngleRadial < SliderScript.gameUlnarBaseAngle - 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";

                }
                else
                {
                    holdTimeText.text = "Wrist Ulnar";
                    currentTime = SliderScript.gameHoldTimeRadial;
                    anim.SetBool(Walk_Animation, false);

                }
            }


        }
        if (initialRadialReps <= 0)
        {
            holdTimeText.text = "Exercise done";
            speed = 0;
            enemy.speed = 0;
            UIManager.GameWon();
            StopBluetooth();
        }
    }

    public void PlayMovementPronation()
    {
        if (initialPronationReps > 0)
        {
            if (isPronation)
            {
                repsText.text = "Repetition Remaining: " + initialPronationReps;
                holdTimeText.text = "Pronation";
                if (jointAnglePronation >= SliderScript.gamePronationBaseAngle && jointAnglePronation <= SliderScript.gamePronationBaseAngle + 2)
                {
                    currentTime -= 1 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);
                    distanceTravelled += 3 * Time.deltaTime;

                    if (currentTime <= 0)
                    {
                        isPronation = !isPronation;
                    }
                }
                else if (jointAnglePronation > SliderScript.gamePronationBaseAngle + 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";
                    anim.SetBool(Walk_Animation, false);

                }
                else
                {
                    holdTimeText.text = "Wrist Pronation";
                    currentTime = SliderScript.gameHoldTimePronation;
                    anim.SetBool(Walk_Animation, false);
                }
            }
            if (isPronation == false)
            {
                holdTimeText.text = " Wrist Supination";
                if (jointAnglePronation >= SliderScript.gameSupinationBaseAngle - 2 && jointAnglePronation <= SliderScript.gameSupinationBaseAngle)
                {
                    currentTime -= 1 * Time.deltaTime;
                    holdTimeText.text = "Time Remaining: " + currentTime.ToString("0");
                    player.transform.position += Vector3.right * speed * Time.deltaTime;
                    anim.SetBool(Walk_Animation, true);
                    distanceTravelled += 3 * Time.deltaTime;

                    if (currentTime <= 0)
                    {
                        isPronation = !isPronation;
                        initialPronationReps--;
                    }
                }
                else if (jointAnglePronation < SliderScript.gameSupinationBaseAngle - 2)
                {
                    holdTimeText.text = "OUCH, lower the angle!!";

                }
                else
                {
                    holdTimeText.text = "Wrist Supination";
                    currentTime = SliderScript.gameHoldTimePronation;
                    anim.SetBool(Walk_Animation, false);

                }
            }


        }
        if (initialPronationReps <= 0)
        {
            holdTimeText.text = "Exercise done";
            speed = 0;
            enemy.speed = 0;
            UIManager.GameWon();
            StopBluetooth();
        }

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            player.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Eat");
            UIManager.GameOver();
            StopBluetooth();
            
        }
    }

    public void bluetoothStream()
    {
        _lineread1 = Finger._bluetoothobj.GetSensor1();

        _splitter1 = _lineread1.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < _splitter1.Length; i++)
        {
            storeSplitter1[i] = _splitter1[i];
        }

        //sensor 1 data
        euler.x = float.Parse(storeSplitter1[1]);
        euler.y = float.Parse(storeSplitter1[3]);
        euler.z = float.Parse(storeSplitter1[5]);


        //sensor 2 data
        euler2.x = float.Parse(storeSplitter1[7]);
        euler2.y = float.Parse(storeSplitter1[9]);
        euler2.z = float.Parse(storeSplitter1[11]);

    }

    //resets all the repetitions
    public void ResetExtensionReps()
    {
        initialExtensionReps = SliderScript.gameNumRepExtension;
        initialRadialReps = SliderScript.gameNumRepRadial;
        initialPronationReps = SliderScript.gameNumRepPronation;

    }
    public void StopBluetooth()
    {
        Finger._bluetoothobj.Stop();
        Debug.Log("Stopped bluetooth");
    }
}
