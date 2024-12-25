using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placementObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placementObjectIndex)
    {
        List<Vector3Int> positionOccupy = CalculatePosition(gridPosition, objectSize);
        PlacementData data = new (positionOccupy, ID, placementObjectIndex);
        foreach (var pos in positionOccupy)
        {
            if (placementObjects.ContainsKey(pos))
                throw new Exception("Hücre dolu: " + pos);

            placementObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnValue = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnValue.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnValue;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupied = CalculatePosition(gridPosition, objectSize);
        foreach (var pos in positionToOccupied)
        {
            if (placementObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPosition;


    public int ID { get; private set; }

    public int PlacementObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPosition, int ýD, int placementObjectIndex)
    {
        this.occupiedPosition = occupiedPosition;
        ID = ýD;
        PlacementObjectIndex = placementObjectIndex;
    }
}