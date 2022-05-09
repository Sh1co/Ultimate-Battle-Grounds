using System;
using System.Collections.Generic;
using UnityEngine;


public class GameSetter : MonoBehaviour
{
    public Transform FirstArmySpawn;
    public Transform SecondArmySpawn;
    [SerializeField] private float _xSpacing = 2.5f;
    [SerializeField] private float _ySpacing = 2.5f;


    public BattleController SetArmies(List<Troop> firstArmy, List<Troop> secondArmy)
    {
        var battleController = new BattleController();
        _countTarget = firstArmy.Count + secondArmy.Count;
        battleController.FirstArmy = SetArmy(firstArmy, FirstArmySpawn, _xSpacing, _ySpacing, 1);
        battleController.SecondArmy = SetArmy(secondArmy, SecondArmySpawn, _xSpacing, _ySpacing, 2);
        battleController.Pause();
        return battleController;
    }

    private List<Troop> SetArmy(List<Troop> army, Transform spawnPoint, float xSpacing, float ySpacing, int team)
    {
        List<Troop> setTroops = new List<Troop>();

        int armyIndex = 0;
        var armySqrt = Mathf.Ceil(Mathf.Sqrt(army.Count));
        int xShift = Mathf.FloorToInt((armySqrt / 2) * xSpacing);
        int yShift = Mathf.FloorToInt((armySqrt / 2) * ySpacing);

        for (var i = armySqrt - 1; i >= 0; i--)
        {
            for (var j = 0; j < armySqrt; j++)
            {
                if (armyIndex == army.Count) break;
                var troop = Instantiate(army[armyIndex], spawnPoint, false);
                troop.transform.localPosition = new Vector3(xSpacing * i - xShift, 0, ySpacing * j - yShift);
                troop.Team = team;
                setTroops.Add(troop);
                armyIndex++;
            }

            if (armyIndex == army.Count) break;
        }

        return setTroops;
    }

    private int _initializedCounter = 0;
    private int _countTarget;
}