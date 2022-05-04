using System.Collections.Generic;
using UnityEngine.AI;

public class BattleController
{
    public List<Troop> FirstArmy;
    public List<Troop> SecondArmy;

    public void Play()
    {
        foreach (var troop in FirstArmy)
        {
            troop.Play();
        }
        foreach (var troop in SecondArmy)
        {
            troop.Play();
        }
    }

    public void Pause()
    {
        foreach (var troop in FirstArmy)
        {
            troop.Pause();
        }
        foreach (var troop in SecondArmy)
        {
            troop.Pause();
        }
    }
}