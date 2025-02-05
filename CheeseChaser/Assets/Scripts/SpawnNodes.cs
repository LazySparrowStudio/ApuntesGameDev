using UnityEngine;

public class SpawnNodes : MonoBehaviour
{
    [SerializeField] int numToSpawn = 28;
    [SerializeField] public float spawnOffset = 0.255f;
    public float currentSpawnOffset;
    void Start()
    {
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
    }

    void Update()
    {
        
    }
}
