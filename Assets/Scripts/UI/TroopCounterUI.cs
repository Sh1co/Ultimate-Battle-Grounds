using System;
using TMPro;
using UnityEngine;


public class TroopCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _firstArmyCounterText;
    [SerializeField] private TextMeshProUGUI _secondArmyCounterText;
    [SerializeField] private TroopCounter _troopCounter;

    private void UpdateTroopCounts()
    {
        _firstArmyCounterText.text = "First army: " + _troopCounter.FirstArmyCount + " alive";
        _secondArmyCounterText.text = "Second army: " + _troopCounter.SecondArmyCount + " alive";
    }

    private void OnEnable()
    {
        _troopCounter.TroopCountChanged += UpdateTroopCounts;
    }

    private void OnDisable()
    {
        _troopCounter.TroopCountChanged -= UpdateTroopCounts;
    }
}