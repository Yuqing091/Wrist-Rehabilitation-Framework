using System;
using System.Threading;
using UnityEngine;
using ServerReceiver;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class FishScript : MonoBehaviour
{
    private float moveForce = 5.0f;
    [SerializeField]
    private GameObject player;

    // TCP Bluetooth Object
    private Server _bluetoothobj;
    private float jointAngle = 0.0f;
    private float playerPosition = 0.0f;

    private string _lineread1;
    private string[] _splitter1;
    private string[] storeSplitter1 = new string[30];

    private Vector3 euler = new Vector3(0, 0, 0);
    private Vector3 euler2 = new Vector3(0, 0, 0);
    private char[] _delimiter = { ',', ':', '{', '}', '[', ']', '\"', ' ', '|' };
    void Start()
    {
        //IMUdata
        // Start receiving from bluetooth of both sensors instead of IP 
        Debug.Log("Starting");
        _bluetoothobj = new Server();
        Debug.Log("Created");
        _bluetoothobj.Start();
        Debug.Log("Starting Bluetooth");

        // Wait to ensure bluetooth communication begins before calibration
        Thread.Sleep(300);
        for (int i = 0; i < storeSplitter1.Length; i++)
        {
            storeSplitter1[i] = "0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        _lineread1 = _bluetoothobj.GetSensor1();

        _splitter1 = _lineread1.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < _splitter1.Length; i++)
        {
            storeSplitter1[i] = _splitter1[i];
        }

        //sensor 1 data
        euler.x = float.Parse(storeSplitter1[1]);
        euler.y = float.Parse(storeSplitter1[3]);
        euler.z = float.Parse(storeSplitter1[5]);

        //Debug.Log("x: " + euler.x + "y: " + euler.y + "z: " + euler.z);

        //sensor 2 data
        euler2.x = float.Parse(storeSplitter1[7]);
        euler2.y = float.Parse(storeSplitter1[9]);
        euler2.z = float.Parse(storeSplitter1[11]);
        //Debug.Log("x2: " + euler2.x + "y2: " + euler2.y + "z2: " + euler2.z);

        jointAngle = -(euler.y - euler2.y);
        playerPosition = jointAngle / 35;

        //Debug.Log("player position: " + playerPosition);
        PlayMovement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBluetooth();
        }
    }

    public void PlayMovement()
    {
        player.transform.position += new Vector3(0.0f, playerPosition, 0.0f) * Time.deltaTime * moveForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
            player.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    public void StopBluetooth()
    {
        _bluetoothobj.Stop();
        Debug.Log("Stopped bluetooth");
    }
}
