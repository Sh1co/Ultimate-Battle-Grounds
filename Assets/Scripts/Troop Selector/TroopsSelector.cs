using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsSelector : MonoBehaviour
{
    [SerializeField] private int _selectionTeam = 1;
    [SerializeField] private LayerMask _troopsLayerMask;
    [SerializeField] private LayerMask _groundLayerMask;

    private void Start()
    {
        _selectedTroops = GetComponent<SelectedTroopsDict>();
        _dragSelect = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragPoint1 = Input.mousePosition;
        }
        
        if (Input.GetMouseButton(0))
        {
            if((_dragPoint1 - Input.mousePosition).magnitude > 40)
            {
                _dragSelect = true;
            }
        }
        
        if (!Input.GetMouseButtonUp(0)) return;
        if(_dragSelect == false) //single select
        {
            Ray ray = Camera.main.ScreenPointToRay(_dragPoint1);

            if(Physics.Raycast(ray,out _raycastHit, int.MaxValue,_troopsLayerMask))
            {
                if (_raycastHit.transform.GetComponent<Troop>().Team == 1)
                {
                    if (Input.GetKey(KeyCode.LeftShift)) //inclusive select
                    {
                        _selectedTroops.Add(_raycastHit.transform.GetComponent<Troop>());
                    }
                    else //exclusive selected
                    {
                        _selectedTroops.RemoveAll();
                        _selectedTroops.Add(_raycastHit.transform.GetComponent<Troop>());
                    }
                }
            }
            else //if we didnt hit something
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _selectedTroops.RemoveAll();
                }
            }
        }
        else //drag select
        {
            verts = new Vector3[4];
            vecs = new Vector3[4];
            int index = 0;
            _dragPoint2 = Input.mousePosition;
            _selectionCorners = getBoundingBox(_dragPoint1, _dragPoint2);

            foreach (Vector2 corner in _selectionCorners)
            {
                Ray ray = Camera.main.ScreenPointToRay(corner);

                if (Physics.Raycast(ray, out _raycastHit, int.MaxValue, _groundLayerMask))
                {
                    verts[index] = new Vector3(_raycastHit.point.x, _raycastHit.point.y, _raycastHit.point.z);
                    vecs[index] = ray.origin - _raycastHit.point;
                }
                index++;
            }
            
            
            _selectionMesh = generateSelectionMesh(verts,vecs);
            _selectionBox = gameObject.AddComponent<MeshCollider>();
            _selectionBox.sharedMesh = _selectionMesh;
            _selectionBox.convex = true;
            _selectionBox.isTrigger = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                _selectedTroops.RemoveAll();
            }

            Destroy(_selectionBox, Time.fixedDeltaTime);

        }

        _dragSelect = false;

    }

    private void OnGUI()
    {
        if(_dragSelect == true)
        {
            var rect = GraphicsUtils.GetScreenRect(_dragPoint1, Input.mousePosition);
            GraphicsUtils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            GraphicsUtils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    private Vector2[] getBoundingBox(Vector2 point1,Vector2 point2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(point1, point2);
        var topRight = Vector3.Max(point1, point2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }

    private Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] trisMap = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for(int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for(int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = trisMap;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Troop>() == null) return;
        var troop = other.GetComponent<Troop>();
        if (troop.Team != _selectionTeam) return;
        _selectedTroops.Add(other.GetComponent<Troop>());
    }
    
    
    private SelectedTroopsDict _selectedTroops;
    private RaycastHit _raycastHit;
    private bool _dragSelect;
    private MeshCollider _selectionBox;
    private Mesh _selectionMesh;
    private Vector3 _dragPoint1;
    private Vector3 _dragPoint2;
    private Vector2[] _selectionCorners;

    //the vertices of our meshcollider
    Vector3[] verts;
    Vector3[] vecs;
}