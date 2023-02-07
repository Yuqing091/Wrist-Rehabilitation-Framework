using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTiles : MonoBehaviour
{

    private float speed = 1.6f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
