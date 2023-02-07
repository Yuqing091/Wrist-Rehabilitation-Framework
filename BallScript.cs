using System;
using UnityEngine;
using UnityEngine.UI;
using ServerReceiver;

public class BallScript : MonoBehaviour
{
    public DodgeManager manager;
    public Text scoreText;
    public Text highScoreText;
    public Text jointAngleText;
    public GameObject ball;

    private float score = 0f;
    private float horizontal;
    private float speed;
    private float jointAngle = 0f;
    private float tempJointAngle;
    private float finalJointAngle = 0f;
    private float currentTime = 0;
    private float maxTime = 0.2f;
    private Rigidbody2D rb;
    // Update is called once per frame

    ////////////////////////////////////////
    //IMUdata
    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    ////////

    //sensor 1 data
    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };

    ////////////////////////////////////////////
    void Start()
    {
        ///////////////////////////////////////////////////                  //
        Debug.Log("Created");                            //
        Finger._bluetoothobj.Start();                           //
        Debug.Log("Starting Bluetooth");                 //   
                                                         //               
        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }
        //////////////////////////////////////////////////
        rb = GetComponent<Rigidbody2D>();
        highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("DSHighScore").ToString("0");
        FindObjectOfType<AudioManager>().Play("Background");
    }
    void Update()
    {

        currentTime += Time.deltaTime;

        if (currentTime > maxTime && currentTime < 0.5f)
        {
            tempJointAngle = jointAngle;
            Debug.Log("Initial temp angle is: " + tempJointAngle);
        }

        bluetoothStream();
        jointAngle = euler.x;
        jointAngleText.text = "Joint Angle: " + finalJointAngle.ToString("0");
        speed = finalJointAngle / 20;
        score += (Time.timeScale/100);
        scoreText.text = "Score: " + score.ToString("0");
        rb.AddForce(new Vector3(speed,0f,0f)) ;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBluetooth();
        }

        if (score > PlayerPrefs.GetFloat("DSHighScore"))
        {
            PlayerPrefs.SetFloat("DSHighScore", score);
            highScoreText.text = "High Score: " + score.ToString("0");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tempJointAngle = jointAngle;
            Debug.Log("Angle Calibrated");
        }

        finalJointAngle = jointAngle - tempJointAngle;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike")) 
        {
            StopBluetooth();
            FindObjectOfType<AudioManager>().Play("Pop");
            ball.SetActive(false);
            manager.GameOver();
            
        }

        if (collision.gameObject.CompareTag("Boundry"))
        {
            StopBluetooth();
            manager.GameOver();
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

    public void StopBluetooth()
    {
        Finger._bluetoothobj.Stop();
        Debug.Log("Bluetooth Stopped");
    }
}
