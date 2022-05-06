using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<float> _timeScales = new List<float> { 0.25f, 0.5f, 1f, 2f, 4f };
    public List<Troop> FirstArmy;
    public List<Troop> SecondArmy;
    public Action BattleReady;
    public Action<float> TimeScaleChanged;


    public BattleController GetBattleController()
    {
        return _battleController;
    }
    
    private void Start()
    {
        _gameSetter = GetComponent<GameSetter>();
        _gameSetter.ArmiesSet += TroopsReady;
        _battleController = _gameSetter.SetArmies(FirstArmy, SecondArmy);
        _controllerReady = true;
        
        SetupTimeScale();
    }
    private void Update()
    {
        if (_troopsReady && _controllerReady && !_battleReady)
        {
            _battleReady = true;
            BattleReady?.Invoke();
            Debug.Log("battle is ready, press F to start...");
        }
        
        if(Input.GetKeyDown(KeyCode.F) && _battleReady) _battleController.Play();
        
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            if (_battleSpeedIndex > 0) _battleSpeedIndex--;
            Time.timeScale = _timeScales[_battleSpeedIndex];
            TimeScaleChanged?.Invoke(Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (_battleSpeedIndex < _timeScales.Count-1) _battleSpeedIndex++;
            Time.timeScale = _timeScales[_battleSpeedIndex];
            TimeScaleChanged?.Invoke(Time.timeScale);
        }
        
    }

    private void TroopsReady()
    {
        _troopsReady = true;
    }
    
    private void SetupTimeScale()
    {
        _battleSpeedIndex = _timeScales.IndexOf(1f);
        if (_battleSpeedIndex == -1) _battleSpeedIndex = 0;
        Time.timeScale = _timeScales[_battleSpeedIndex];
        TimeScaleChanged?.Invoke(Time.timeScale);
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
    private int _battleSpeedIndex;
}