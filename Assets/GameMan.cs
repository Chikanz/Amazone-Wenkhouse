using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public TextMeshProUGUI WinText;
    
    Vector3[] PlayerPositions = new Vector3[2];
    Quaternion[] PlayerRotations = new Quaternion[2];
    private Player[] players;
    private JEFF[] jeffs;

    // Start is called before the first frame update
    void Start()
    {
        //Get all jeff classes
        jeffs = FindObjectsOfType<JEFF>();
        
        //Loop through all jeffs
        foreach(JEFF jeff in jeffs)
        {
            jeff.OnRoundFinish += OnRoundFinish;
        }
        
        
        //Get all players
        players = FindObjectsOfType<Player>();
        
        //Log player starts 
        for (var i = 0; i < players.Length; i++)
        {
            var player = players[i];
            PlayerPositions[i] = player.transform.position;
            PlayerRotations[i] = player.transform.rotation;
        }
    }

    //Takes the player who died and resets the round
    private void OnRoundFinish(int loserID)
    {
        print("Round finished");
        WinText.text = "Player " + (loserID + 1) + " was fired!";
        Invoke(nameof(ResetRound), 3);
        
        foreach(JEFF jeff in jeffs)
        {
            jeff.countDown = false;
        }
    }

    private void ResetRound()
    {
        print("Reste round");
        //Destroy all penguins
        var penguins = FindObjectsOfType<Penguin>();
        foreach (var penguin in penguins)
        {
            Destroy(penguin.gameObject);
        }
        
        //Spawn more pepnguins
        FindObjectsOfType<PenguinSpawner>()[0].Spawn();
        
        //Reset player positions 
        for (var i = 0; i < players.Length; i++)
        {
            var player = players[i];
            player.transform.position = PlayerPositions[i];
            player.transform.rotation = PlayerRotations[i];
        }

        //Start countdowns
        foreach(JEFF jeff in jeffs)
        {
            jeff.Reset(15);
        }
        
        WinText.text = "";
        
        //Destroy all objects with fish tag
        var fish = GameObject.FindGameObjectsWithTag("Fish");
        foreach (var f in fish)
        {
            Destroy(f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
