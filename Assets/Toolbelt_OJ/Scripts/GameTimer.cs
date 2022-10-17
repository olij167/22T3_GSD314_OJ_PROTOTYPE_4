using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float seconds, minutes, hours;
    private TextMeshProUGUI timerUI;

    private void Start()
    {
        timerUI = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds > 59)
        {
            minutes += 1;
            seconds = 0;
        }

        if (minutes > 59)
        {
            hours += 1;
            minutes = 0;
        }

        if (hours > 0)
        {
            timerUI.text = hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else if (minutes > 0)
        {
            timerUI.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            timerUI.text = seconds.ToString("00");
        }
    }
}
