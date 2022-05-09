using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BattleController
{
    public readonly List<Troop> FirstArmy;
    public readonly List<Troop> SecondArmy;
    public BattleController(List<Troop> firstArmy, List<Troop> secondArmy, SelectedTroopsDict selectedDict,
        float xSpacing, float ySpacing)
    {
        FirstArmy = firstArmy;
        SecondArmy = secondArmy;
        _selectedTroopsDict = selectedDict;
        _xSpacing = xSpacing;
        _ySpacing = ySpacing;
    }

    public void Play()
    {
        PlayArmy(FirstArmy);
        PlayArmy(SecondArmy);
    }

    public void Pause()
    {
        PauseArmy(FirstArmy);
        PauseArmy(SecondArmy);
    }

    public void OrderSelectedToPosition(Vector3 position)
    {
        var positionsList = GetListOfPositions(position, _selectedTroopsDict.SelectedTroops.Count);
        var index = 0;
        
        foreach (var pair in _selectedTroopsDict.SelectedTroops.Where(pair => pair.Value != null))
        {
            pair.Value.GoToPosition(positionsList[index]);
            index++;
        }
    }

    private List<Vector3> GetListOfPositions(Vector3 center, int count)
    {
        var formationSqrt = Mathf.Ceil(Mathf.Sqrt(count));
        var xShift = Mathf.FloorToInt((formationSqrt / 2) * _xSpacing);
        var yShift = Mathf.FloorToInt((formationSqrt / 2) * _ySpacing);
        var positionsList = new List<Vector3>();
        
        for (var i = formationSqrt - 1; i >= 0; i--)
        {
            for (var j = 0; j < formationSqrt; j++)
            {
                if (count == 0) break;
                positionsList.Add(new Vector3(center.x + _xSpacing * i - xShift, center.y, center.z + _ySpacing * j - yShift));
                count--;
            }

            if (count == 0) break;
        }

        return positionsList;
    }

    private void PlayArmy(List<Troop> army)
    {
        foreach (var troop in army.Where(troop => troop != null))
        {
            troop.Play();
        }
    }
    private void PauseArmy(List<Troop> army)
    {
        foreach (var troop in army.Where(troop => troop != null))
        {
            troop.Pause();
        }
    }
    private SelectedTroopsDict _selectedTroopsDict;
    private float _xSpacing;
    private float _ySpacing;
}