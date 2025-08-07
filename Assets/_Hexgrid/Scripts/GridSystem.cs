using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class GridSystem : MonoBehaviour
{
    // References
    public GameObject hexagonPrefab;

    public GameObject boatPrefab;
    private GameObject boatInstance;

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

    private readonly List<GameObject> activeNeighborMarkers = new();
  
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
        HexStructure spawnTile = hexagonGrid.Find(h => h.passable); 
        if (spawnTile != null)
        {
            boatInstance = Instantiate(boatPrefab);
            boatInstance.transform.position = spawnTile.hexView.transform.position;
            boatInstance.GetComponent<Boat>().currentHex = spawnTile.hexData;
        }
    }

    public List<Hex> FindPathForBoat(Hex start, Hex end)
    {
        foreach (GameObject neighbor in activeNeighborMarkers)
        {
            neighbor.transform.GetChild(0).gameObject.SetActive(false);
        }
        activeNeighborMarkers.Clear();

        Queue<Hex> frontier = new();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int?> cameFrom = new()
        {
            [start.GetAxialCoordinates()] = null
        };

        while (frontier.Count > 0)
        {
            Hex current = frontier.Dequeue();
            Vector2Int currentCoords = current.GetAxialCoordinates();

            if (currentCoords == end.GetAxialCoordinates())
            {
                break;
            }

            for (int i = 0; i < 6; i++)
            {
                Hex potential = HexHelperFunctions.Hex_Neighbors(current, i);
                Vector2Int neighborCoords = potential.GetAxialCoordinates();
                if (cameFrom.ContainsKey(neighborCoords))
                {
                    continue;
                }
                HexStructure neighbor = hexagonGrid.Find(h => h.Q == neighborCoords.x && h.R == neighborCoords.y);
                if (neighbor != null && neighbor.passable)
                {
                    frontier.Enqueue(neighbor.hexData);
                    cameFrom[neighborCoords] = currentCoords;
                }
            }
        }

        List<Hex> path = new();
        Vector2Int currentStep = end.GetAxialCoordinates();
        if (!cameFrom.ContainsKey(currentStep))
        {
            return null;
        }

        while (currentStep != start.GetAxialCoordinates())
        {
            path.Add(new Hex(currentStep.x, currentStep.y));
            HexStructure stepStruct = hexagonGrid.Find(h => h.Q == currentStep.x && h.R == currentStep.y);
            if (stepStruct != null)
            {
                GameObject marker = stepStruct.hexView;
                marker.transform.GetChild(0).gameObject.SetActive(true);
                activeNeighborMarkers.Add(marker);
            }
            currentStep = cameFrom[currentStep].Value;
        }

        path.Reverse();
        return path;
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
        return terrainTypes[0];
    }

    public HexStructure GetHexStructure(Hex hex)
    {
        return hexagonGrid.Find(h => h.Q == hex.Q && h.R == hex.R);
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
                    Vector2Int coords = new(hexAttr.Q, hexAttr.R);
                    Hex targetHex = new(coords.x, coords.y);
                    HexStructure targetStruct = GetHexStructure(targetHex);

                    if (targetStruct != null && targetStruct.passable && boatInstance != null)
                    {
                        Boat boat = boatInstance.GetComponent<Boat>();
                        List<Hex> path = FindPathForBoat(boat.currentHex, targetHex);
                        if (path != null)
                        {
                            boat.movementOfBoat(path, this);
                            boat.currentHex = targetHex;
                        }
                    }
                }
            }
        }
    }
}

