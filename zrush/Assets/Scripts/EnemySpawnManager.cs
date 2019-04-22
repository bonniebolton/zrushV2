using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // used to keep track of when the next spawn will occur
    private float nextTimeToSpawn = 0f;
    //float used to change firerate
    public float SpawnRate = .125f;

    [HideInInspector]
    public int LevelLocation = 1;

    //array storing spawn points
    public Transform[] Level1SpawnPoints;
    //used to select spawn point
    private int currentSpawnPoint = 0;
    

    // Update is called once per frame
    void Update()
    {
        //if the player is in level 1 and the spawn timer ends
        if (Time.time >= nextTimeToSpawn)
        {
            //resets the spawn timer 
            //the higher SpawnRate is, the quicker the spawns happen
            nextTimeToSpawn = Time.time + 1f / SpawnRate;
            //calls spawn function
            Spawn();
        }
    }

    void Spawn()
    {

        //if the player is in level 1
        if(LevelLocation == 1)
        {

            int i = currentSpawnPoint;

            //creates a zombie at the next level 1 spawn point
            Instantiate(Resources.Load("Zombie"), Level1SpawnPoints[i].position, Level1SpawnPoints[i].rotation);

            //moves to next spawn point 
            currentSpawnPoint++;
            //if the next spawn point is not in the array 
            if (currentSpawnPoint > (Level1SpawnPoints.Length - 1))
            {
                //go back to the first spawn point
                currentSpawnPoint = 0;
            }
        }
    }
}
