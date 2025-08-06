using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;

    [SerializeField]
    private int gridRadius = 3;

    [SerializeField]
    private List<HexStructure> hexagonGrid = new();

    [SerializeField]
    private float noiseScale = 0.3f;
    [SerializeField]
    private List<TerrainObject> terrainTypes;

    private float seedOffsetX;
    private float seedOffsetY;

    private List<GameObject> activeNeighborMarkers = new();

    private Vector3 CalculationOfPosition(int q, int r)
    {
        float size = 1.0f;
        float x = size * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
        float z = size * (3f / 2f * r);
        return new Vector3(x, 0, z);
    }

    void Start()
    {
        seedOffsetX = UnityEngine.Random.Range(0f, 1000f);
        seedOffsetY = UnityEngine.Random.Range(0f, 1000f);
        for (int q = -gridRadius; q < gridRadius + 1; q++)
        {
            for (int r = -gridRadius; r < gridRadius + 1; r++)
            {
                int s = -q - r;
                if (s <= gridRadius && -gridRadius <= s)
                {
                    GameObject hexagon = Instantiate(hexagonPrefab);
                    hexagon.transform.position = CalculationOfPosition(q, r);
                    float noise = Mathf.PerlinNoise((q + seedOffsetX) * noiseScale, (r + seedOffsetY) * noiseScale);
                    TerrainObject terrain = GetTerrainByNoise(noise);
                    Color color = terrain.terrainGradient.Evaluate(noise);
                    hexagon.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
                    hexagon.GetComponent<HexAttributes>().Q = q;
                    hexagon.GetComponent<HexAttributes>().R = r;
                    HexStructure newHexStruct = new(hexagon, new Hex(q, r));
                    if (terrain.passable == true)
                    {
                        newHexStruct.passable = true;
                    }
                    hexagonGrid.Add(newHexStruct);
                }
            }
        }
    }

    public void Breadth_First_Search(Hex start)
    {
        foreach (GameObject neighbor in activeNeighborMarkers)
        {
            neighbor.transform.GetChild(0).gameObject.SetActive(false);
        }
        activeNeighborMarkers.Clear();
        Queue<Hex> frontier = new();
        frontier.Enqueue(start);

        HashSet<Vector2Int> reached = new()
        {
            start.GetAxialCoordinates()
        };

        while (frontier.Count > 0)
        {
            Hex current = frontier.Dequeue();

            for (int i = 0; i < 6; i++)
            {
                Hex potential = HexHelperFunctions.Hex_Neighbors(current, i);
                Vector2Int neighborCoords = potential.GetAxialCoordinates();
                if (reached.Contains(neighborCoords))
                {
                    continue;
                }
                HexStructure neighbor = hexagonGrid.Find(h => h.Q == neighborCoords.x && h.R == neighborCoords.y);
                if (neighbor != null && neighbor.passable)
                {
                    GameObject canidateGameObject = neighbor.hexView;
                    canidateGameObject.transform.GetChild(0).gameObject.SetActive(true);
                    activeNeighborMarkers.Add(canidateGameObject);

                    frontier.Enqueue(neighbor.hexData);
                    reached.Add(neighborCoords);
                }
            }
        }
    }

    private TerrainObject GetTerrainByNoise(float noise)
    {
        foreach (var terrain in terrainTypes)
        {
            if (noise >= terrain.minimumNoise && noise <= terrain.maximumNoise)
            {
                return terrain;
            }
        }

        Debug.LogWarning("No terrain matched noise value. Returning default.");
        return terrainTypes[0]; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedHex = hit.collider.gameObject;
                if (clickedHex.TryGetComponent<HexAttributes>(out var hexAttr))
                {
                    Hex startHex = new(hexAttr.Q, hexAttr.R);
                    Breadth_First_Search(startHex);
                }
            }
        }
    }

}
