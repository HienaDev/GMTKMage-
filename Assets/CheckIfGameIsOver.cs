using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfGameIsOver : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerMove player;
    private TimerDungeon timer;

    [SerializeField] private GameObject arenaUpgrade;
    [SerializeField] private GameObject playerUpgrade;
    void Start()
    {
        timer = FindObjectOfType<TimerDungeon>();
    }

    // Update is called once per frame
    void Update()
    {

        if(timer.TimeRemaining <= 0)
        {
            arenaUpgrade.SetActive(true);
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerMove>();
        }

        if(player.IsDead())
        {
            playerUpgrade.SetActive(true);
            timer.StopTimer();
        }


    }

    public void ResetGame()
    {
        player.ResetPlayer();
        timer.ResetTimer();
        playerUpgrade.SetActive(false);
        arenaUpgrade.SetActive(false);
    }
}
