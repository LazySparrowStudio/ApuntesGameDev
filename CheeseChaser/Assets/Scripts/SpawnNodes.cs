using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnNodes : MonoBehaviour
{
    
    [SerializeField] int numToSpawn;
    [SerializeField] public float spawnOffset;
    public float currentSpawnOffset;
    void Start()
    {
        /*gameObject.name = "Node";
        return;
        if (gameObject.name == "Node")
        {
            currentSpawnOffset = spawnOffset;
            for (int i = 0; i < numToSpawn; i++) 
            {
                //Clone a new node
                GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x , transform.position.y + currentSpawnOffset,0), Quaternion.identity);
                currentSpawnOffset += spawnOffset;
            }
        }
        */
    }

    void Update()
    {
        
    }
}
