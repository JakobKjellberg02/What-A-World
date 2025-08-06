using System;
using UnityEngine;

public class HexHelperFunctions
{

    #region "Neighbors"
    public static readonly Vector2Int[] hexDirectionVectors = new Vector2Int[]
    {
        new(1, 0),
        new(1, -1),
        new(0, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, 1)
    };

    public static Vector2Int Hex_Direction(int direction)
    {
        return hexDirectionVectors[direction];
    }

    public static Hex Hex_Add(Hex hex, Vector2Int vector)
    {
        return new Hex(hex.Q + vector.x, hex.R + vector.y);
    }

    public static Hex Hex_Neighbors(Hex hex, int direction)
    {
        return Hex_Add(hex, Hex_Direction(direction));
    }
    #endregion

    #region "Conversion between Hex and cube"
    public Cube Axial_to_cube(Hex hex)
    {
        int q = hex.Q;
        int r = hex.R;
        int s = -q - r;
        return new Cube(q, r, s);
    }

    public Hex Cube_to_axial(Cube cube)
    {
        int q = cube.Q;
        int r = cube.R;
        return new Hex(q, r);
    }
    #endregion

    #region "Axial distance"
    public int Axial_distance(Hex a, Hex b)
    {
        return (Math.Abs(a.Q - b.Q) + Math.Abs(a.Q + a.R - b.Q - b.R) + Math.Abs(a.R - b.R)) / 2;
    }
    #endregion

}
