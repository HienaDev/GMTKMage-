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

    private float timerText = 0.15f;
    //private bool textChanged = false;
    private float textJustChanged = 0f;

    private bool colored = true;

    void Start()
    {

        text = GetComponent<TMP_Text>();

        ResetTimer();

        


    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (Time.time - textJustChanged > timerText && TimeRemaining < 4)
            {
                colored = !colored;
                textJustChanged = Time.time;
            }

            if (colored)
                text.color = Color.red;
            else
                text.color = Color.white;

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

    public void ArenaUpgradeAdd10s()
    {
        time += 10;
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public void ResetTimer()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        text.color = Color.red;
        TimeRemaining = time;

    
    }
}
