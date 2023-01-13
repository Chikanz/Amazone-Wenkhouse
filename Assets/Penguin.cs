using System.Collections;
using System.Collections.Generic;
using dook.tools.animatey;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

        agent = GetComponent<NavMeshAgent>();

        ChooseNextBehaviour();
    }

    protected override void Update()
    {
        base.Update();
        
        // If we're not falling, we can walk
        // if (walkGoal != null )
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, walkGoal.Value, walkSpeed * Time.deltaTime);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(walkGoal.Value - transform.position), 0.1f);
        // }

        // If we're close enough to our goal, choose a new one
        if (agent.hasPath && agent.remainingDistance < 0.5f)
        {
            CancelInvoke(nameof(ChooseNextBehaviour));
            ChooseNextBehaviour();
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

    public override void Use(Transform cameraRoot)
    {
        StopCoroutine(GetUpAnim.Routine());
        base.Use(cameraRoot);
        agent.enabled = false;
    }

    private void ChooseNextBehaviour()
    {
        if (!pickedUp && !thrown && agent.enabled)
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
        base.Pickup(cameraRoot);
        agent.enabled = !pickedUp;
    }
}