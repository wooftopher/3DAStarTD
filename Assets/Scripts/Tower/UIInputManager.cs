using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIInputManager : MonoBehaviour {
    // make wall and tower parent
    public enum TowerType {
        Range = 1,
        Melee = 2,
        Net = 3
    }
    private WallPlacement wallPlacement;
    public event Action<BaseTower> OnSellTower;
    public event Action<Wall> OnSellWall;
    public event Action<Wall, int> OnPlaceTowerOnWall;
    [SerializeField] private Button sellButtonTower;
    [SerializeField] private Button sellButtonWall;
    [SerializeField] private Button rangeTowerButton;
    [SerializeField] private Button meleeTowerButton;
    [SerializeField] private Button netTowerButton;

    private void Awake(){
        wallPlacement = GetComponent<WallPlacement>();
    }
    void Start(){
        sellButtonTower.onClick.AddListener(OnSellButtonTowerPressed);
        sellButtonWall.onClick.AddListener(OnSellButtonWallPressed);
        rangeTowerButton.onClick.AddListener(() => OnTowerButtonPressed(TowerType.Range));
        meleeTowerButton.onClick.AddListener(() => OnTowerButtonPressed(TowerType.Melee));
        netTowerButton.onClick.AddListener(() => OnTowerButtonPressed(TowerType.Net));
    }
    private void OnSellButtonWallPressed() {
        if(wallPlacement.GetSelectedWall())
            OnSellWall?.Invoke(wallPlacement.GetSelectedWall());
    }
    private void OnSellButtonTowerPressed() {
        if (wallPlacement.GetSelectedTower())
            OnSellTower?.Invoke(wallPlacement.GetSelectedTower());
    }

    private void OnTowerButtonPressed(TowerType towerType) {
    // Get the last selected ISelectable object
    ISelectable selectedObject = wallPlacement.GetLastSelectedSelectable();

    if (selectedObject != null) {
        if (selectedObject is Wall selectedWall) {
            // Now we know it's a Wall, so we can proceed with the wall-specific logic
            if (selectedWall.towerObject == null) {
                OnPlaceTowerOnWall?.Invoke(selectedWall, (int)towerType);
            } else {
                Debug.LogWarning("Tower already exists on the selected wall!");
            }
        } else {
            Debug.LogWarning("Selected object is not a wall! Cannot place tower.");
        }
    } else {
        Debug.LogWarning("No object selected. Please select a wall before placing a tower.");
    }
}

}
