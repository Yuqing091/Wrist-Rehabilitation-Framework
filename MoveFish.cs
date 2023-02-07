using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFish : MonoBehaviour
{

    public float speed;
    private void Start()
    {
        speed = 5.0f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; 
    }

    
}
