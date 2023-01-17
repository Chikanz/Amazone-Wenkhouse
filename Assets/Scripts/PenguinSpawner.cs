using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PenguinSpawner : MonoBehaviour
{
    public float radius = 50;
    public GameObject PengObj;

    public int penguinCount = 100;

    // Start is called before the first frame update
    void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        //Spawn all the penguins
        for (int i = 0; i < penguinCount; i++)
        {
            //Set the position of the penguin
            var posCirc = Random.insideUnitCircle * radius;
            var pos = new Vector3(posCirc.x, 99, posCirc.y);
            
            //Raycast down onto level
            var ray = new Ray(pos, Vector3.down);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                pos = hitInfo.point;
            }
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Spawn a penguin
                // Debug.DrawLine( hitInfo.point + Vector3.up * 10,  pos, Color.green, 99999);
                GameObject penguin = Instantiate(PengObj);
                penguin.transform.position = hit.position;
                penguin.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)); 
            }
            else
            {
                print("No NavMesh");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}