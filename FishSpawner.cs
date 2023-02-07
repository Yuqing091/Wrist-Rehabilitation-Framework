using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float maxTime = 8f;
    private float timer = 0f;
    public GameObject fish;
    private float height = 1.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > maxTime )
        {
            GameObject newFish = Instantiate(fish);
            if(height > 1.7f)
            {
                newFish.transform.position = transform.position + new Vector3(0, height, 0);
                height = -1.2f;
            }
            else if(height < -1.1f)
            {
                newFish.transform.position = transform.position + new Vector3(0, height, 0);
                height = 1.8f;
            }
            
            Destroy(newFish, 80);
            timer = 0;
        }


        timer += Time.deltaTime;
        

    }
}
