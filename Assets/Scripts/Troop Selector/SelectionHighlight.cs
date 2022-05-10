using UnityEngine;

public class SelectionHighlight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _ogColor = GetComponent<Renderer>().material.color; 
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnDestroy()
    {
        GetComponent<Renderer>().material.color = _ogColor;
    }

    private Color _ogColor;

}