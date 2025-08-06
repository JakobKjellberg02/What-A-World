using UnityEngine;

public class Hex
{
    public int Q { get; }
    public int R { get; }

    public Hex(int q, int r)
    {
        Q = q;
        R = r;
    }
    
    public Vector2Int GetAxialCoordinates() => new Vector2Int(Q, R);
}