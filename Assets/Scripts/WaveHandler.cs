using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // For reading the JSON file
using static Collections;
using static Enums;

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

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Enemies.Instantiate(EnemyType.Basic, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wave.cooldown);
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
public class Wave
{
    public int enemyCount;
    public float cooldown;
}

[System.Serializable]
public class WaveData
{
    public List<Wave> waves;
}
