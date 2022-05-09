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
            //go.AddComponent<selection_component>();
        }
    }

    public void Remove(int id)
    {
        //Destroy(selectedTable[id].GetComponent<selection_component>());
        SelectedTroops.Remove(id);
    }

    public void RemoveAll()
    {
        /*foreach(KeyValuePair<int,Troop> pair in SelectedTroops)
        {
            if(pair.Value != null)
            {
                //Destroy(selectedTable[pair.Key].GetComponent<selection_component>());
            }
        }*/
        SelectedTroops.Clear();
    }
}