using System;
using System.Collections;
using System.Collections.Generic;
using ServerReceiver;
using UnityEngine;
using UnityEngine.UI;

public class TileSpawner : MonoBehaviour
{
    //Canvas switch
    public TilesManager manager;
    public Text highScoreText;
    public Text score;
    public Text lifeText;
    private float maxTime;
    private float tileTime;
    private float timer = 0f;
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject tile4;
    private int randNum;
    GameObject newTile1;
    GameObject newTile2;
    GameObject newTile3;
    GameObject newTile4;
    private bool one = false;
    private bool two = false;
    private bool three = false;
    private bool four = false;
    private bool isOver;
    private int currentScore;
    private int life;

    //IMUdata
    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    ////////
    private float leftIndexAngle;
    private float leftMiddleAngle;
    private float leftRingAngle;
    private float leftPinkyAngle;
    
    private char[] _delimiter = { 'R', 'r', 'o', 'l', 'P', 'p', 'i', 't', 'c', 'h', 'a', 'w', 'Y', 'x', 'y', 'z', ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Created");
        Finger._bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }
        currentScore = 0;
        life = 3;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("PTHighScore", 0).ToString();
        isOver = false;
        FindObjectOfType<AudioManager>().Play("Background");
        maxTime = 4f;
        tileTime = 3.9f;
}

    // Update is called once per frame
    void Update()
    {
        bluetoothStream();

        if (timer > maxTime)
        {
            //new 1:-3.27 2.-1.1 3:1.068 4: 3.24
            //1:-3.556 2: -1.375 3:0.81 4:2.99
            randNum = UnityEngine.Random.Range(1, 5);
            Debug.Log(randNum);

            if(randNum == 1)
            {
                newTile1 = Instantiate(tile1);
                newTile1.transform.position = transform.position + new Vector3(-3.27f, -10, 0);
                one = true;
                two = false;
                three = false;
                four = false;
            }
            else if(randNum == 2)
            {
                newTile2 = Instantiate(tile2);
                newTile2.transform.position = transform.position + new Vector3(-1.1f, -10, 0);
                one = false;
                two = true;
                three = false;
                four = false;
            }
            else if (randNum == 3)
            {
                newTile3 = Instantiate(tile3);
                newTile3.transform.position = transform.position + new Vector3(1.068f, -10, 0);
                one = false;
                two = false;
                three = true;
                four = false;
            }
            else if (randNum == 4)
            {
                newTile4 = Instantiate(tile4);
                newTile4.transform.position = transform.position + new Vector3(3.24f, -10, 0);
                one = false;
                two = false;
                three = false;
                four = true;
            }

            

            Destroy(newTile1,tileTime);
            Destroy(newTile2, tileTime);
            Destroy(newTile3, tileTime);
            Destroy(newTile4, tileTime);
            timer = 0;
        }

        if (!isOver)
        {
            if (leftIndexAngle < 10 && one && !two && !three&&!four)
            {
                newTile1.GetComponent<Renderer>().material.color = Color.green;
                currentScore++;
                maxTime -= 0.1f;
                tileTime -= 0.1f;
                FindObjectOfType<AudioManager>().Play("Index");
                one = !one;
                Debug.Log("Destroyed");
            }
            else if (leftMiddleAngle < 10 && one || leftRingAngle < 10 && one || leftPinkyAngle < 10 && one)
            {
                newTile1.GetComponent<Renderer>().material.color = Color.red;
                one = !one;
                life--;
            }

            if (leftMiddleAngle < 10 && two &&!one&&!three&&!four)
            {
                newTile2.GetComponent<Renderer>().material.color = Color.green;
                currentScore++;
                maxTime -= 0.1f;
                tileTime -= 0.1f;
                FindObjectOfType<AudioManager>().Play("Middle");
                two = !two;
                Debug.Log("Destroyed");
            }
            else if (leftRingAngle < 10 && two || leftIndexAngle < 10 && two || leftPinkyAngle < 10 && two)
            {
                newTile2.GetComponent<Renderer>().material.color = Color.red;
                two = !two;
                life--;
            }

            if (leftRingAngle < 10 && three && !one && !two && !four)
            {
                newTile3.GetComponent<Renderer>().material.color = Color.green;
                currentScore++;
                maxTime -= 0.1f;
                tileTime -= 0.1f;
                FindObjectOfType<AudioManager>().Play("Ring");
                three = !three;
                Debug.Log("Destroyed");
            }
            else if (leftMiddleAngle < 10 && three || leftIndexAngle < 10 && three || leftPinkyAngle < 10 && three)
            {
                newTile3.GetComponent<Renderer>().material.color = Color.red;
                three = !three;
                life--;
            }

            if (leftPinkyAngle < 10 && four && !one && !two && !three)
            {
                newTile4.GetComponent<Renderer>().material.color = Color.green;
                currentScore++;
                maxTime -= 0.1f;
                tileTime -= 0.1f;
                FindObjectOfType<AudioManager>().Play("Pinky");
                four = !four;
                Debug.Log("Destroyed");
            }
            else if (leftMiddleAngle < 10 && four || leftIndexAngle < 10 && four || leftRingAngle < 10 && four)
            {
                newTile4.GetComponent<Renderer>().material.color = Color.red;
                four = !four;
                life--;
            }

            if (currentScore > PlayerPrefs.GetInt("PTHighScore", 0))
            {
                PlayerPrefs.SetInt("PTHighScore", currentScore);
                highScoreText.text = "High Score: " + currentScore.ToString();
            }

            score.text = "Score: " + currentScore.ToString("0");
            lifeText.text = "Life: " + life.ToString("0");
            timer += Time.deltaTime;
        }

        if(life <= 0)
        {
            manager.GameOver();
            isOver = true;
            StopBluetooth();
            life = 3;
            Time.timeScale = 0;
        }
        Debug.Log("maxTime: " + maxTime);
        Debug.Log("tileTime: " + tileTime);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        leftIndexAngle = float.Parse(storeSplitter1[0]);
        leftMiddleAngle = float.Parse(storeSplitter1[1]);
        leftRingAngle = float.Parse(storeSplitter1[2]);
        leftPinkyAngle = float.Parse(storeSplitter1[3]);

        //Debug.Log("Index: " + leftIndexAngle + "middle: " + leftMiddleAngle + "ring " + leftRingAngle + "pinky" + leftPinkyAngle);
    }

    public void StopBluetooth()
    {
        Finger._bluetoothobj.Stop();
        Debug.Log("Bluetooth Stopped");
    }

}
