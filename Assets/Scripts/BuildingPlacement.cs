using System;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    
    [SerializeField]
    private string[] buildingNames;
    [SerializeField]
    private GameObject[] buildingPrefabs;
    private GameObject _currentBuilding;

    private MeshRenderer _placerMeshRenderer;
    private BoxCollider _placerCollider;
    private LayerMask _terrainMask;
    private bool _canPlace = true;

    private void Start()
    {
        SetBuilding("Headquarters");
        _placerMeshRenderer = GetComponent<MeshRenderer>();
        Debug.Log(gameObject.GetComponent<BoxCollider>());
        _placerCollider = gameObject.GetComponent<BoxCollider>();
        Debug.Log(_placerCollider);
        _terrainMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, _terrainMask);

        transform.position = hit.point;
        transform.Rotate(0f, Input.mouseScrollDelta.y * 20, 0f);

        //_placerMeshRenderer.material.color = _canPlace ? Color.blue: Color.red;
        
        if (Input.GetMouseButtonDown(0) && hit.collider && _canPlace)
        {
            Instantiate(_currentBuilding, hit.point, transform.rotation);
        }
    }

    public void SetBuilding(string buildingType)
    {
        //Corresponding Index of other array
        _currentBuilding = buildingPrefabs[Array.IndexOf(buildingNames, buildingType)];
        
        //Mesh Correction
        Mesh mesh = _currentBuilding.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<MeshFilter>().sharedMesh = mesh;
        
        //Box Collider Correction
        BoxCollider buildingCollider = _currentBuilding.GetComponent<BoxCollider>();
        //Debug.Log(_placerCollider);

        if (_placerCollider != null)
            _placerCollider.size.Set(buildingCollider.size.x, buildingCollider.size.y, buildingCollider.size.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        _canPlace = false;
    }

    private void OnCollisionExit(Collision other)
    {
        _canPlace = true;
    }
}
