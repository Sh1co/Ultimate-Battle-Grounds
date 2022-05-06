﻿using System;
using UnityEngine;


public class TroopCounter : MonoBehaviour
{
    [HideInInspector] public int FirstArmyCount;
    [HideInInspector] public int SecondArmyCount;
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
        Debug.Log("First army: " + FirstArmyCount + ". Second army: " + SecondArmyCount);
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
        Debug.Log("First army: " + FirstArmyCount + ". Second army: " + SecondArmyCount);
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