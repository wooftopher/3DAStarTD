using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WallPlacement : MonoBehaviour {

    private VisualizerManager visualizerManager;
    private UIInputManager uIInputManager;

    public GameObject wallPrefab;
    public GameObject towerPrefabMelee;
    public GameObject towerPrefabRange;
    public GameObject towerPrefabNet;

    public Transform spawner;
    public Transform goal;

    private InfoUI infoUI;
    private Player player;

    private int currentBuildMode;
    private int wallLayerMask;
    private int towerLayerMask;

    private Wall lastHoveredWall;
    public Wall GetLastHoveredWall(){
        return lastHoveredWall;
    }
    // public void SetLastHoveredWall(Wall lastHoveredWall){
    //     this.lastHoveredWall = lastHoveredWall;
    // }
    private BaseTower lasthoveredTower;
    public BaseTower GetLastHoveredTower(){
        return lasthoveredTower;
    }

    private Wall selectedWall;

    public void SetSelectedWall(Wall selectedWall){
        this.selectedWall = selectedWall;
    }

    private BaseTower selectedTower;
    public BaseTower GetSelectedTower(){
        return selectedTower;
    }
    public Wall GetSelectedWall(){
        return selectedWall;
    }

    private ISelectable lastSelectedSelectable;
    private ISelectable lastHoveredSelectable;

    public ISelectable GetLastSelectedSelectable(){
        return lastSelectedSelectable;
    }
    public ISelectable GetSelectedSelectable() {
        return lastSelectedSelectable;
    }

    public void SetLastSelectedSelectable(ISelectable lastSelectedSelectable){
        this.lastSelectedSelectable = lastSelectedSelectable;
    }
    public void SetLastHoveredSelectable(ISelectable lastHoveredSelectable) {
        this.lastHoveredSelectable = lastHoveredSelectable;
    }

    private void Awake(){
        visualizerManager = GetComponent<VisualizerManager>();
        uIInputManager = GetComponent<UIInputManager>();
        uIInputManager.OnSellTower += SellTower;
        uIInputManager.OnSellWall += SellWall;
        uIInputManager.OnPlaceTowerOnWall += PlaceTowerOnWall;
    }
    void Start(){
        wallLayerMask = LayerMask.GetMask("Wall");
        towerLayerMask = LayerMask.GetMask("Tower");
        infoUI = FindObjectOfType<InfoUI>();
        player = FindObjectOfType<Player>();
    }

    private void SellTower(BaseTower towerToSell) {
        player.EarnGold(towerToSell.GetTowerData().price);
        Destroy(towerToSell.gameObject);
        selectedTower = null;
        infoUI.ShowEmptyPanel();
    }
    private void SellWall(Wall wallToSell) {
        player.EarnIceBlock(1);
        wallToSell.node.walkable = true;
        wallToSell.node.isBuildable = true;
        Destroy(wallToSell.gameObject);
        selectedWall = null;
        infoUI.ShowEmptyPanel();
    }

    public void SetBuildMode(int mode) {
        currentBuildMode = mode;
        // Add logic here to handle different build modes based on currentBuildMode
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            infoUI.ShowEmptyPanel();
            if (lastHoveredSelectable != null) {
                lastHoveredSelectable.ResetColor();
            }
            if (lastSelectedSelectable != null) {
                lastSelectedSelectable.ResetColor(); 
            }
            // Clear the selected and hovered selectables
            lastSelectedSelectable = null;
            lastHoveredSelectable = null;
        }
        if (currentBuildMode != 2){}
            visualizerManager.GetNodeSelectionVisualizer().HideVisualizer();
        if (currentBuildMode == 1) {
            BaseTower towerUnderMouse = GetTowerUnderMouse();///<---please help
            if (towerUnderMouse) {
                HighlightTower(towerUnderMouse);

                if (Input.GetMouseButtonDown(0)) { // Left mouse button
                    towerUnderMouse.Select();
                }
            } else if (lasthoveredTower != null && lasthoveredTower != selectedTower) {
                // If no tower is under the mouse, reset the highlight for the previously hovered tower
                lasthoveredTower.ResetColor();
                lasthoveredTower = null; // Clear the reference to the hovered tower
            }

            // Highlight walls
            Wall wallUnderMouse = GetWallUnderMouse();
            if (wallUnderMouse) {
                HighlightWall(wallUnderMouse);
                if (Input.GetMouseButtonDown(0)) {
                    wallUnderMouse.Select();
                }
            } else if (lastHoveredWall != null && lastHoveredWall != selectedWall ) {
                lastHoveredWall.ResetColor();
                lastHoveredWall = null; // Clear the last hovered wall
            }
        }

        else if (currentBuildMode == 2) {
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);

            if (nodeUnderMouse != null) {
                // Check if there is already a wall at the node
                if (nodeUnderMouse.wallObject) { // Assuming Node has a method HasWall
                    visualizerManager.GetNodeSelectionVisualizer().HideVisualizer(); // Hide visualizer if a wall is present
                } else {
                    visualizerManager.GetNodeSelectionVisualizer().ShowNodeVisual(nodeUnderMouse);
                    if (Input.GetMouseButtonDown(0)) { // Left mouse button
                        PlaceWallAtNode(nodeUnderMouse);
                    }
                }
            }
            else {
                // Optional: Hide the visualizer if the node is out of bounds
                visualizerManager.GetNodeSelectionVisualizer().HideVisualizer();
            }
        }

    }
    // void Update() {
    //     if (Input.GetKeyDown(KeyCode.Alpha1)) {
    //         infoUI.ShowEmptyPanel();
    //         ResetWallColor(lastHoveredWall);
    //         ResetWallColor(selectedWall);
    //         selectedWall = null;
    //         lastHoveredWall = null;
    //         ResetTowerColor(lasthoveredTower);
    //         ResetTowerColor(selectedTower);
    //         selectedTower = null;
    //         lasthoveredTower = null;
    //     }
    //     if (currentBuildMode != 2){}
    //         visualizerManager.GetNodeSelectionVisualizer().HideVisualizer();
    //     if (currentBuildMode == 1) {
    //         BaseTower towerUnderMouse = GetTowerUnderMouse();
    //         if (towerUnderMouse) {
    //             HighlightTower(towerUnderMouse);

    //             if (Input.GetMouseButtonDown(0)) { // Left mouse button
    //                 SelectTower(towerUnderMouse); // Select the tower
    //             }
    //         } else if (lasthoveredTower != null && lasthoveredTower != selectedTower) {
    //             // If no tower is under the mouse, reset the highlight for the previously hovered tower
    //             ResetTowerColor(lasthoveredTower);
    //             lasthoveredTower = null; // Clear the reference to the hovered tower
    //         }

    //         // Highlight walls
    //         Wall wallUnderMouse = GetWallUnderMouse();
    //         if (wallUnderMouse) {
    //             HighlightWall(wallUnderMouse);
    //             if (Input.GetMouseButtonDown(0)) {
    //                 SelectWall(wallUnderMouse);
    //             }
    //         } else if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
    //             ResetWallColor(lastHoveredWall);
    //             lastHoveredWall = null; // Clear the last hovered wall
    //         }
    //     }

    //     else if (currentBuildMode == 2) {
    //         Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
    //         Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);

    //         if (nodeUnderMouse != null) {
    //             // Check if there is already a wall at the node
    //             if (nodeUnderMouse.wallObject) { // Assuming Node has a method HasWall
    //                 visualizerManager.GetNodeSelectionVisualizer().HideVisualizer(); // Hide visualizer if a wall is present
    //             } else {
    //                 visualizerManager.GetNodeSelectionVisualizer().ShowNodeVisual(nodeUnderMouse);
    //                 if (Input.GetMouseButtonDown(0)) { // Left mouse button
    //                     PlaceWallAtNode(nodeUnderMouse);
    //                 }
    //             }
    //         }
    //         else {
    //             // Optional: Hide the visualizer if the node is out of bounds
    //             visualizerManager.GetNodeSelectionVisualizer().HideVisualizer();
    //         }
    //     }

    // }
    
    // Function to highlight the wall when hovered
    private void HighlightWall(Wall wall) {
        if (lastHoveredWall != wall && selectedWall != wall) { // Only highlight if not already hovered or selected
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null) {
                if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
                    lastHoveredWall.ResetColor();
                }

                lastHoveredWall = wall; // Set the current wall as hovered

                // Set the color to a light bluish filter for hover
                if (selectedWall != wall)
                    wallRenderer.material.color = new Color(0.7f, 0.8f, 1f, 1f); 
            } else {
                Debug.LogError("Renderer not found on the hovered wall!");
            }
        }
    }



    // Function to highlight the tower when hovered
    private void HighlightTower(BaseTower tower) {
        if (lasthoveredTower != tower) { // Only highlight if it's not already highlighted
            Renderer towerRenderer = tower.GetComponent<Renderer>();
            if (towerRenderer != null) {
                // Store the original color if it's not already stored
                if (lasthoveredTower != null  && lasthoveredTower != selectedTower) {
                    lasthoveredTower.ResetColor(); // Reset previous hover highlight
                }

                // Store the currently hovered tower
                lasthoveredTower = tower;

                // Check if the tower is not selected before changing color
                if (selectedTower != tower) {
                    towerRenderer.material.color = new Color(0.8f, 1f, 0.8f, 1f); // Light green filter for hover
                }
            } else {
                Debug.LogError("Renderer not found on the hovered tower!");
            }
        }
    }
    // void SelectWall(Wall wall) {
    //     // Check if the wall parameter is null
    //     if (wall == null) {
    //         Debug.LogError("SelectWall called with a null wall!");
    //         return;
    //     }

    //     // If there's already a selected wall, reset its color
    //     if (selectedWall != null) {
    //         ResetWallColor(selectedWall);
    //     }
    //     if (selectedTower != null)
    //         ResetTowerColor(selectedTower);

    //     selectedWall = wall; // Set the new selected wall

    //     // Ensure infoUI is not null before calling the method
    //     if (infoUI == null) {
    //         Debug.LogError("InfoUI is not assigned!");
    //         return;
    //     }

    //     // Change the color of the selected wall to a darker blue tint
    //     Renderer wallRenderer = selectedWall.GetComponent<Renderer>();
    //     if (wallRenderer != null) {
    //         wallRenderer.material.color = new Color(0.2f, 0.3f, 0.6f, 1f); // Darker blue filter
    //     } else {
    //         Debug.LogError("Renderer not found on the selected wall!");
    //     }
    //     infoUI.ShowWallPanel();
    // }

    // void ResetWallColor(Wall wall) {
    //     if (wall != null) {
    //         Renderer wallRenderer = wall.GetComponent<Renderer>();
    //         if (wallRenderer != null) {
    //             wallRenderer.material.color = wall.originalColor; // Restore the original color
    //         }
    //     }
    // }
    // void SelectTower(BaseTower tower) {
    //     // Check if the tower parameter is null
    //     if (tower == null) {
    //         Debug.LogError("SelectTower called with a null tower!");
    //         return;
    //     }

    //     // If there's already a selected tower, reset its color
    //     if (selectedTower != null) {
    //         ResetTowerColor(selectedTower);
    //     }
    //     if (selectedWall != null) {
    //         ResetWallColor(selectedWall);
    //     }

    //     selectedTower = tower; // Set the new selected tower
        

    //     // Ensure infoUI is not null before calling the method
    //     if (infoUI == null) {
    //         Debug.LogError("InfoUI is not assigned!");
    //         return;
    //     }

    //     infoUI.ShowTowerPanel();
    //     infoUI.UpdateTowerInfo(selectedTower.GetTowerData());

    //     // Change the color of the selected tower to a moderate light greenish tint
    //     Renderer towerRenderer = selectedTower.GetComponent<Renderer>();
    //     if (towerRenderer != null) {
    //         towerRenderer.material.color = new Color(0.6f, 1f, 0.6f, 1f); // Moderate light green filter
    //     } else {
    //         Debug.LogError("Renderer not found on the selected tower!");
    //     }
    // }

    // void ResetTowerColor(BaseTower tower) {
    //     if (tower != null) {
    //         Renderer towerRenderer = tower.GetComponent<Renderer>();
    //         if (towerRenderer != null) {
    //             towerRenderer.material.color = tower.originalColor; // Restore the original color
    //         }
    //     }
    // }

    BaseTower GetTowerUnderMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            BaseTower tower = hit.collider.GetComponent<BaseTower>();
            if (tower != null) {
                return tower; // Return the detected tower
            }
        }

        return null;
    }

    Wall GetWallUnderMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayerMask)) {
            Wall wall = hit.collider.GetComponent<Wall>();
            if (wall != null && !wall.towerObject) {
                // Debug.Log("Collision detected with wall: " + wall.name);
                return wall;
            }
        }

        return null;
    }

    void PlaceTowerOnWall(Wall wall, int towerType) {
        if (wall == null) {
            Debug.LogError("Wall is null! Cannot place tower.");
            return;
        }

        GameObject towerPrefab = null;

        // Check the tower type and assign the correct prefab
        if (towerType == 1) {
            towerPrefab = towerPrefabRange;
        } else if (towerType == 2) {
            towerPrefab = towerPrefabMelee;
        } else if (towerType == 3) {
            towerPrefab = towerPrefabNet; 
        } else {
            Debug.LogError("Invalid tower type! Cannot place tower.");
            return; // Exit the method if an invalid tower type is provided
        }

        // Instantiate the tower on top of the existing wall
        Vector3 additionalObjectPosition = wall.transform.position;
        additionalObjectPosition.y += wallPrefab.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall

        GameObject towerObject = Instantiate(towerPrefab, additionalObjectPosition, Quaternion.identity);
        wall.towerObject = towerObject;

        // Access the BaseTower component to get its information
        BaseTower baseTower = towerObject.GetComponent<BaseTower>();

        if (baseTower != null) {
            // Ensure the towerData is initialized before accessing it
            if (baseTower.GetTowerData() != null) {
                // Safe to access towerData
                Debug.Log(baseTower.GetTowerData().price);
            } else {
                Debug.LogError("TowerData is not initialized yet");
                return; // Exit if towerData is not initialized yet
            }

            // Access the tower cost from the BaseTower component
            int towerCost = baseTower.GetTowerData().price;

            // Check if the player can afford the tower
            if (player.CanBuildTower(towerCost)) {
                // Deduct the tower cost from the player's gold
                player.SpendGold(towerCost);

                // Select the tower
                baseTower.Select();
            } else {
                Debug.LogWarning("Not enough gold to place the tower!");
                Destroy(towerObject); // Destroy the tower if the player can't afford it
            }
        } else {
            Debug.LogError("BaseTower component not found on the instantiated tower!");
        }
    }


    void PlaceWallAtNode(Node node) {
        if (node == null) {
            Debug.LogError("Node is null!");
            return;
        }

        if (node.wallObject != null) {
            Debug.LogWarning("Node already has a wall!");
            return;
        }

        if (node.walkable && node.isBuildable) {
            // Check if the player has enough resources to build a wall
            if (player.CanBuildWall()) {
                // Place the wall at the node's position
                Vector3 offsetPosition = node.worldPosition;
                offsetPosition.y += 0.3f;  // Adjust height to avoid sinking

                // Random rotation for variety
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                GameObject wallObject = Instantiate(wallPrefab, offsetPosition, randomRotation);
                
                // Access the Wall component from the instantiated wall object
                Wall wallComponent = wallObject.GetComponent<Wall>();
                if (wallComponent != null) {
                    wallComponent.node = node;  // Assign the node to the wall
                    node.wallObject = wallObject;  // Link the wall to the node

                    // wallComponent.wallPlacement = this; <-- mayby
                } else {
                    Debug.LogError("Wall component missing on instantiated wall!");
                }
                // Update node properties
                node.walkable = false;  // Mark the node as non-walkable

                // Deduct resources from the player
                player.UseIceBlock();

                // Check if the placement blocks the path
                bool pathExists = Pathfinding.Instance.DoesPathExist(spawner.position, goal.position);
                if (!pathExists) {
                    // If the wall blocks the path, remove it
                    Destroy(wallObject);
                    node.walkable = true;
                    node.wallObject = null;
                    Debug.LogWarning("Wall placement blocked the path. Wall removed.");
                }
            } else {
                Debug.LogWarning("Not enough ice blocks to build a wall!");
            }
        } else {
            Debug.LogWarning("Node is not walkable or buildable.");
        }
    }
}
