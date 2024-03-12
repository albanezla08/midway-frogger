using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileObject", order = 1)]
public class TileData : ScriptableObject
{
    public string tileName;

    public bool isWalkable;
    public GameObject prefab;
}