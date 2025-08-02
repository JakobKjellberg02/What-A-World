using UnityEngine;



public class HexAttributes : MonoBehaviour
{
    [Header("Hex Coordinates")]
    [SerializeField]
    private int q;
    [SerializeField]
    private int r;

    public int Q
    {
        get { return q; }
        set { q = value; }
    }

    public int R
    {
        get { return r; }
        set { r = value; }
    }

}

