using System;
using TMPro;
using UnityEngine;


public class BattleSpeedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _battleSpeedText;
    [SerializeField] private GameManager _manager;

    
    private void UpdateBattleSpeedText(float speed)
    {
        _battleSpeedText.text = "Battle speed: x" + speed;
    }
    
    private void OnEnable()
    {
        _manager.TimeScaleChanged += UpdateBattleSpeedText;
    }

    private void OnDisable()
    {
        _manager.TimeScaleChanged -= UpdateBattleSpeedText;
    }
}