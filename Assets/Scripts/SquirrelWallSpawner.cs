using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWallSpawner : ObstacleSpawner
{
    [SerializeField] float minWallDuration;
    [SerializeField] float maxWallDuration;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        StartCoroutine(SpawnWallAfter(2));
    }

    IEnumerator SpawnWallAfter(float wait) {
        yield return new WaitForSeconds(wait);
        float wallDuration = Random.Range(minWallDuration, maxWallDuration);
        PutWall(wallDuration);

        // Start next car spawn timer
        float nextWait = Random.Range(minSpawnDelay, maxSpawnDelay);
        StartCoroutine(SpawnWallAfter(wallDuration + nextWait));
    }

    private void PutWall(float wallDuration)
    {
        float squirrelSize = obstaclePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.max.x*2;
        float spawnWaitTime = squirrelSize/obstacleSpeed;
        // Debug.Log(spawnWaitTime);
        Coroutine wallRoutine = StartCoroutine(KeepWall(spawnWaitTime, wallDuration));
        StartCoroutine(StopWallAfter(wallDuration, wallRoutine));
    }
    private IEnumerator KeepWall(float spawnWaitTime, float duration)
    {
        while (true)
        {
            GameObject squirrel = Instantiate(obstaclePrefabs[0], spawnPos, Quaternion.identity);
            squirrel.GetComponent<CarController>().setSpeed(obstacleSpeed);
            yield return new WaitForSeconds(spawnWaitTime);
        }
    }
    private IEnumerator StopWallAfter(float stopWaitTime, Coroutine wallRoutine)
    {
        yield return new WaitForSeconds(stopWaitTime);
        StopCoroutine(wallRoutine);
        // Debug.Log("Stopped wall");
    }
}
