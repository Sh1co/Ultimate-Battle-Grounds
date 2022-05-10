using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSetup : MonoBehaviour
{

    [SerializeField] private string _battleSceneName = "Testing";
    [SerializeField] private GameObject _battleSetup;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private TroopsListItemUI _listItemPrefab;
    [SerializeField] private List<Troop> _allyTroopTypes;
    [SerializeField] private Transform _allyTroopsList;
    [SerializeField] private List<Troop> _enemyTroopTypes;
    [SerializeField] private Transform _enemyTroopsList;


    private void Start()
    {
        BattleArmies.FirstArmy = new List<Troop>();
        BattleArmies.SecondArmy = new List<Troop>();
        
        foreach (var troopType in _allyTroopTypes)
        {
            _allyTroopsDict.Add(troopType.name, troopType);
        }
        
        foreach (var troopType in _enemyTroopTypes)
        {
            _enemyTroopsDict.Add(troopType.name, troopType);
        }
    }

    public void StartBattle()
    {
        CreateArmy(_selectedAllyTroops, BattleArmies.FirstArmy, _allyTroopsDict);
        CreateArmy(_selectedEnemyTroops, BattleArmies.SecondArmy, _enemyTroopsDict);
        SceneManager.LoadScene(_battleSceneName);
    }
    
    public void AddListItem(bool ally)
    {
        if(ally)_selectedAllyTroops.Add(AddToList(_allyTroopsList, _allyTroopsDict));
        else _selectedEnemyTroops.Add(AddToList(_enemyTroopsList, _enemyTroopsDict));
    }

    public void ToggleSetupPanel(bool state)
    {
        _battleSetup.SetActive(state);
        _mainMenu.SetActive(!state);
    }

    private void CreateArmy(List<TroopsListItemUI> selectionList, List<Troop> troopList, Dictionary<string, Troop> troopTypeDict)
    {
        foreach (var listItemUI in selectionList)
        {
            var typeName = listItemUI.GetTroopType();
            var type = troopTypeDict[typeName];
            var count = listItemUI.GetTroopCount();
            for (var i = 0; i < count; i++)
            {
                troopList.Add(type);
            }
        }
    }
    private TroopsListItemUI AddToList(Transform list, Dictionary<string, Troop> troopTypeDict)
    {
        var listItem = Instantiate(_listItemPrefab, list);
        listItem.transform.SetSiblingIndex(listItem.transform.parent.childCount - 2);
        foreach(var pair in troopTypeDict)
        {
            listItem.AddOption(pair.Key);
        }
        return listItem;
    }

    private List<TroopsListItemUI> _selectedAllyTroops = new List<TroopsListItemUI>();
    private List<TroopsListItemUI> _selectedEnemyTroops = new List<TroopsListItemUI>();
    private Dictionary<string, Troop> _allyTroopsDict = new Dictionary<string, Troop>();
    private Dictionary<string, Troop> _enemyTroopsDict = new Dictionary<string, Troop>();
}