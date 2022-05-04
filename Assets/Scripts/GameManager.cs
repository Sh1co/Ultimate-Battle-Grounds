using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public List<Troop> FirstArmy;
    public List<Troop> SecondArmy;

    private void Start()
    {
        _gameSetter = GetComponent<GameSetter>();
        _gameSetter.ArmiesSet += TroopsReady;
        _battleController = _gameSetter.SetArmies(FirstArmy, SecondArmy);
        _controllerReady = true;
    }

    private void Update()
    {
        if (_troopsReady && _controllerReady && !_battleReady)
        {
            _battleReady = true;
            Debug.Log("battle is ready");
        }
        
        if(Input.GetKeyDown(KeyCode.F) && _battleReady) _battleController.Play();
        
    }

    private void TroopsReady()
    {
        _troopsReady = true;
    }

    private void OnDisable()
    {
        _gameSetter.ArmiesSet -= TroopsReady;
    }

    private BattleController _battleController;
    private GameSetter _gameSetter;
    private bool _troopsReady;
    private bool _controllerReady;
    private bool _battleReady;
}