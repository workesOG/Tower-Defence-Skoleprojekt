using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using static Collections;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    public List<SerializableTowerPlacementButton> towerPlacementButtons;

    [SerializeField]
    public Button startWaveButton;  // The "Next Wave" button reference

    private bool waveInProgress = false; // Track if a wave is currently running

    private void Start()
    {
        SetupButtons();

        // Assign listeners to the button
        startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);

        startWaveButton.interactable = true; // Ensure the button is enabled at the beginning
    }

    private void Update()
    {
        foreach (var stpb in towerPlacementButtons)
        {
            Tower tower = Towers.GetTower(stpb.type);
            bool buttonEnabled = CurrencyHandler.Instance.Money >= tower.price && !waveInProgress; // Disable during wave
            stpb.button.interactable = buttonEnabled;
            stpb.button.transform.GetComponentInChildren<TMP_Text>().text = $"{stpb.type} ({tower.price})";
        }
    }

    private void SetupButtons()
    {
        foreach (var stpb in towerPlacementButtons)
        {
            TowerType type = stpb.type;
            stpb.button.onClick.AddListener(() => TowerPlacer.Instance.EnableTowerPlacement(type));
            Tower tower = Towers.GetTower(type);
            bool buttonEnabled = CurrencyHandler.Instance.Money >= tower.price && !waveInProgress;
            stpb.button.interactable = buttonEnabled;
            stpb.button.transform.GetComponentInChildren<TMP_Text>().text = $"{type} ({tower.price})";
        }
    }

    // This function is called when the "Start Wave" button is pressed
    private void OnStartWaveButtonClicked()
    {
        if (!waveInProgress)
        {
            waveInProgress = true;
            startWaveButton.interactable = false; // Disable the start button during the wave

            // Call the WaveHandler to start the next wave
            WaveHandler.Instance.StartNextWave(OnWaveCompleted);
        }
    }

    // Called when the wave is completed
    private void OnWaveCompleted()
    {
        waveInProgress = false;
        startWaveButton.interactable = true; // Re-enable the button after wave completion
    }
}

[Serializable]
public class SerializableTowerPlacementButton
{
    public TowerType type;
    public Button button;
}
