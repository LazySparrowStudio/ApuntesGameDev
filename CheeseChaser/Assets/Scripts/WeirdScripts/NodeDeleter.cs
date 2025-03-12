using UnityEngine;

public class NodeDeleter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Node")
        {
            Destroy(collision.gameObject);
        }
    }
}

