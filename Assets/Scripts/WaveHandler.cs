using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // For reading the JSON file
using static Collections;
using static Enums;
using System;

public class WaveHandler : MonoBehaviour
{
    public GameObject enemy;
    public string jsonFilePath = "Assets/WaveData.json"; // Path to the JSON file

    private WaveData waveData;
    private int currentWaveIndex = 0;

    public static WaveHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //WriteTestWaveConfig();
            LoadWaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadWaveData()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            waveData = JsonUtility.FromJson<WaveData>(json);
        }
        else
        {
            Debug.LogError("JSON file not found at: " + jsonFilePath);
        }
    }

    public void WriteTestWaveConfig()
    {
        string filePath = "Assets/TestWaveData.json";

        WaveData waveData = new WaveData()
        {
            waves = new List<Wave>
            {
                new Wave()
                {
                    subWaves = new List<SubWave>
                    {
                        new SubWave()
                        {
                            type = "Basic", count = 5, interval = 0.5f, endInterval = 1.5f,
                        }
                    }
                },
                new Wave()
                {
                    subWaves = new List<SubWave>
                    {
                        new SubWave()
                        {
                            type = "Basic", count = 10, interval = 0.5f, endInterval = 1.5f,
                        },
                        new SubWave()
                        {
                            type = "Basic", count = 15, interval = 0.5f, endInterval = 1.5f,
                        }
                    }
                },
            }
        };

        string jsonContent = JsonUtility.ToJson(waveData, true);
        File.WriteAllText(filePath, jsonContent);
    }

    public void StartNextWave(System.Action onWaveComplete)
    {
        if (currentWaveIndex < waveData.waves.Count)
        {
            StartCoroutine(SpawnWave(waveData.waves[currentWaveIndex], onWaveComplete));
            currentWaveIndex++;
        }
        else
        {
            Debug.Log("All waves completed!");
            onWaveComplete?.Invoke();
        }
    }

    private IEnumerator SpawnWave(Wave wave, System.Action onWaveComplete)
    {
        Debug.Log($"Starting wave {currentWaveIndex + 1}");

        foreach (SubWave subWave in wave.subWaves)
        {
            for (int i = 0; i < subWave.count; i++)
            {
                EnemyType enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), subWave.type);
                Enemies.Instantiate(enemyType, this.transform.position, Quaternion.identity);
                float waitTime = i == subWave.count - 1 ? subWave.endInterval : subWave.interval;
                yield return new WaitForSeconds(waitTime);
            }
        }

        Debug.Log("Wave completed.");
        onWaveComplete?.Invoke();
    }

    // Method to check if there are more waves left to spawn
    public bool HasWavesRemaining()
    {
        return currentWaveIndex < waveData.waves.Count;
    }
}

[System.Serializable]
public class WaveData
{
    public List<Wave> waves;
}

[System.Serializable]
public class Wave
{
    public List<SubWave> subWaves;
}

[System.Serializable]
public class SubWave
{
    public string type;
    public int count;
    public float interval;
    public float endInterval;
}