using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // For reading files

public class WaveHandler : MonoBehaviour
{
    public GameObject enemy;
    public string jsonFilePath = "Assets/WaveData.json"; // Path to the JSON file

    private WaveData waveData;

    private void Start()
    {
        // Load the wave data from the JSON file
        LoadWaveData();
        
        // Start the coroutine to spawn enemies based on the wave data
        StartCoroutine(SpawnWaves());
    }

    private void LoadWaveData()
    {
        // Check if the file exists
        if (File.Exists(jsonFilePath))
        {
            // Load the JSON file and deserialize it into WaveData
            string json = File.ReadAllText(jsonFilePath);
            waveData = JsonUtility.FromJson<WaveData>(json);
        }
        else
        {
            Debug.LogError("JSON file not found at: " + jsonFilePath);
        }
    }

    public IEnumerator SpawnWaves()
    {
        foreach (Wave wave in waveData.waves)
        {
            // Spawn enemies for the current wave
            for (int i = 0; i < wave.enemyCount; i++)
            {
                // Instantiate enemy at this object's position
                Instantiate(enemy, this.transform.position, Quaternion.identity);
                Debug.Log("Spawning enemy " + (i + 1) + " of " + wave.enemyCount);

                // Wait between each enemy spawn
                yield return new WaitForSeconds(wave.cooldown);
            }

            // Wait for the specified delay between this wave and the next wave
            Debug.Log("Wave completed. Waiting for " + wave.delayBetweenWaves + " seconds before the next wave.");
            yield return new WaitForSeconds(wave.delayBetweenWaves);
        }

        Debug.Log("All waves completed!");
    }
}

[System.Serializable]
public class Wave
{
    public int enemyCount;
    public float delayBetweenWaves;
    public float cooldown;
}

[System.Serializable]
public class WaveData
{
    public List<Wave> waves;
}
