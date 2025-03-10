using UnityEngine;

public class SpawnNodes : MonoBehaviour
{
    [SerializeField] private int numToSpawnX; // Cantidad de nodos en X
    [SerializeField] private int numToSpawnY; // Cantidad de nodos en Y
    [SerializeField] private float spawnOffsetX; // Espaciado en X
    [SerializeField] private float spawnOffsetY; // Espaciado en Y
    [SerializeField] private GameObject nodePrefab; // Prefab del nodo

    void Start()
    {
        SpawnGrid();
    }

    private void SpawnGrid()
    {
        if (nodePrefab == null)
        {
            Debug.LogError("Node Prefab is not assigned!");
            return;
        }
        
        Vector3 startPosition = nodePrefab.transform.position;
        
        for (int y = 0; y < numToSpawnY; y++) // Filas en Y
        {
            for (int x = 0; x < numToSpawnX; x++) // Columnas en X
            {
                Vector3 spawnPosition = new Vector3(
                    startPosition.x + (x * spawnOffsetX), 
                    startPosition.y + (y * spawnOffsetY), 
                    startPosition.z);
                
                GameObject clone = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, transform);
                clone.name = $"Node_{x}_{y}";
            }
        }
    }
}