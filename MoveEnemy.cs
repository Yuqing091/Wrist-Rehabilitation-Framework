using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    public float speed;

    private void Start()
    {
        speed = 3.0f;
    }
   
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime; 
    }

    
}
