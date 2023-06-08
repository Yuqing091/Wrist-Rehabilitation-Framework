using System;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;
using System.IO;

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

    //Recording data to csv
    private FileStream streamFile;
    private StreamWriter writeStream;
    private string timeStamp;
    private string timeStampPrint;
    private DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    private DateTime dateTimeNow;
    private TimeSpan timeElapsed;
    private double timeElapsedInseconds;

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

        setTimeStamp();
        CreateBreakOutFile(); // creating the csv file for the data recording

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

        //writing to filestream
        dateTimeNow = DateTime.Now;

        timeElapsed = dateTimeNow.Subtract(dateTime);

        timeElapsedInseconds = Convert.ToDouble(timeElapsed.TotalSeconds);

        writeStream.WriteLine(",,,," + timeElapsedInseconds.ToString() + "," + calibratedAngle.ToString());

        writeStream.Flush();
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
    public void CreateBreakOutFile()
    {
        streamFile = new FileStream("C:\\Unity Projects\\Wrist Rehabilitation Framework Final\\Wrist Rehabilitation Framework v2\\DataLog\\" + "_BreakOut_" + timeStamp + ".csv", FileMode.OpenOrCreate);
        writeStream = new StreamWriter(streamFile);

        writeStream.WriteLine("{0}", timeStampPrint);
        writeStream.WriteLine("Breakout Game");
        writeStream.WriteLine(",,,,Timestamp,Y_Angle");
    }

    public void StopBluetooth()
    {
        _bluetoothobj.Stop();
        Debug.Log("Bluetooth Stopped");
    }
}