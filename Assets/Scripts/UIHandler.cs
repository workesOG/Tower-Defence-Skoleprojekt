using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    public List<SerializableTowerPlacementButton> towerPlacementButtons;
    // Start is called before the first frame update
    void Start()
    {
        SetupButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupButtons()
    {
        foreach (var stpb in towerPlacementButtons)
        {
            TowerType type = stpb.type;
            stpb.button.onClick.AddListener(() => TowerPlacer.Instance.EnableTowerPlacement(type));
            stpb.button.transform.GetComponentInChildren<TMP_Text>().text = type.ToString();
        }
    }
}

[Serializable]
public class SerializableTowerPlacementButton
{
    public TowerType type;
    public Button button;
}
