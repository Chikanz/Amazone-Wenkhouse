using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Player round manager
public class JEFF : MonoBehaviour
{
    private float taskClock;
    public float taskTime = 10f;

    public TextMeshProUGUI ClockUI;

    public delegate void RoundFinish(int playerID);
    public RoundFinish OnRoundFinish;
    
    public bool countDown = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Reset(15);
    }

    // Update is called once per frame
    void Update()
    {
        if(countDown) taskClock -= Time.deltaTime;
        ClockUI.text = taskClock.ToString("0");
        
        ClockUI.color = taskClock > 5 ? Color.white : Color.red;
        ClockUI.fontSize = taskClock > 5 ? 170 : 400;
        
        if(taskClock < 0 && countDown)
        {
            countDown = false;
            OnRoundFinish?.Invoke(GetComponent<Player>().PlayerID); //Pass player who died
        }
    }

    public void Reset(int customTime = 0)
    {
        if(customTime == 0) taskClock = taskTime;
        else taskClock = customTime;
        countDown = true;
    }
}
