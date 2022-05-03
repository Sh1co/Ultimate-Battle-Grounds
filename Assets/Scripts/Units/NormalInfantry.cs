using UnityEngine;


public class NormalInfantry : Troop
{
    

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}