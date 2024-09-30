using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; 
using static Collections;
using static Enums; 
using System; 

// Class responsible for managing waves of enemies
public class WaveHandler : MonoBehaviour
{
    // Reference to the enemy prefab that will be instantiated
    public GameObject enemy;

    // Path to the JSON file that contains wave data
    public string jsonFilePath = "Assets/WaveData.json"; 

    // Variable to store the loaded wave data
    private WaveData waveData;

    // Index to track the current wave being processed
    private int currentWaveIndex = 0;

    // Singleton instance of WaveHandler to ensure there's only one active at a time
    public static WaveHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton Pattern: ensure only one instance exists
        if (Instance == null)
        {
            Instance = this; 
            // WriteTestWaveConfig();
            LoadWaveData(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to load wave data from a JSON file
    private void LoadWaveData()
    {
        // Check if the JSON file exists at the specified path
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON file content
            string json = File.ReadAllText(jsonFilePath);
            // Deserialize the JSON into the WaveData object
            waveData = JsonUtility.FromJson<WaveData>(json);
        }
        else
        {
            // Log an error if the JSON file is not found
            Debug.LogError("JSON file not found at: " + jsonFilePath);
        }
    }

    // Method to write a test wave configuration to a JSON file
    public void WriteTestWaveConfig()
    {
        string filePath = "Assets/TestWaveData.json";

        // Create a new instance of WaveData with test wave information
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

        // Serialize the WaveData object to JSON format
        string jsonContent = JsonUtility.ToJson(waveData, true);
        // Write the JSON content to the specified file
        File.WriteAllText(filePath, jsonContent);
    }

    // Method to start the next wave of enemies
    public void StartNextWave(System.Action onWaveComplete)
    {
        // Check if there are more waves to spawn
        if (currentWaveIndex < waveData.waves.Count)
        {
            // Start the coroutine to spawn the current wave
            StartCoroutine(SpawnWave(waveData.waves[currentWaveIndex], onWaveComplete));
            currentWaveIndex++; 
        }
        else
        {
            // Log message if all waves have been completed
            Debug.Log("All waves completed!");
            onWaveComplete?.Invoke(); 
        }
    }

    // Coroutine to spawn a wave of enemies
    private IEnumerator SpawnWave(Wave wave, System.Action onWaveComplete)
    {
        // Log message indicating the start of the wave
        Debug.Log($"Starting wave {currentWaveIndex + 1}");

        // Iterate through each sub-wave in the wave
        foreach (SubWave subWave in wave.subWaves)
        {
            // Spawn enemies based on the sub-wave configuration
            for (int i = 0; i < subWave.count; i++)
            {
                // Parse the enemy type from the string to the EnemyType enum
                EnemyType enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), subWave.type);
                // Instantiate the enemy at the current position
                Enemies.Instantiate(enemyType, this.transform.position, Quaternion.identity);

                // Calculate the wait time: if it's the last enemy, use the end interval, otherwise use the standard interval
                float waitTime = i == subWave.count - 1 ? subWave.endInterval : subWave.interval;
                // Wait for the calculated time before spawning the next enemy
                yield return new WaitForSeconds(waitTime); 
            }
        }

        // Log message indicating the completion of the wave
        Debug.Log("Wave completed.");
        onWaveComplete?.Invoke(); 
    }

    // Method to check if there are more waves left to spawn
    public bool HasWavesRemaining()
    {
        // Return true if there are more waves remaining
        return currentWaveIndex < waveData.waves.Count;
    }
}

// Class representing the overall wave data structure
[System.Serializable]
public class WaveData
{
    public List<Wave> waves;
}

// Class representing a single wave
[System.Serializable]
public class Wave
{
    // List of sub-waves within the wave
    public List<SubWave> subWaves; 
}

// Class representing a sub-wave
[System.Serializable]
public class SubWave
{
    public string type; 
    public int count; 
    public float interval; 
    public float endInterval; 
}
