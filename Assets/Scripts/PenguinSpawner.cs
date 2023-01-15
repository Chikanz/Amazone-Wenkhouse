using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PenguinSpawner : MonoBehaviour
{
    public float radius = 50;
    public GameObject PengObj;

    public int penguinCount = 100;

    // Start is called before the first frame update
    void Awake()
    {
        //Spawn all the penguins
        for (int i = 0; i < penguinCount; i++)
        {
            //Set the position of the penguin
            var pos = Random.insideUnitSphere * radius;
            pos.y = 0;
            Debug.DrawLine(pos, pos + Vector3.up, Color.red);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
            {
                //Spawn a penguin
                GameObject penguin = Instantiate(PengObj);
                penguin.transform.position = hit.position;
                penguin.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)); 
                Debug.DrawLine(hit.position, hit.position + Vector3.up, Color.green);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}