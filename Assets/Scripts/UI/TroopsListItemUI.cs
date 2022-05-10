using System;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopsListItemUI : MonoBehaviour
{
    [SerializeField] private Dropdown _troopTypeSelector;
    [SerializeField] private TMP_InputField _troopCount;

    public string GetTroopType()
    {
        Debug.Log(_troopTypeSelector.options[_troopTypeSelector.value].text);
        return _troopTypeSelector.options[_troopTypeSelector.value].text;
    }

    public void AddOption(string option)
    {
        _troopTypeSelector.options.Add(new Dropdown.OptionData(option));
    }

    public int GetTroopCount()
    {
        return Convert.ToInt32(_troopCount.text);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
    
}