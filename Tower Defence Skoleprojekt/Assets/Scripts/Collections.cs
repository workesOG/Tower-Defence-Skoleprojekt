using System;
using System.Collections.Generic;
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
            { TowerType.Basic, typeof(TowerBasic)}
        };

        public static Dictionary<TowerType, string> towerPaths = new Dictionary<TowerType, string>
        {
            { TowerType.Basic, "Basic Tower"}
        };

        public static T GetTower<T>(TowerType type) where T : Tower
        {
            foreach (var kvp in towers)
            {
                if (kvp.Key == type)
                {
                    if (kvp.Value is T tower)
                        return tower;
                }
            }
            return default;
        }
    }
}
