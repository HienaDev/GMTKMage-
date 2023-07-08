using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrap : Trap
{

    float spawnTime;

    //[SerializeField] public GameObject pointer;

    AudioSource shoot;

    public GameObject beam;
    public GameObject laser;
    public GameObject sprite;

    float laserTime = 0.75f;
    float beamTime = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        shoot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        processSpell();
    }

    public override void fireSpell(Vector3 startPos, Vector3 direction, float timeHeld, bool friend)
    {
        timeHeld = Mathf.Pow(0.1f+timeHeld, 2);
        beamTime = timeHeld; 

        direction.z = startPos.z;
        this.transform.position = startPos;
        this.transform.up = -(startPos+direction - transform.position);
        spawnTime = Time.time;
        beam.SetActive(false);
        sprite.transform.localScale = new Vector3(0.5f, 0.5f, 1f) * (1 + timeHeld);
        if (friend)
        {
            sprite.transform.localScale = sprite.transform.localScale / 2f;
        }
    }

    void processSpell() {
        float timeFromSpawn = Time.time - spawnTime;
        if(timeFromSpawn < laserTime)
        {
            Vector3 newscale = laser.transform.localScale;
            newscale.x = (timeFromSpawn-laserTime)/laserTime;
            laser.transform.localScale = newscale;
        }

        if(timeFromSpawn > laserTime && timeFromSpawn < laserTime+beamTime)
        {
            if (!beam.activeSelf)
            {
                laser.SetActive(false);
                beam.SetActive(true);
                shoot.pitch = shoot.pitch / beamTime;
                shoot.Play();
            };
            Vector3 beamScale = beam.transform.localScale;
            beamScale.x = ((beamTime+laserTime)-timeFromSpawn)/(beamTime+laserTime);
            beam.transform.localScale = beamScale;
        }

        if(timeFromSpawn > laserTime + beamTime)
        {
            beam.SetActive(false);
            this.transform.localScale = this.transform.localScale * 0.95f;
            if(this.transform.localScale.magnitude < 0.1)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
