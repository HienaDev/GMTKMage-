using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    [SerializeField] private Image[] images;

    private PlayerMove player;

    private int health;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMove>();
        }
        else
            health = player.Health;

        if (player != null)
        {
            for (int i = 0; i < health; i++)
            {
                images[i].gameObject.SetActive(true);
            }
            for (int i = health; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }
}