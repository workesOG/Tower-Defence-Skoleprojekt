using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoWaveController : MonoBehaviour
{
    public float autoModeInterval = 10f; // Interval to wait before starting the next wave in auto mode
    private bool autoModeEnabled = false;
    private Coroutine autoWaveCoroutine;

    private WaitForSeconds waitForAutoModeInterval;  // Store reference to avoid repeated allocations

    public static AutoWaveController Instance { get; private set; }

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
        // Store the WaitForSeconds object to avoid memory allocations each time
        waitForAutoModeInterval = new WaitForSeconds(autoModeInterval);
    }

    // Triggered when the auto-mode toggle is changed
    public void OnAutoModeToggleChanged(bool isOn)
    {
        autoModeEnabled = isOn;

        if (autoModeEnabled)
        {
            StartAutoMode();
        }
        else
        {
            StopAutoMode();
        }
    }

    // Start automatic wave spawning
    private void StartAutoMode()
    {
        // Start the auto wave coroutine if not already running
        if (autoWaveCoroutine == null)
        {
            autoWaveCoroutine = StartCoroutine(AutoWaveRoutine());
            UIHandler.Instance.startWaveButton.interactable = false;
        }
    }

    // Stop automatic wave spawning
    private void StopAutoMode()
    {
        // Stop the auto wave coroutine if running
        if (autoWaveCoroutine != null)
        {
            StopCoroutine(autoWaveCoroutine);
            autoWaveCoroutine = null;
        }
    }

    // Coroutine to handle automatic wave progression
    private IEnumerator AutoWaveRoutine()
    {
        while (autoModeEnabled)  // Keep running while auto mode is enabled
        {
            if (!WaveHandler.Instance) yield break;

            // Check if there are remaining waves
            if (WaveHandler.Instance.HasWavesRemaining())
            {
                // Start the next wave
                WaveHandler.Instance.StartNextWave(() =>
                {
                    Debug.Log("Auto mode: Wave completed, waiting to start the next wave.");
                });

                // Wait for the interval before starting the next wave
                yield return waitForAutoModeInterval;  // Use cached WaitForSeconds object
            }
            else
            {
                // No more waves remaining, stop auto mode
                Debug.Log("All waves completed, disabling auto mode.");
                StopAutoMode();

                // Update the AutoModeToggle to reflect the change
                UIHandler.Instance.AutoModeToggle.isOn = false;
                UIHandler.Instance.AutoModeToggle.interactable = false;

                yield break;  // Exit the coroutine
            }
        }
    }
}
