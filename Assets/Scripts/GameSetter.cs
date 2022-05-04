﻿using System;
using System.Collections.Generic;
using UnityEngine;


public class GameSetter : MonoBehaviour
{
    public Transform FirstArmySpawn;
    public Transform SecondArmySpawn;

    public Action ArmiesSet;

    public BattleController SetArmies(List<Troop> FirstArmy, List<Troop> SecondArmy)
    {
        var battleController = new BattleController();
        _countTarget = FirstArmy.Count + SecondArmy.Count;
        battleController.FirstArmy = SetArmy(FirstArmy, FirstArmySpawn, 2.5f, 2.5f);
        battleController.SecondArmy = SetArmy(SecondArmy, SecondArmySpawn, 2.5f, 2.5f);
        battleController.Pause();
        battleController.SetTroopsEnemies();
        battleController.StartTroopSearch();
        return battleController;
    }

    private List<Troop> SetArmy(List<Troop> army, Transform spawnPoint, float xSpacing, float ySpacing)
    {
        List<Troop> setTroops = new List<Troop>();

        int armyIndex = 0;
        var armySqrt = Mathf.Ceil(Mathf.Sqrt(army.Count));
        int xShift = Mathf.FloorToInt((armySqrt / 2) * xSpacing);
        int yShift = Mathf.FloorToInt((armySqrt / 2) * ySpacing);

        for (var i = 0; i < armySqrt; i++)
        {
            for (var j = 0; j < armySqrt; j++)
            {
                if (armyIndex == army.Count) break;
                var troop = Instantiate(army[armyIndex],
                    new Vector3(spawnPoint.position.x + xSpacing * i - xShift, spawnPoint.position.y,
                        spawnPoint.position.z + ySpacing * j - yShift), Quaternion.identity, spawnPoint);
                troop.TroopReady += TroopReady;
                setTroops.Add(troop);
                armyIndex++;
            }

            if (armyIndex == army.Count) break;
        }

        return setTroops;
    }

    private void TroopReady(Troop troop)
    {
        troop.TroopReady -= TroopReady;
        _initializedCounter++;

        if (_initializedCounter == _countTarget)
        {
            ArmiesSet?.Invoke();
        }
    }

    private int _initializedCounter = 0;
    private int _countTarget;
}