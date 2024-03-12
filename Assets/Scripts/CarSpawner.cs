using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : ObstacleSpawner
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        InitCars();
    }

    void InitCars()
    {
        float screenWidth = tilesManager.getTopRight().x*2;
        // int numCars = (int)(screenWidth/minWait + 0.5);
        int numCars = 5; // couldnt figure out the math so I just made it 5
        float carDistance = screenWidth/numCars;
        float rightMostX = tilesManager.getTopRight().x;
        for (int i = 0; i < numCars; i++)
        {
            Vector3 spawnOffset = new Vector3(rightMostX - carDistance * i, spawnPos.y);
            GameObject car = Instantiate(obstaclePrefabs[0], spawnOffset, Quaternion.identity);
            car.GetComponent<CarController>().setSpeed(obstacleSpeed);
        }
        StartCoroutine(SpawnAfter(Random.Range(minSpawnDelay, maxSpawnDelay)));
    }

    IEnumerator SpawnAfter(float wait) {
        yield return new WaitForSeconds(wait);
        GameObject car = Instantiate(ChooseCarPrefab(), spawnPos, Quaternion.identity);
        car.GetComponent<CarController>().setSpeed(obstacleSpeed);

        // Start next car spawn timer
        float nextWait = Random.Range(minSpawnDelay, maxSpawnDelay);
        StartCoroutine(SpawnAfter(nextWait));
    }

    GameObject ChooseCarPrefab()
    {
        return obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
    }
}
