using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Enums;

public static class Collections
{
    // Nested static class for managing paddle-related assets
    /*public static class Paddles
    {
        // Dictionary that maps PaddleType enums to their respective resource paths
        private static readonly Dictionary<PaddleType, string> paddlePaths = new Dictionary<PaddleType, string>
        {
            { PaddleType.PlayerPaddle, "Prefabs/Paddles/Player Paddle" },
            { PaddleType.StandardEnemyPaddle, "Prefabs/Paddles/Standard Enemy Paddle" },
        };

        // Properties to directly access paddles
        public static GameObject PlayerPaddle => LoadPaddle(PaddleType.PlayerPaddle);
        public static GameObject StandardEnemyPaddle => LoadPaddle(PaddleType.StandardEnemyPaddle);

        // Method to load a paddle GameObject based on the specified PaddleType
        public static GameObject LoadPaddle(PaddleType paddleType)
        {
            return LoadAsset(paddlePaths, paddleType, "PaddleType");
        }
    }

    // Nested static class for managing ball-related assets
    public static class Balls
    {
        // Dictionary that maps BallType enums to their respective resource paths
        private static readonly Dictionary<BallType, string> ballPaths = new Dictionary<BallType, string>
        {
            { BallType.StandardBall, "Prefabs/Balls/Standard Ball" },
        };

        // Properties to directly access balls
        public static GameObject StandardBall => LoadBall(BallType.StandardBall);

        // Method to load a ball GameObject based on the specified BallType
        public static GameObject LoadBall(BallType ballType)
        {
            return LoadAsset(ballPaths, ballType, "BallType");
        }
    }*/

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

            Debug.Log("Type not found");
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
}
