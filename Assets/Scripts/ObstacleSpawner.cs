using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] protected TilesManager tilesManager;
    [SerializeField] protected GameObject[] obstaclePrefabs;
    [SerializeField] protected float obstacleSpeed;
    [SerializeField] protected Vector3 spawnPos;
    [SerializeField] protected int spawnRow;
    [SerializeField] protected float minSpawnDelay = 4;
    [SerializeField] protected float maxSpawnDelay = 8;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Vector3 tileSize = tilesManager.getTileSize();
        Vector3 tilesCount = tilesManager.getTileCount();
        float xPos = tilesManager.getTopRight().x + 1;
        float yPos = tilesManager.getBottomLeft().y + tileSize.y * ((float)spawnRow + 0.5f);
        spawnPos = new Vector3(xPos, yPos);
    }
}
