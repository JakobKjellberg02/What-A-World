using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;
    public GameObject selectPrefab;

    [SerializeField]
    private int gridRadius = 3;

    [SerializeField]
    private List<HexStructure> hexagonGrid = new();

    private GameObject selectionMarker;
    private Vector3 CalculationOfPosition(int q, int r)
    {
        float size = 1.0f;
        float x = size * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
        float z = size * (3f / 2f * r);
        return new Vector3(x, 0, z);
    }

    private readonly Vector2Int[] hexDirections = new Vector2Int[]
    {
        new(1, 0),   
        new(1, -1),   
        new(0, -1),  
        new(-1, 0),  
        new(-1, 1),  
        new(0, 1)    
    };

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
                    hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.blue);
                    hexagon.GetComponent<HexAttributes>().Q = q;
                    hexagon.GetComponent<HexAttributes>().R = r;
                    hexagonGrid.Add(new HexStructure(hexagon, q, r));
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
        if (selectionMarker == null)
        {
            selectionMarker = Instantiate(selectPrefab, newSelection.transform.position, Quaternion.Euler(-90, 0, 0));
            int selectedQ = newSelection.GetComponent<HexAttributes>().Q;
            int selectedR = newSelection.GetComponent<HexAttributes>().R;

            foreach (Vector2Int direction in hexDirections)
            {
                int neighborQ = selectedQ + direction.x;
                int neighborR = selectedR + direction.y;
                HexStructure candidate = hexagonGrid.Find(x => x.q == neighborQ && x.r == neighborR);
                if (candidate != null)
                {
                    GameObject canidateGameObject = candidate.hexGameObject;
                    canidateGameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                }
            }
        }
        else
        {
            selectionMarker.transform.position = newSelection.transform.position;
        }
    }


}
