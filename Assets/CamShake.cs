using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    bool shaking = false;
    float shakeStart = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            processShake();
        }
        if(Time.time-shakeStart > 0.25f && shaking == true)
        {
            endShake();
        }
    }

    public void startShake()
    {
        shaking = true;
        shakeStart = Time.time;
    }

    void processShake()
    {
        this.transform.position = new Vector3(0, 0, -10) + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
    }

    void endShake()
    {
        this.transform.position = new Vector3(0, 0, -10);
        shaking = false;
    }
}
