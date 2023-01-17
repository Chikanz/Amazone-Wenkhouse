using System;
using System.Collections;
using System.Collections.Generic;
using dook.tools.animatey;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Penguin : Throwable
{
    public Animatey GetUpAnim;
    private Quaternion fallRot;
    private Vector3 fallPos;

    public float walkDist = 5;
    public float walkSpeed = 1;
    public Vector2 nextMoveTime = new Vector2(2, 8);
    public float idleChance = 0.7f;

    private Vector3? walkGoal;
    private NavMeshAgent agent;

    public Animator ExlamAC;

    public bool Hungry { get; private set; }

    private AudioSource AS;
    private static readonly int Exclaim = Animator.StringToHash("Exclaim");

    private Collider[] playerCols = new Collider[2];

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        AS = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        GetUpAnim.Action += (val, startRot) =>
        {
            transform.rotation = Quaternion.Slerp(fallRot, Quaternion.identity, val);
            transform.position = Vector3.Lerp(transform.position, fallPos, val);

            if (val >= 1)
                agent.enabled = true;
        };

        agent.enabled = true; //Agent disabled by default to stop spawn shenannigins
        ChooseNextBehaviour();
    }

    protected override void Update()
    {
        base.Update();

        //look at nearest player
        if (Hungry)
        {
            var length = Physics.OverlapSphereNonAlloc(transform.position, 20, playerCols, 1 << 8);
            
            //Get closest
            float closestDist = 999;
            int closestIndex = -1;
            for (int i = 0; i < length; i++)
            {
                if(Vector3.Distance(transform.position, playerCols[i].transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(transform.position, playerCols[i].transform.position);
                    closestIndex = i;
                }
            }
            
            //look at closest
            if (closestIndex != -1)
            {
                var direction = playerCols[closestIndex].transform.position - transform.position;
                // direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    protected void FixedUpdate()
    {
        //Sit back up after being thrown if velocity is low enough
        if (thrown && RB.velocity.sqrMagnitude <= 1.0f)
        {
            print("Getting up");
            RB.isKinematic = true;
            fallRot = transform.rotation;
            GetUpAnim.Play(this);
            thrown = false;

            //Raycast to find the floor from current position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
            {
                fallPos = hit.point;
            }
        }
    }

    public override bool Use(Transform cameraRoot)
    {
        StopCoroutine(GetUpAnim.Routine());
        base.Use(cameraRoot);
        agent.enabled = false;
        return true; //Drop this bitch
    }

    private void ChooseNextBehaviour()
    {
        if (!pickedUp && !thrown && agent.enabled && !Hungry)
        {
            if (Random.value <= idleChance)
            {
                //Idle
                walkGoal = null;
                agent.SetDestination(transform.position);
                // agent.isStopped = true;
            }
            else
            {
                //Choose a random point to walk to
                var offset = Random.insideUnitCircle * walkDist;
                agent.enabled = true;
                agent.isStopped = false;
                walkGoal = transform.position + new Vector3(offset.x, 0, offset.y);
                agent.SetDestination(walkGoal.Value);
            }
        }

        Invoke(nameof(ChooseNextBehaviour), Random.Range(nextMoveTime.x, nextMoveTime.y));
    }

    public override void Pickup(Transform cameraRoot)
    {
        if (Hungry) return;
        base.Pickup(cameraRoot);
        agent.enabled = !pickedUp;
    }

    public void MakeHungry()
    {
        CancelInvoke(nameof(ChooseNextBehaviour));
        Hungry = true;
        agent.enabled = false;
        AS.Play();

        ExlamAC.gameObject.SetActive(true);
        ExlamAC.SetTrigger(Exclaim);
    }

    public void Feed()
    {
        Hungry = false;
        AS.Stop();
        ExlamAC.gameObject.SetActive(false);
        ChooseNextBehaviour();
    }
}