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
        GameObject newObject = Instantiate(dataBase.objects[selectedObjectIndex].Prefab);
        Debug.Log("Obje olu�turuldu");
        newObject.transform.position = grid.GetCellCenterWorld(gridPosition); // H�crenin tam ortas�ndan ba�lar
        //newObject.transform.position = grid.CellToWorld(gridPosition); // H�crenin Sol alt k��esinden ba�lar
    }


}
