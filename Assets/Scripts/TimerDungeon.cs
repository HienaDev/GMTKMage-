using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDungeon : MonoBehaviour
{
    // Start is called before the first frame update

    private TMP_Text text;

    [SerializeField] private float timeRemaining = 20;
    private bool timerIsRunning = false;

    void Start()
    {



        // Starts the timer automatically
        timerIsRunning = true;

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
            if (Mathf.FloorToInt(timeRemaining % 60).ToString().Length > 1)
                text.text = "0" + Mathf.FloorToInt(timeRemaining / 60).ToString() + " : " + Mathf.FloorToInt(timeRemaining % 60).ToString();
            else
                text.text = "0" + Mathf.FloorToInt(timeRemaining / 60).ToString() + " : " + Mathf.FloorToInt(timeRemaining % 60).ToString() + "0";
        }
    }
}
