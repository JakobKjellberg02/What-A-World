using System;
using System.Collections.Generic;
using UnityEngine;


public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;
    public GameObject selectPrefab;

    [HideInInspector]
    public GameObject currentlySelected = null;

    public List<GameObject> hexagonGrid = new();

    private GameObject selectionMarker;
    private Vector3 CalculationOfPosition(int x, int z)
    {
        float width = (float)Math.Sqrt(3);
        float height = 3.0f / 2.0f;

        if (z % 2 == 1)
        {
            return new Vector3(x * width + (width / 2), 0.0f, z * height);
        }
        else
        {
            return new Vector3(x * width, 0.0f, z * height);
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedHex = hit.collider.gameObject;
                SelectHex(clickedHex);
            }
        }
    }

    private void SelectHex(GameObject newSelection)
    {
    currentlySelected = newSelection;

    if (selectionMarker == null)
    {
        selectionMarker = Instantiate(selectPrefab, newSelection.transform.position, Quaternion.Euler(-90, 0, 0));
    }
    else
    {
        selectionMarker.transform.position = newSelection.transform.position;
    }
    }
}
