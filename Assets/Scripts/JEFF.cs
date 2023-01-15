using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Player manager
public class JEFF : MonoBehaviour
{
    private float taskClock;
    public float taskTime = 10f;

    public TextMeshProUGUI ClockUI;
    public TextMeshProUGUI LivesUI;

    public int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        taskClock = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        taskClock -= Time.deltaTime;
        ClockUI.text = taskClock.ToString("0");
        
        if(taskClock < 0)
        {
            // Debug.Break();
        }
    }

    public void TaskCompleted()
    {
        taskClock = taskTime;
    }
}
