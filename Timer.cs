using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Timer : MonoBehaviour
{

    [ReadOnly]
    public float timeRemaining;
    [ReadOnly]
    public bool timerIsRunning = false;
    [ReadOnly]
    public string TimeDisplay;

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimeDisplay = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void SetTimer(float time)
    {
        timeRemaining = time;
    }

    public Timer(float timerTime)
    {
        timeRemaining = timerTime;
    }
}
