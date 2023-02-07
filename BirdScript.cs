using System;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour
{
    //Variable declaration
    [SerializeField]
    private float moveForce =0.5f;
    //private float XSpeed = 1.5f;
    [SerializeField]
    private Rigidbody2D myBody;
    private SpriteRenderer sr;

    private int score = 0;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text gameOverScoreText;
    [SerializeField]
    private Text highScore;
    [SerializeField]
    private Text jointAngleText;
    [SerializeField]
    private GameObject hand;

    private float movementY;
    private float currentTime;
    private float maxTime = 0.5f;



    public FloppyBirdGameManager gameManager;

    private string GROUND_TAG = "Ground";
    private string PIPE_TAG = "Pipe";
    private string SKY_TAG = "Sky";
    public static bool isOver;

    private float jointAngleExtension;
    private float tempJointAngle;
    private float calibratedAngle;

    //IMUdata
    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    ////////

    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };




    // TCP Bluetooth Object
    //private Server _bluetoothobj;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Starting");
        //_bluetoothobj = new Server();
        Debug.Log("Created");
        Finger._bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }

        movementY = 0;
        myBody.gravityScale = 0;
        isOver = false;
        FindObjectOfType<AudioManager>().Play("background");

        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > maxTime && currentTime <1f)
        {
            tempJointAngle = jointAngleExtension;
            Debug.Log("Initial temp angle is: " + tempJointAngle);
        }
        
        bluetoothStream();

        jointAngleExtension = -(euler.y - euler2.y);
        if (!isOver)
        {

            PlayerMoveKeyboard();
            AnimatePlayer();
            scoreText.text = score.ToString();
            gameOverScoreText.text = score.ToString();
            //Debug.Log(quat[0].x);

        }

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore.text = score.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBluetooth();
        }
        jointAngleText.text = calibratedAngle.ToString("0");
        hand.transform.rotation = Quaternion.Euler(0f, 0f, calibratedAngle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tempJointAngle = jointAngleExtension;
        }

        calibratedAngle = jointAngleExtension - tempJointAngle;
    }



    void PlayerMoveKeyboard()
    {

        movementY = calibratedAngle / 25;

        transform.position += new Vector3(0f, movementY , 0f) * Time.deltaTime * moveForce;

    }

    void AnimatePlayer()
    {
        if (movementY > 0.5)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 30.0f);

        }
        else if (movementY < -0.5)
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, -30.0f);

        }
        else
        {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PIPE_TAG) || collision.gameObject.CompareTag(SKY_TAG))
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            FindObjectOfType<AudioManager>().Play("Die");
            sr.flipY = true;
            myBody.gravityScale = 1;

            isOver = true;
            

        }

        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            FindObjectOfType<AudioManager>().Play("Hit");
            StopBluetooth();
            gameManager.GameOver();
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
        euler.x = float.Parse(storeSplitter1[0]);
        euler.y = float.Parse(storeSplitter1[1]);
        euler.z = float.Parse(storeSplitter1[2]);


        //sensor 2 data
        euler2.x = float.Parse(storeSplitter1[3]);
        euler2.y = float.Parse(storeSplitter1[4]);
        euler2.z = float.Parse(storeSplitter1[5]);

        Debug.Log("x: " + euler.x + "y: " + euler.y + "z: " + euler.z);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            FindObjectOfType<AudioManager>().Play("point");
            other.gameObject.SetActive(false);
            score++;
        }
    }

    public void StopBluetooth()
    {
        Finger._bluetoothobj.Stop();
        Debug.Log("Bluetooth Stopped");
    }



}