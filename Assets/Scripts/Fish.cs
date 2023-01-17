using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public EventHandler OnFed;

    private

        // Start is called before the first frame update
        void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Penguin"))
        {
            if (collision.gameObject.GetComponent<Penguin>().Hungry)
            {
                OnFed?.Invoke(this, EventArgs.Empty);
                collision.gameObject.GetComponent<Penguin>().Feed();
                Destroy(gameObject);
            }
        }
    }
}