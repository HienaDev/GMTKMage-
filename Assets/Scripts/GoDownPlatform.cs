using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDownPlatform : MonoBehaviour
{
    // Start is called before the first frame update

    private PlatformEffector2D platforms;

    [SerializeField] private float platformTimer = 1f;
    private float platformWasReversed;
    private bool platformReversed = false;



    void Start()
    {
        platforms = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && !platformReversed)
        {
            platformReversed = true;
            platforms.rotationalOffset = 180f;
            platformWasReversed = Time.time;
        }

        if ((Time.time - platformWasReversed) > platformTimer && platformReversed)
        {
            platformReversed = false;
            platforms.rotationalOffset = 0f;
        }

        Debug.Log(platforms.rotationalOffset);

    }
}
