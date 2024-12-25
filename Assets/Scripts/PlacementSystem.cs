using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    InputManager inputManager;

    [SerializeField]
    Grid grid;

    [SerializeField]
    SO dataBase;
    int selectedObjectIndex = -1;

    [SerializeField]
    GameObject gridVisualization;

    GridData florData, furnitureData;

    Renderer previewRenderer;

    List<GameObject> placedGameObject = new();

    private void Start()
    {
        StopPlacement();
        gridVisualization.SetActive(false);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;


        florData = new();
        furnitureData = new();
        previewRenderer = cellIndicator.GetComponent<Renderer>();
    }


    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.GetCellCenterWorld(gridPosition); // H�crenin tam ortas�ndan ba�lar
        //cellIndicator.transform.position = grid.CellToWorld(gridPosition); // // H�crenin Sol alt k��esinden ba�lar

    }

    public void StartPlacement(int ID)
    {      
        selectedObjectIndex = dataBase.objects.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"ID yok {ID}");
            return;
        }
        Debug.Log("Objeye t�klandi");
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            Debug.Log("Pointer �u an UI �zerinde.");
            return;
        }
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(dataBase.objects[selectedObjectIndex].Prefab);
        Debug.Log("Obje olu�turuldu");
        //newObject.transform.position = grid.GetCellCenterWorld(gridPosition); // H�crenin tam ortas�ndan ba�lar
        newObject.transform.position = grid.CellToWorld(gridPosition); // H�crenin Sol alt k��esinden ba�lar

        placedGameObject.Add(newObject);
        GridData selectedData = dataBase.objects[selectedObjectIndex].ID == 0 ? florData: furnitureData;

        selectedData.AddObjectAt(gridPosition, 
        dataBase.objects[selectedObjectIndex].Size,
        dataBase.objects[selectedObjectIndex].ID,
        placedGameObject.Count - 1);
        
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = dataBase.objects[selectedObjectIndex].ID == 0 ? florData: furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, dataBase.objects[selectedObjectIndex].Size);
    }
}
