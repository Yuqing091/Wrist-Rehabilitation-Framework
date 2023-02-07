using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    private float maxTime = 3f;
    private float timer = 0f;
    public GameObject Pipe;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (timer > maxTime)
        {
            GameObject newPipe = Instantiate(Pipe);
            newPipe.transform.position = transform.position + new Vector3(0, Random.Range(-2, 5), 0);

            Destroy(newPipe, 20);
            timer = 0;
        }


        timer += Time.deltaTime;


    }
}
