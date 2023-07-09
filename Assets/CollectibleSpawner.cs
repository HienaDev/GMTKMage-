using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{

    public GameObject collectible;

    // Start is called before the first frame update
    void Start()
    {
        spawnCollectible();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnCollectible()
    {
        Transform spot = transform.GetChild(0);
        Instantiate(gameObject, spot);
    }
}
