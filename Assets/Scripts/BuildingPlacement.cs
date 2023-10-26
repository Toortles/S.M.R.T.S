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
    private LayerMask _terrainMask;
    private BoxCollider _placerCollider;
    private bool _canPlace = true;

    private void Start()
    {
        _placerMeshRenderer = GetComponent<MeshRenderer>();
        _terrainMask = LayerMask.GetMask("Terrain");
        _placerCollider = GetComponent<BoxCollider>();
        SetBuilding("Headquarters");
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000f, _terrainMask);

        Vector3 snapPosition = (Vector3) Vector3Int.RoundToInt(hit.point / 0.5f) * 0.5f;
        transform.position = Vector3.Lerp(transform.position, snapPosition, Time.deltaTime * 50);
        transform.Rotate(0f, Input.mouseScrollDelta.y * 15, 0f);

        _placerMeshRenderer.material.color = _canPlace ? Color.blue: Color.red;
        
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

        BoxCollider buildingCollider = _currentBuilding.GetComponent<BoxCollider>();
        _placerCollider.size = buildingCollider.size;
        _placerCollider.center = buildingCollider.center;
    }

    private void OnTriggerEnter(Collider other)
    {
        _canPlace = false;
    }

    private void OnTriggerExit(Collider other)
    {
        _canPlace = true;
    }
}
