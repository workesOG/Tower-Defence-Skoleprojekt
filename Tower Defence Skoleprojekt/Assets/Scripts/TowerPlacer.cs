using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using static Collections;

public class TowerPlacer : MonoBehaviour
{
	public static TowerPlacer Instance;

	public Type typeOfTowerToPlace = null;
	public GameObject towerPrefab;
	public float towerPlacementRadius = 5f;
	private bool canPlaceTower = false;
	private List<GameObject> placedTowers = new List<GameObject>();

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		if (!(canPlaceTower && Input.GetMouseButtonDown(0)))  // Only allow placing if the flag is true
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Create a ray from the camera to where the mouse clicked
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			Transform hitTransform = hit.transform;

			// Check if the clicked object is a child of an object called "BuildableWalls"
			if (!(hitTransform != null && hitTransform.root.name == "BuildableWalls"))
			{
				return;
			}
			if (IsTowerTooClose(hit.point))
			{
				Debug.Log("Cannot place tower here, too close to another tower.");
				return;
			}
			// Place the tower at the hit position
			GameObject newTower = Instantiate(towerPrefab, hit.point, Quaternion.identity);
			newTower.AddComponent(typeOfTowerToPlace);

			// Add the newly placed tower to the list
			placedTowers.Add(newTower);

			// Disable further tower placement until the button is clicked again
			canPlaceTower = false;
		}
	}

	// This method will be linked to the button in Unity
	public void EnableTowerPlacement(TowerType towerType)
	{
		canPlaceTower = true; // Allow the player to place a tower

		// Get the tower type from the dictionary
		Type towerTypeToCreate;
		if (Towers.towers.TryGetValue(towerType, out towerTypeToCreate))
		{
			typeOfTowerToPlace = towerTypeToCreate;
			towerPrefab = Resources.Load<GameObject>($"Prefabs/Towers/{Towers.towerPaths[towerType]}");
		}
		else
			Debug.LogError($"Tower type {towerType} not found in the towers dictionary.");
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
