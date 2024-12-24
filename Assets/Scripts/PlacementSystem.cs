using System;
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


    private void Start()
    {
        StopPlacement();
        gridVisualization.SetActive(false);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }


    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.GetCellCenterWorld(gridPosition); // Hücrenin tam ortasýndan baþlar
        //cellIndicator.transform.position = grid.CellToWorld(gridPosition); // // Hücrenin Sol alt köþesinden baþlar

    }

    public void StartPlacement(int ID)
    {      
        selectedObjectIndex = dataBase.objects.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"ID yok {ID}");
            return;
        }
        Debug.Log("Objeye týklandi");
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
            Debug.Log("Pointer þu an UI üzerinde.");
            return;
        }
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(dataBase.objects[selectedObjectIndex].Prefab);
        Debug.Log("Obje oluþturuldu");
        newObject.transform.position = grid.GetCellCenterWorld(gridPosition); // Hücrenin tam ortasýndan baþlar
        //newObject.transform.position = grid.CellToWorld(gridPosition); // Hücrenin Sol alt köþesinden baþlar
    }


}
