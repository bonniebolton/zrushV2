using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    // used to keep track of when the next spawn will occur
    private float nextTimeToSpawn = 0f;
    //float used to change spawnrate
    public float ResourceSpawnRate = .1f;

    //used to reference the Level Location variable
    EnemySpawnManager eSpawn;

    //array storing spawn points
    public Transform[] Level1ResourceSpawnPoints;
    public Transform[] Level2ResourceSpawnPoints;
    public Transform[] Level3ResourceSpawnPoints;

    private string[] ResourceDrops;

    private void Awake()
    {
        //creates string array that will be used to pick one of these two pick ups randomly to be instantiated
        ResourceDrops = new string[]
        {
            "steelPickup",
            "woodPickup",
        };
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is in level 1 and the spawn timer ends
        if (Time.time >= nextTimeToSpawn)
        {
            //resets the spawn timer 
            //the higher SpawnRate is, the quicker the spawns happen
            nextTimeToSpawn = Time.time + 1f / ResourceSpawnRate;
            //calls spawn function
            SpawnDrop();
        }
    }

    void SpawnDrop()
    {

        //if the player is in level 1
        if (eSpawn.LevelLocation == 1)
        {

            //picks random spawn point
            int i = Random.Range(0, Level1ResourceSpawnPoints.Length - 1);

            //creates a zombie at the next level 1 spawn point
            Instantiate(Resources.Load(PickRandomResource()), Level1ResourceSpawnPoints[i].position, Level1ResourceSpawnPoints[i].rotation);
            
        }

        //if the player is in level 2
        if (eSpawn.LevelLocation == 2)
        {

            //picks random spawn point
            int i = Random.Range(0, Level2ResourceSpawnPoints.Length - 1);

            //creates a zombie at the next level 1 spawn point
            Instantiate(Resources.Load(PickRandomResource()), Level2ResourceSpawnPoints[i].position, Level2ResourceSpawnPoints[i].rotation);
            
        }

        //if the player is in level 3
        if (eSpawn.LevelLocation == 3)
        {
            //nothing spawns
            return;

        }
    }

    //picks a random resource from the array and returns a string
    public string PickRandomResource()
    {
        return ResourceDrops[Random.Range(0, ResourceDrops.Length - 1)];
    }
}
