using System;
using UnityEngine;


public class TroopCounter : MonoBehaviour
{
    public int FirstArmyCount { get; private set; }
    public int SecondArmyCount { get; private set; }
    public Action TroopCountChanged;

    private void TroopDied(Troop troop)
    {
        if (troop.Team == 1)
        {
            FirstArmyCount--;
        }
        else
        {
            SecondArmyCount--;
        }

        troop.TroopDied -= TroopDied;
        TroopCountChanged?.Invoke();
    }

    private void SetupCount()
    {
        _controller = _manager.GetBattleController();
        FirstArmyCount = _controller.FirstArmy.Count;
        SecondArmyCount = _controller.SecondArmy.Count;

        foreach (var troop in _controller.FirstArmy)
        {
            troop.TroopDied += TroopDied;
        }

        foreach (var troop in _controller.SecondArmy)
        {
            troop.TroopDied += TroopDied;
        }

        TroopCountChanged?.Invoke();
    }

    private void OnEnable()
    {
        _manager = GetComponent<GameManager>();
        _manager.BattleReady += SetupCount;
    }

    private void OnDisable()
    {
        _manager.BattleReady -= SetupCount;

        foreach (var troop in _controller.FirstArmy)
        {
            if (troop != null) troop.TroopDied -= TroopDied;
        }

        foreach (var troop in _controller.SecondArmy)
        {
            if (troop != null) troop.TroopDied -= TroopDied;
        }
    }

    private GameManager _manager;
    private BattleController _controller;
}