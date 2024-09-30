// Er lavet med hjælp fra chatGPT. Den hjælp vi har fået er hjælp til at lave toggle den smarteste måde så den kan arbejde sammen med WaveHandleren.
using System.Collections;
using UnityEngine;
public class AutoWaveController : MonoBehaviour
{
    // Flag to check if auto mode is enabled
    private bool autoModeEnabled = false;

    // Reference to the currently running coroutine for auto wave spawning 
    private Coroutine autoWaveCoroutine; 

    // Caching the WaitForSeconds object to optimize performance
    private WaitForSeconds waitForAutoModeInterval;

    // Singleton instance for easy access to the AutoWaveController
    public static AutoWaveController Instance { get; private set; }

    private void Awake()
    {
        // Implement singleton pattern to ensure only one instance of AutoWaveController exists
        if (Instance == null)
        {
            // Set this instance as the singleton
            Instance = this; 
        }
        else
        {
            // Destroy duplicate instance
            Destroy(gameObject); 
        }
    }

    
    // Method to be called when the auto-mode toggle is changed
    public void OnAutoModeToggleChanged(bool isOn)
    {
         // Update the auto mode state based on the toggle
        autoModeEnabled = isOn;

        if (autoModeEnabled)
        {
            // Start automatic wave spawning if enabled
            StartAutoMode(); 
        }
        else
        {
            // Stop automatic wave spawning if disabled
            StopAutoMode(); 
        }
    }

    // Starts the automatic wave spawning process
    private void StartAutoMode()
    {
        // Check if the auto wave coroutine is not already running
        if (autoWaveCoroutine == null)
        {
            // Start the coroutine to handle auto wave progression
            autoWaveCoroutine = StartCoroutine(AutoWaveRoutine());
            // Disable the start button to prevent manual interference
            UIHandler.Instance.startWaveButton.interactable = false; 
        }
    }

    // Stops the automatic wave spawning process
    private void StopAutoMode()
    {
        // Check if the auto wave coroutine is currently running
        if (autoWaveCoroutine != null)
        {
            // Stop the coroutine
            StopCoroutine(autoWaveCoroutine);

            // Clear the coroutine reference 
            autoWaveCoroutine = null; 

            // Re-enable the start button
            UIHandler.Instance.startWaveButton.interactable = true; 
        }
    }

    // Coroutine that handles the automatic wave progression
    private IEnumerator AutoWaveRoutine()
    {
        // Loop while auto mode is enabled
        while (autoModeEnabled)  
        {
            // Check if WaveHandler instance exists; if not, exit the coroutine
            if (!WaveHandler.Instance) yield break;

            // Check if there are remaining waves to spawn
            if (WaveHandler.Instance.HasWavesRemaining())
            {
                // Start the next wave
                WaveHandler.Instance.StartNextWave(() =>
                {
                    // Log a message when the wave is completed
                    Debug.Log("Auto mode: Wave completed, waiting to start the next wave.");
                });

                // Wait for the specified interval before starting the next wave
                yield return waitForAutoModeInterval; 
            }
            else
            {
                // Log that all waves are completed and disable auto mode
                Debug.Log("All waves completed, disabling auto mode.");
                StopAutoMode(); // Stop the automatic wave spawning

                // Update the AutoModeToggle to reflect the change
                UIHandler.Instance.AutoModeToggle.isOn = false; 
                UIHandler.Instance.AutoModeToggle.interactable = false;

                // Exit the coroutine as no more waves are available
                yield break;
            }
        }
    }
}
