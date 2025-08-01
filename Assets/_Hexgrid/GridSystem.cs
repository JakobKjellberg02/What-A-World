using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;
    public List<GameObject> hexagonGrid = new();

    private UnityEngine.Vector3 CalculationOfPosition(int x, int z)
    {
        float width = (float)Math.Sqrt(3);
        float height = 3.0f/2.0f;

        if (z % 2 == 1)
        {
            return new UnityEngine.Vector3(x * width + (width / 2), 0.0f, z * height);
        }
        else
        {
            return new UnityEngine.Vector3(x * width, 0.0f, z * height);
        }
    }

    void Start()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                GameObject hexagon = Instantiate(hexagonPrefab);
                hexagon.transform.position = CalculationOfPosition(x, z);
                if (z % 2 == 1)
                {
                    hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.blue);
                }
                else
                {
                    hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                }
                hexagonGrid.Add(hexagon);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
