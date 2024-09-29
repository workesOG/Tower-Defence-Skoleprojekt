using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Enums;

public static class Collections
{
    public static class Towers
    {
        public static Dictionary<TowerType, Type> towers = new Dictionary<TowerType, Type>
        {
            { TowerType.Basic, typeof(TowerBasic)},
            { TowerType.Sniper, typeof(TowerSniper)},
        };

        public static Dictionary<TowerType, string> towerPaths = new Dictionary<TowerType, string>
        {
            { TowerType.Basic, "Basic Tower"},
            { TowerType.Sniper, "Sniper Tower"},
        };

        public static Tower GetTower(TowerType type)
        {
            if (towers.TryGetValue(type, out Type towerType))
            {

                // Ensure that the type is a Tower and assignable from MonoBehaviour
                if (typeof(Tower).IsAssignableFrom(towerType))
                {

                    // Create a temporary GameObject
                    GameObject tempGameObject = new GameObject("TempTower");

                    try
                    {
                        // Add the tower component to the GameObject
                        Tower tower = tempGameObject.AddComponent(towerType) as Tower;

                        if (tower != null)
                        {
                            // Cleanup the temporary GameObject after use
                            GameObject.Destroy(tempGameObject);
                            return tower;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error creating tower instance: {ex.Message}");
                    }

                    // Cleanup the temporary GameObject in case of failure
                    GameObject.Destroy(tempGameObject);
                }
            }

            Debug.Log("Tower type not found");
            return null;
        }

        public static float GetTowerPrice(TowerType type)
        {
            // Get an instance of the tower
            Tower tower = GetTower(type);
            if (tower != null)
            {
                // Directly access the price property from the tower instance
                return tower.price;
            }
            return 0f; // Return a default or error value if the tower or price isn't found
        }
    }

    public static class Enemies
    {
        public static Dictionary<EnemyType, Type> enemies = new Dictionary<EnemyType, Type>
        {
            { EnemyType.Basic, typeof(EnemyBasic)},
        };

        public static Dictionary<EnemyType, string> enemyPaths = new Dictionary<EnemyType, string>
        {
            { EnemyType.Basic, "Basic Enemy"},
        };

        public static Enemy GetEnemy(EnemyType type)
        {
            if (enemies.TryGetValue(type, out Type enemyType))
            {

                // Ensure that the type is a Tower and assignable from MonoBehaviour
                if (typeof(Enemy).IsAssignableFrom(enemyType))
                {

                    // Create a temporary GameObject
                    GameObject tempGameObject = new GameObject("TempEnemy");

                    try
                    {
                        // Add the tower component to the GameObject
                        Enemy enemy = tempGameObject.AddComponent(enemyType) as Enemy;

                        if (enemy != null)
                        {
                            // Cleanup the temporary GameObject after use
                            GameObject.Destroy(tempGameObject);
                            return enemy;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error creating enemy instance: {ex.Message}");
                    }

                    // Cleanup the temporary GameObject in case of failure
                    GameObject.Destroy(tempGameObject);
                }
            }

            Debug.Log("Enemy type not found");
            return null;
        }

        public static GameObject Instantiate(EnemyType type, Vector3 position, Quaternion rotation)
        {
            GameObject original = Resources.Load<GameObject>($"Prefabs/Enemies/{enemyPaths[type]}");
            GameObject gameObject = GameObject.Instantiate(original, position, rotation);
            gameObject.AddComponent(enemies[type]);
            return gameObject;
        }
    }
}