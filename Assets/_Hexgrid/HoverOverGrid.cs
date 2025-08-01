using UnityEngine;

public class HoverOverGrid : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
    }

    void Update()
    {
        
    }
}
