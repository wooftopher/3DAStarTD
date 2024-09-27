using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WallPlacement : MonoBehaviour {

    // private Grid grid;
    public Button sellButtonTower;
    public Button sellButtonWall;

    private NodeSelectionVisualizer Nodevisualizer;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    public Transform spawner;
    public Transform goal;
    private InfoUI infoUI;
    private Player player;
    private int currentBuildMode;
    private int wallLayerMask;
    private int towerLayerMask;

    private Wall lastHoveredWall;
    private Wall selectedWall;
    private BaseTower lasthoveredTower;
    private BaseTower selectedTower;



    private bool isSellButtonTowerPressed = false;
    private bool isSellButtonWallPressed = false;
    void Start(){
        Nodevisualizer = GetComponent<NodeSelectionVisualizer>();
        wallLayerMask = LayerMask.GetMask("Wall"); // Replace "Wall" with your layer name
        towerLayerMask = LayerMask.GetMask("Tower");
        infoUI = FindObjectOfType<InfoUI>();
        player = FindObjectOfType<Player>();
        sellButtonTower.onClick.AddListener(OnSellButtonTowerPressed);
        sellButtonWall.onClick.AddListener(OnSellButtonWallPressed);
    }
    private void OnSellButtonTowerPressed() {
        if(selectedTower)
            isSellButtonTowerPressed = true; // Mark the sell button as pressed
    }
    private void OnSellButtonWallPressed() {
        if(selectedWall)
            isSellButtonWallPressed = true; // Mark the sell button as pressed
    }

    public void SetBuildMode(int mode) {
        currentBuildMode = mode;
        // Add logic here to handle different build modes based on currentBuildMode
    }
    void Update() {
        if (selectedTower)
            infoUI.ShowTowerPanel();
        else if (selectedWall)
            infoUI.ShowWallPanel();
        else
            infoUI.ShowEmptyPanel();
        if (currentBuildMode != 1) {
            ResetWallColor(lastHoveredWall);
            ResetWallColor(selectedWall);
            selectedWall = null;
            lastHoveredWall = null;
            ResetTowerColor(lasthoveredTower);
            ResetTowerColor(selectedTower);
            selectedTower = null;
            lasthoveredTower = null;
        }
        if (currentBuildMode != 2)
            Nodevisualizer.HideVisualizer();
        if (currentBuildMode != 3)
            Nodevisualizer.HideTowerVisual();

        if (currentBuildMode == 1) {
            BaseTower towerUnderMouse = GetTowerUnderMouse();
            if (towerUnderMouse) {
                HighlightTower(towerUnderMouse);

                if (Input.GetMouseButtonDown(0)) { // Left mouse button
                    SelectTower(towerUnderMouse); // Select the tower
                }
            } else if (lasthoveredTower != null && lasthoveredTower != selectedTower) {
                // If no tower is under the mouse, reset the highlight for the previously hovered tower
                ResetTowerColor(lasthoveredTower);
                lasthoveredTower = null; // Clear the reference to the hovered tower
            }

            // Highlight walls
            Wall wallUnderMouse = GetWallUnderMouse();
            if (wallUnderMouse) {
                HighlightWall(wallUnderMouse);
                if (Input.GetMouseButtonDown(0)) {
                    SelectWall(wallUnderMouse);
                }
            } else if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
                ResetWallColor(lastHoveredWall);
                lastHoveredWall = null; // Clear the last hovered wall
            }

            // Handle sell button logic
            if (isSellButtonTowerPressed) {
                SellTower(); // Call the sell tower method
                isSellButtonTowerPressed = false; // Reset the button press state
            }
            if (isSellButtonWallPressed) {
                SellWall(); // Call the sell tower method
                isSellButtonWallPressed = false; // Reset the button press state
            }
        }

        else if (currentBuildMode == 2) {
            Vector3 mouseWorldPosition = Grid.Instance.GetMouseWorldPosition();
            Node nodeUnderMouse = Grid.Instance.NodeFromWorldPoint(mouseWorldPosition);

            if (nodeUnderMouse != null) {
                // Check if there is already a wall at the node
                if (nodeUnderMouse.wallObject) { // Assuming Node has a method HasWall
                    Nodevisualizer.HideVisualizer(); // Hide visualizer if a wall is present
                } else {
                    Nodevisualizer.ShowNodeVisual(nodeUnderMouse);
                    if (Input.GetMouseButtonDown(0)) { // Left mouse button
                        PlaceWallAtNode(nodeUnderMouse);
                    }
                }
            }
            else {
                // Optional: Hide the visualizer if the node is out of bounds
                Nodevisualizer.HideVisualizer();
            }
        }
        else if (currentBuildMode == 3) {
            Wall wallUnderMouse = GetWallUnderMouse();
            if (wallUnderMouse) {
                // Check if a tower is already placed on the wall
                if (!wallUnderMouse.towerObject) { // Assuming Wall has a method HasTower
                    Nodevisualizer.ShowTowerVisual(wallUnderMouse);
                    if (Input.GetMouseButtonDown(0)) { // Left mouse button
                        PlaceTowerOnWall(wallUnderMouse);
                    }
                } else {
                    Nodevisualizer.HideTowerVisual(); // Hide if a tower is already present
                }
            } else {
                Nodevisualizer.HideTowerVisual();
            }
        }
        // else if (currentBuildMode == 4) {
        //     HandleRemoveMode();
        // }
    }
    
    // Function to highlight the wall when hovered
    private void HighlightWall(Wall wall) {
        if (lastHoveredWall != wall && selectedWall != wall) { // Only highlight if not already hovered or selected
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null) {
                if (lastHoveredWall != null && lastHoveredWall != selectedWall) {
                    ResetWallColor(lastHoveredWall);
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

    void SelectWall(Wall wall) {
        // Check if the wall parameter is null
        if (wall == null) {
            Debug.LogError("SelectWall called with a null wall!");
            return;
        }

        // If there's already a selected wall, reset its color
        if (selectedWall != null) {
            ResetWallColor(selectedWall);
        }
        if (selectedTower != null)
            ResetTowerColor(selectedTower);

        selectedWall = wall; // Set the new selected wall

        // Ensure infoUI is not null before calling the method
        if (infoUI == null) {
            Debug.LogError("InfoUI is not assigned!");
            return;
        }

        // infoUI.UpdateWallInfo(selectedWall); // Update the wall info in the UI

        // Change the color of the selected wall to a darker blue tint
        Renderer wallRenderer = selectedWall.GetComponent<Renderer>();
        if (wallRenderer != null) {
            wallRenderer.material.color = new Color(0.2f, 0.3f, 0.6f, 1f); // Darker blue filter
        } else {
            Debug.LogError("Renderer not found on the selected wall!");
        }
    }

    void ResetWallColor(Wall wall) {
        if (wall != null) {
            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer != null) {
                wallRenderer.material.color = wall.originalColor; // Restore the original color
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
                    ResetTowerColor(lasthoveredTower); // Reset previous hover highlight
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

    void SelectTower(BaseTower tower) {
        // Check if the tower parameter is null
        if (tower == null) {
            Debug.LogError("SelectTower called with a null tower!");
            return;
        }

        // If there's already a selected tower, reset its color
        if (selectedTower != null) {
            ResetTowerColor(selectedTower);
        }
        if (selectedWall != null) {
            ResetWallColor(selectedWall);
        }

        selectedTower = tower; // Set the new selected tower
        

        // Ensure infoUI is not null before calling the method
        if (infoUI == null) {
            Debug.LogError("InfoUI is not assigned!");
            return;
        }

        infoUI.UpdateTowerInfo(selectedTower);

        // Change the color of the selected tower to a moderate light greenish tint
        Renderer towerRenderer = selectedTower.GetComponent<Renderer>();
        if (towerRenderer != null) {
            towerRenderer.material.color = new Color(0.6f, 1f, 0.6f, 1f); // Moderate light green filter
            // towerRenderer.material.color = Color.black; // Set color to black
        } else {
            Debug.LogError("Renderer not found on the selected tower!");
        }

        // Debug.Log("Selected Tower: " + selectedTower.name); // Debug message for selected tower
    }

    void ResetTowerColor(BaseTower tower) {
        if (tower != null) {
            Renderer towerRenderer = tower.GetComponent<Renderer>();
            if (towerRenderer != null) {
                towerRenderer.material.color = tower.originalColor; // Restore the original color
            }
        }
    }
    private void SellTower() {
        // Logic to sell the tower
        player.EarnGold(selectedTower.Price); // Give gold back to player
        Destroy(selectedTower.gameObject); // Destroy the tower
        selectedTower = null; // Clear the selected tower
        // infoUI.UpdateTowerInfo(null); // Update UI to clear tower info
    }
    private void SellWall() {
        Debug.Log("sell press");
        // Logic to sell the tower
        player.EarnIceBlock(1); // Give gold back to player
        selectedWall.node.walkable = true;
        selectedWall.node.isBuildable = true;
        Destroy(selectedWall.gameObject); // Destroy the tower
        selectedWall = null; // Clear the selected tower

        // infoUI.UpdateTowerInfo(null); // Update UI to clear tower info
    }

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

    // void HandleRemoveMode(){
    //     Wall wallUnderMouse = GetWallUnderMouse();
        
    //     // If hovering over a wall
    //     if (wallUnderMouse) {
    //         // Change color only if it's different from the last hovered wall
    //         if (lastHoveredWall != wallUnderMouse) {
    //             // Restore the color of the previously hovered wall
    //             if (lastHoveredWall != null) {
    //                 RestoreWallColor(lastHoveredWall);
    //             }

    //             // Store the current wall as the last hovered
    //             lastHoveredWall = wallUnderMouse;

    //             // Get the renderer and store the original color
    //             Renderer wallRenderer = wallUnderMouse.GetComponent<Renderer>();
    //             if (wallRenderer != null) {
    //                 // Store the original color
    //                 originalColorWall = wallRenderer.material.color;

    //                 // Set the color to reddish
    //                 wallRenderer.material.color = Color.red; // Change this to your desired color
    //             }
    //         }
    //     } 
    //     else {
    //         // Restore the color if not hovering over a wall
    //         if (lastHoveredWall != null) {
    //             RestoreWallColor(lastHoveredWall);
    //             lastHoveredWall = null; // Clear the last hovered wall
    //         }
    //     }

    //     // If left-clicking, remove the wall
    //     if (Input.GetMouseButtonDown(0) && wallUnderMouse) {
    //         RemoveWallAtNode(Grid.Instance.NodeFromWorldPoint(wallUnderMouse.transform.position));
    //     }
    // }

    Wall GetWallUnderMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallLayerMask)) {
            Wall wall = hit.collider.GetComponent<Wall>();
            if (wall != null && !wall.towerObject) {
                // Debug.Log("Collision detected with wall: " + wall.name);
                return wall;
            }
        // } else {
        //     Debug.Log("No collision detected.");
        }

        return null;
    }

    void PlaceTowerOnWall(Wall wall) {
        if (wall == null) {
            Debug.LogError("Wall is null! Cannot place tower.");
            return;
        }

        // Check if the player can build the tower (enough gold)
        int towerCost = 50; // Replace with your tower cost value whyyyyyyyyyyyyyyyyyy???????????
        // for now just one tower but must be changed
        if (player.CanBuildTower(towerCost)) {
            // Place tower on top of the existing wall
            Vector3 additionalObjectPosition = wall.transform.position;
            additionalObjectPosition.y += wallPrefab.GetComponent<Renderer>().bounds.size.y; // Adjust the y position to place it on top of the wall
            
            GameObject towerObject = Instantiate(towerPrefab, additionalObjectPosition, Quaternion.identity);
            wall.towerObject = towerObject;

            // Deduct the tower cost from the player's gold
            player.SpendGold(towerCost);
        } else {
            Debug.LogWarning("Not enough gold to place the tower!");
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



    void RemoveWallAtNode(Node node) {
        if (node == null) {
            Debug.LogError("Node is null!");
            return;
        }
        
        if (!node.walkable) {
            // Check if the wall exists
            if (node.wallObject != null) {
                // Get the Wall component to access the tower
                Wall wall = node.wallObject.GetComponent<Wall>();

                // Check if the tower exists and destroy it
                if (wall.towerObject != null) {
                    Destroy(wall.towerObject);
                    Debug.Log("Tower removed.");
                }

                // Destroy the wall object
                Destroy(node.wallObject);
                node.wallObject = null; // Clear the reference to the wall object
                player.EarnIceBlock(1);
            }
            
            node.walkable = true; // Set the node to walkable
        } else {
            Debug.Log("Node is not walkable.");
        }
    }
}
