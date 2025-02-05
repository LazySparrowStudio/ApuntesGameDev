using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject currentNode;
    [SerializeField] public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        //Figure out if we´re at the center of our current node
        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y)
        {
            // Get the next node from out node controller using our current direction
            GameObject newNode = currentNodeController.GetNodeFromDirection(direction);
            if (newNode != null)
            {
                currentNode = newNode;
                lastMovingDirection = direction;
            }
            // We cant move in the desired direction, try to keep going in the last moving direction
            else
            {
                direction = lastMovingDirection;
                newNode = currentNodeController.GetNodeFromDirection(direction);
                if(newNode != null)
                {
                    currentNode = newNode;
                }

            }
        }
    }

        public void SetDirection (string newDirection)
        {
            direction = newDirection; 
        }
    
}

