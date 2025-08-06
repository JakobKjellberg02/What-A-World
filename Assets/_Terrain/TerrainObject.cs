using System;
using UnityEngine;



[CreateAssetMenu(fileName = "TerrainObject", menuName = "Scriptable Objects/TerrainObject")]


public class TerrainObject : ScriptableObject
{
    [Range(0f, 1.0f)] public float minimumNoise;
    [Range(0f, 1.0f)] public float maximumNoise;

    public Gradient terrainGradient;
    public bool passable;

}
