using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMove player;
    private BoxCollider2D hitbox;
    private Rigidbody2D playerRB;

    private float colliderNormalOffset = -1.5f;
    private float colliderNormalSize = 25f;

    private float colliderCrouchedOffset = -5f;
    private float colliderCrouchedSize = 17.6f;

    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
        playerRB = GetComponentInParent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Crouched && player.Grounded)
        {
            hitbox.size = new Vector2(hitbox.size.x, colliderCrouchedSize);
            hitbox.offset = new Vector2(hitbox.offset.x, colliderCrouchedOffset);
        }
        else
        {
            hitbox.size = new Vector2(hitbox.size.x, colliderNormalSize);
            hitbox.offset = new Vector2(hitbox.offset.x, colliderNormalOffset);
        }

        if (player.IsDashing)
        {
            hitbox.enabled = false;
        }
        else
            hitbox.enabled = true;
    }
}
