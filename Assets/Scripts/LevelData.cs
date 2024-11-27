using UnityEngine;
using System;

[Serializable]
public struct DotPair
{
    public Vector2Int dot1Position;
    public Vector2Int dot2Position;
    public int colorIndex; // Color index from color array in GridManager
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Flow/Level Data")]
public class LevelData : ScriptableObject
{
    public int gridWidth = 5;
    public int gridHeight = 5;
    public DotPair[] dotPairs;
}