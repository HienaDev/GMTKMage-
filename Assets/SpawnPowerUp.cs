using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SpawnPowerUp : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float timer = 10f;
    private float justSpawned = 0f;
    private bool readyToSpawn = false;

    [SerializeField] private GameObject collectible;
    [SerializeField] private Transform[] positions;
    private System.Random rnd;
    private int i;

    void Start()
    {
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToSpawn)
        {
            i = rnd.Next(0, positions.Length);
            Instantiate(collectible, positions[i]);
            readyToSpawn = false;
            justSpawned = Time.time;
        }

        if (Time.time - justSpawned > timer)
        {
            Debug.Log("spawns");
            readyToSpawn = true;
        }
        Debug.Log(Time.time);
    }
}
