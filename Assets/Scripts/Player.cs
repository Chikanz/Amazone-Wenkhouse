using System;
using System.Collections;
using System.Collections.Generic;
using dook.tools.animatey;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animatey BlackoutAnim;
    public Image Blackout;

    private FirstPersonController FPS;
    private float speed;
    public float penguinKOTime = 0.6f;

    public int PlayerID = 0;
    
    void Start()
    {
        BlackoutAnim.Action = (val, args) =>
        {
            var col = val > 0.99f ? 1 : 0;
            Blackout.color = new Color(col, col, col, val);
        };

        FPS = GetComponent<FirstPersonController>();
        speed = FPS.MoveSpeed;

        // BlackoutAnim.ChainAt(0.01f).Chain += delegate(object sender, EventArgs args)
        // {
        //     print("Chain");
        //     Blackout.color = new Color(1, 1, 1, 1);
        // };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Penguin"))
        {
            if (collision.gameObject.GetComponent<Penguin>().thrown)
            {
                BlackoutAnim.duration = 1.5f;
                BlackoutAnim.Play(this);
                FPS.MoveSpeed = 0;
                CancelInvoke(nameof(ResetSpeed));
                Invoke(nameof(ResetSpeed), penguinKOTime);
            }
        }
    }

    private void ResetSpeed()
    {
        FPS.MoveSpeed = speed;
    }
}
