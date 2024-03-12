using UnityEngine;

[CreateAssetMenu(fileName = "TilePattern", menuName = "ScriptableObjects/PatternObject", order = 1)]
public class PatternData : ScriptableObject
{
    public GameObject[] tileOrder;
    public int[] tiles;
}