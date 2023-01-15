using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Causes chaos
public class ChaosMonkey : MonoBehaviour
{
    public Vector2 penguinHungryRate = new Vector2(5, 10);
    public int maxHungry = 5;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(HungerPenguins), Random.Range(0.0f, 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HungerPenguins()
    {
        //Get all penguins
        var PenguinsGOs = GameObject.FindGameObjectsWithTag("Penguin");

        //find how many are hungry
        int hungryPenguins = 0;
        foreach (var penguin in PenguinsGOs)
        {
            if (penguin.GetComponent<Penguin>().Hungry)
            {
                hungryPenguins++;
            }
        }

        if (hungryPenguins < maxHungry)
        {
            PenguinsGOs[Random.Range(0, PenguinsGOs.Length)].GetComponent<Penguin>().MakeHungry();
            Invoke(nameof(HungerPenguins), Random.Range(penguinHungryRate.x, penguinHungryRate.y));
        }
    }
}