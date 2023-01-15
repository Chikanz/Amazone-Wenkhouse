using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Interactable
{
    public GameObject fish;
    public float fishForce = 500;

    public override void Use(Transform cameraRoot)
    {
        base.Use(cameraRoot);
        //Spawn a fish + throw
        GameObject newFish = Instantiate(fish, cameraRoot.position + cameraRoot.forward, Quaternion.identity);
        newFish.GetComponent<Rigidbody>().AddForce(cameraRoot.forward * fishForce);
        newFish.transform.rotation = Random.rotation;
        newFish.GetComponent<Fish>().OnFed += (sender, args) =>
        {
            FinishedTask();
        };
        Destroy(newFish,20);
    }
}
