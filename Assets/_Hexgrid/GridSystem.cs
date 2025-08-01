using System;
using System.Collections.Generic;
using UnityEngine;


public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;
    public GameObject selectPrefab;

    public int gridRadius = 3;

    [HideInInspector]
    public GameObject currentlySelected = null;

    [HideInInspector]
    public List<GameObject> hexagonGrid = new();

    private GameObject selectionMarker;
    private Vector3 CalculationOfPosition(int q, int r)
    {
        float size = 1.0f; 
        float x = size * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
        float z = size * (3f / 2f * r);
        return new Vector3(x, 0, z);
    }

    void Start()
    {
        for (int q = -gridRadius; q < gridRadius + 1; q++)
        {
            for (int r = -gridRadius; r < gridRadius + 1; r++)
            {
                int s = -q - r;
                if (s <= gridRadius && -gridRadius <= s)
                {
                    GameObject hexagon = Instantiate(hexagonPrefab);
                    hexagon.transform.position = CalculationOfPosition(q, r);
                    if (q % 2 == 1)
                    {
                        hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.blue);
                    }
                    else
                    {
                        hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                    }
                    hexagon.GetComponent<HexAttributes>().q = q;
                    hexagon.GetComponent<HexAttributes>().r = r;
                    hexagonGrid.Add(hexagon);
                }
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
