using UnityEngine;

public class HexStructure
{
    public GameObject hexView;
    public Hex hexData;

    public int Q => hexData.Q;
    public int R => hexData.R;

    public bool passable = false;

    public HexStructure(GameObject hexView, Hex hexData)
    {
        this.hexView = hexView;
        this.hexData = hexData;
    }

}