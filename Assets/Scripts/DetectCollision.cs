using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    [SerializeField] private LayerMask laserBeam;
    [SerializeField] private GameObject player;
    private PlayerMove playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int x = 1 << collision.gameObject.layer;

        Debug.Log("Hit");

        if (x == laserBeam.value && !playerScript.Flashing)
        {
            playerScript.TakeDamage(1);
            Debug.Log("Laser hit");
            if (playerScript.Health <= 0)
            {
                Destroy(player);
            }
        }
    }
}
