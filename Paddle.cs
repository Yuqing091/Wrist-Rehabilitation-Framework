using System;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{

    [SerializeField]
    private Text jointAngle;
    private float speed;
    private float movementHorizontal;

    //angle calibration variables
    private float calibratedAngle = 0f;
    private float jointAngleRadial;
    private float tempJointAngle;
    private float currentTime = 0;
    private float maxTime = 0.2f;

    public BreakOutManager manager;
    //IMUdata
    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    ////////

    //sensor 1 data
    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };

    // TCP Bluetooth Object
    private Server _bluetoothobj;

    void Start()
    {
        Debug.Log("Starting");
        _bluetoothobj = new Server();
        Debug.Log("Created");
        _bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }
        FindObjectOfType<AudioManager>().Play("Background");

    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > maxTime && currentTime < 0.5f)
        {
            tempJointAngle = jointAngleRadial;
            Debug.Log("Initial temp angle is: " + tempJointAngle);
        }

        bluetoothStream();
        jointAngle.text = "Joint Angle: " + calibratedAngle.ToString("0");

        jointAngleRadial = -euler.z;
        speed = calibratedAngle / 4;
        Debug.Log("speed: " + speed);
        if((speed > 0 && transform.position.x < 7.3) || (speed < 0 && transform.position.x > -7.6))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (manager.isOver)
        {
            StopBluetooth();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBluetooth();
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            tempJointAngle = jointAngleRadial;
            Debug.Log("Angle Calibrated");
        }

        calibratedAngle = jointAngleRadial - tempJointAngle;

    }

    public void bluetoothStream()
    {
        _lineread1 = _bluetoothobj.GetSensor1();

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
        _bluetoothobj.Stop();
        Debug.Log("Bluetooth Stopped");
    }
}