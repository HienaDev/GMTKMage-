using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckIfGameIsOver : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerMove player;
    private TimerDungeon timer;

    [SerializeField] private GameObject arenaUpgrade;
    [SerializeField] private GameObject playerUpgrade;

    [SerializeField] private UnityEvent playerWin;

    [SerializeField] private UnityEvent arenaWin;

    private bool gameRunning = true;
    void Start()
    {
        timer = FindObjectOfType<TimerDungeon>();
    }

    // Update is called once per frame
    void Update()
    {

        if(timer.TimeRemaining <= 0 && gameRunning)
        {
            gameRunning = false;
            playerWin.Invoke();
            arenaUpgrade.SetActive(true);
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerMove>();
        }

        if(player.IsDead() && gameRunning)
        {
            gameRunning = false;
            arenaWin.Invoke();
            playerUpgrade.SetActive(true);
            timer.StopTimer();
        }


    }

    public void ResetGame()
    {
        player.ResetPlayer();
        timer.ResetTimer();
        gameRunning = true;
        playerUpgrade.SetActive(false);
        arenaUpgrade.SetActive(false);
    }
}
