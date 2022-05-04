using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class BattleController
{
    public List<Troop> FirstArmy;
    public List<Troop> SecondArmy;

    public void Play()
    {
        foreach (var troop in FirstArmy.Where(troop => troop != null))
        {
            troop.Play();
        }

        foreach (var troop in SecondArmy.Where(troop => troop != null))
        {
            troop.Play();
        }
    }

    public void Pause()
    {
        foreach (var troop in FirstArmy.Where(troop => troop != null))
        {
            troop.Pause();
        }

        foreach (var troop in SecondArmy.Where(troop => troop != null))
        {
            troop.Pause();
        }
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
}