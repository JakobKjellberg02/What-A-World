using UnityEngine;

class HexStructure
{
    public GameObject hexGameObject;
    public int q;
    public int r;

    public HexStructure(GameObject hexGameObject, int q, int r)
    {
        this.hexGameObject = hexGameObject;
        this.q = q;
        this.r = r;
    }
}