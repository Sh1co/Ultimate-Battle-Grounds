using System.Collections.Generic;
using UnityEngine;

public class SelectedTroopsDict : MonoBehaviour
{
    public readonly Dictionary<int, Troop> SelectedTroops = new Dictionary<int, Troop>();

    public void Add(Troop troop)
    {
        int id = troop.GetInstanceID();

        if (!(SelectedTroops.ContainsKey(id)))
        {
            SelectedTroops.Add(id, troop);
            troop.gameObject.AddComponent<SelectionHighlight>();
        }
    }

    public void Remove(int id)
    {
        Destroy(SelectedTroops[id].GetComponent<SelectionHighlight>());
        SelectedTroops.Remove(id);
    }

    public void RemoveAll()
    {
        foreach(KeyValuePair<int,Troop> pair in SelectedTroops)
        {
            if(pair.Value != null)
            {
                Destroy(SelectedTroops[pair.Key].GetComponent<SelectionHighlight>());
            }
        }
        SelectedTroops.Clear();
    }
}