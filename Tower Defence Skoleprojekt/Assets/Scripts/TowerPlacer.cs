using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
	/// <summary>
	/// Det her script er kompliceret og man skal vide hvad man laver hvis man vil lave ændringer
	/// </summary>



	public GameObject towerPrefab;               // The tower prefab to be placed
	public float towerPlacementRadius = 5f;      // The minimum distance between towers
	private bool canPlaceTower = false;          // Whether the player is allowed to place a tower
	private List<GameObject> placedTowers = new List<GameObject>(); // List to store placed towers

	void Update()
	{
		if (canPlaceTower && Input.GetMouseButtonDown(0))  // Only allow placing if the flag is true
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Create a ray from the camera to where the mouse clicked
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				Transform hitTransform = hit.transform;

				// Check if the clicked object is a child of an object called "BuildableWalls"
				if (hitTransform != null && hitTransform.root.name == "BuildableWalls")
				{
					// Perform an additional check to ensure no towers are too close by
					if (!IsTowerTooClose(hit.point))
					{
						// Place the tower at the hit position
						GameObject newTower = Instantiate(towerPrefab, hit.point, Quaternion.identity);

						// Add the newly placed tower to the list
						placedTowers.Add(newTower);

						// Disable further tower placement until the button is clicked again
						canPlaceTower = false;
					}
					else
					{
						Debug.Log("Cannot place tower here, too close to another tower.");
					}
				}
			}
		}
	}

	// This method will be linked to the button in Unity
	public void EnableTowerPlacement()
	{
		canPlaceTower = true; // Allow the player to place a tower
	}

	// Check if there are any towers within the placement radius
	bool IsTowerTooClose(Vector3 position)
	{
		foreach (GameObject tower in placedTowers)
		{
			if (Vector3.Distance(tower.transform.position, position) < towerPlacementRadius)
			{
				return true; // A tower is too close
			}
		}

		return false; // No towers nearby, so it's safe to place one
	}
}
