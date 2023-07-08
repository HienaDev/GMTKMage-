using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{

    float spawnTime;

    [SerializeField] public GameObject pointer;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public abstract void fireSpell(Vector3 startPos, Vector3 direction);
}
