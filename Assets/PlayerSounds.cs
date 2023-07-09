using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioClip dash;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip hit;


    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDash()
    {
        audioSource.clip = dash;
        audioSource.Play();
    }

    public void playJump()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }

    public void playDeath()
    {
        audioSource.clip = death;
        audioSource.Play();
    }

    public void playHit()
    {
        audioSource.clip = hit;
        audioSource.Play();
    }
}
