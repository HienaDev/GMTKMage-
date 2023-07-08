using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : MonoBehaviour
{

    float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        processSpell();
        if(Time.time - spawnTime > 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void fireSpell(Vector3 startPos, Vector3 direction)
    {
        direction.z = startPos.z;
        this.transform.position = startPos;
        this.transform.up = startPos+direction - transform.position;
        this.transform.localScale = new Vector3(0.5f, 1, 1);
        spawnTime = Time.time;
    }

    void processSpell() {
        this.transform.localScale = new Vector3(0.5f, Mathf.Pow(1 + (Time.time - spawnTime)*2, 5), 1);
    }
}
