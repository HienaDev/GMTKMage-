using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDungeon : MonoBehaviour
{
    // Start is called before the first frame update

    private TMP_Text text;

    [SerializeField] private float time = 60;

    public float TimeRemaining { get; private set; }
    private bool timerIsRunning = false;

    void Start()
    {



        // Starts the timer automatically
        timerIsRunning = true;

        TimeRemaining = time;

        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                TimeRemaining = 0;
                timerIsRunning = false;
            }
            if (Mathf.FloorToInt(TimeRemaining % 60).ToString().Length > 1)
                text.text = "0" + Mathf.FloorToInt(TimeRemaining / 60).ToString() + " : " + Mathf.FloorToInt(TimeRemaining % 60).ToString();
            else
                text.text = "0" + Mathf.FloorToInt(TimeRemaining / 60).ToString() + " : 0" + Mathf.FloorToInt(TimeRemaining % 60).ToString();
        }
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }
}
