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
    
    private Collider[] feetColliderBuffer = new Collider[20];
    private Animator AC;
    private static readonly int Slip_Anim = Animator.StringToHash("Slip");
    
    [SerializeField] private float slipInvuln = 2f;
    private float slipTimer = 0;

    private void Awake()
    {
        AC = GetComponent<Animator>();
    }

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
        //Poll for slippable surface at player's feet every 4 frames (1/4 second)
        if(Time.frameCount % 4 == 0){
            int size = Physics.OverlapSphereNonAlloc(transform.position, 0.5f, feetColliderBuffer);
            for(int i = 0; i < size; i++){
                if(feetColliderBuffer[i].gameObject.CompareTag("Fish") ){
                    Slip();
                    int size2 = Physics.OverlapSphereNonAlloc(transform.position, 3, feetColliderBuffer);
                    //destroy all fish around player
                    for(int j = 0; j < size2; j++){
                        if(feetColliderBuffer[j].gameObject.CompareTag("Fish") ){
                            Destroy(feetColliderBuffer[j].gameObject);
                        }
                    }
                }
            }
        }
        
        //Slip invulnerability timer
        if(slipTimer > 0){
            slipTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Penguin"))
        {
            if (collision.gameObject.GetComponent<Penguin>().thrown)
            {
                Slip();
            }
        }
    }

    //Play the slip animation (triggers eat shit from event)
    public void Slip()
    {
        if(slipTimer > 0) return;
        
        slipTimer = slipInvuln;
        FPS.MoveSpeed = 0;
        // AC.SetTrigger(Slip_Anim);
        GetComponent<Animation>().Play();
    }
    
    //Knock out animation
    public void EatShit()
    {
        BlackoutAnim.duration = 1.5f;
        BlackoutAnim.Play(this);
        FPS.MoveSpeed = 0;
        CancelInvoke(nameof(ResetSpeed));
        Invoke(nameof(ResetSpeed), penguinKOTime);
    }

    private void ResetSpeed()
    {
        FPS.MoveSpeed = speed;
    }
}
