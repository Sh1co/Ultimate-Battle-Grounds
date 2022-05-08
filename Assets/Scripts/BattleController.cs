using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class BattleController
{
    public List<Troop> FirstArmy;
    public List<Troop> SecondArmy;

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

    public void SetTroopsEnemies()
    {
        foreach (var troop in FirstArmy)
        {
            troop.Enemies = SecondArmy;
        }

        foreach (var troop in SecondArmy)
        {
            troop.Enemies = FirstArmy;
        }
    }

    public void StartTroopSearch()
    {
        foreach (var troop in FirstArmy)
        {
            troop.FindNewTarget();
        }

        foreach (var troop in SecondArmy)
        {
            troop.FindNewTarget();
        }
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
}