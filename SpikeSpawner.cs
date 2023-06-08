using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    private float maxTime = 3.5f;
    private float timer = 0f;
    public GameObject spike;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (timer > maxTime)
        {
            FindObjectOfType<AudioManager>().Play("SpawnSpike");
            maxTime = Random.Range(1, 3.5f);
            GameObject newSpike = Instantiate(spike);
            newSpike.transform.position = transform.position + new Vector3(Random.Range(-5f, 5.5f),0 , 0);

            Destroy(newSpike, 20);
            timer = 0;
        }


        timer += Time.deltaTime;


    }
}
