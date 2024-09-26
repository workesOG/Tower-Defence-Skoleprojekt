using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // For reading the JSON file

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the wave data from the JSON file
        LoadWaveData();
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
            Debug.Log($"Starting Wave: {currentWaveIndex}");
        }
        else
        {
            Debug.Log("All waves completed!");
            onWaveComplete?.Invoke();
        }
    }

    private IEnumerator SpawnWave(Wave wave, System.Action onWaveComplete)
    {
        Debug.Log("Starting wave " + currentWaveIndex);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Instantiate(enemy, this.transform.position, Quaternion.identity);
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
    public float delayBetweenWaves;
}

[System.Serializable]
public class WaveData
{
    public List<Wave> waves;
}
