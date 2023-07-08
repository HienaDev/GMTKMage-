using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float deathTimer = 1f;
    private float wasSpawned;
    

    void Start()
    {
        wasSpawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - wasSpawned) > deathTimer)
        {
            Destroy(gameObject);
        }
    }
}
