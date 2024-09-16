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

    void Start()
    {
        SetupButtons();
    }

    void Update()
    {
        foreach (var stpb in towerPlacementButtons)
        {
            Tower tower = Towers.GetTower(stpb.type);
            bool buttonEnabled = CurrencyHandler.Instance.Money >= tower.price;
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
            bool buttonEnabled = CurrencyHandler.Instance.Money >= tower.price;
            stpb.button.interactable = buttonEnabled;
            stpb.button.transform.GetComponentInChildren<TMP_Text>().text = $"{type} ({tower.price})";
        }
    }
}

[Serializable]
public class SerializableTowerPlacementButton
{
    public TowerType type;
    public Button button;
}
