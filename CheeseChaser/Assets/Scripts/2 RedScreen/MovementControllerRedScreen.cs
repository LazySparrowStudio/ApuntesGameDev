using UnityEngine;

public class MovementControllerRedScreen : MonoBehaviour
{
    public GameManagerRedScreen gameManager;
    public GameObject currentNode;
    [SerializeField] public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";

    public bool canWarp;
    public bool isGhost = false;
    void Awake()
    {
        gameManager = GameObject.Find("GameManagerRedScreen").GetComponent<GameManagerRedScreen>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameRunning == false)
        {
            return;
        }
        NodeControllerRedScreen currentNodeController = currentNode.GetComponent<NodeControllerRedScreen>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;

        if (direction == "left" && lastMovingDirection == "right"
        || direction == "right" && lastMovingDirection == "left"
        || direction == "up" && lastMovingDirection == "down"
        || direction == "down" && lastMovingDirection == "up")
        {
            reverseDirection = true;
        }
        //Figure out if we´re at the center of our current node
        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y || reverseDirection)
        {
            if (isGhost)
            {
                GetComponent<EnemyControllerRedScreen>().ReachedCenterofNode(currentNodeController);
            }

            // If we reached the center of the left warp, warp to the right warp

            // Otherwise, find the next node we are going to be moving towards
            else
            {
                //If we are not a ghost that is respawning, and we are on the start node, and we are trying to move down, stop
                if (currentNodeController.isGhostStartingNode && direction == "down"
                && (!isGhost || GetComponent<EnemyControllerRedScreen>().ghostNodeState != EnemyControllerRedScreen.GhostNodeStateEnum.respawning))
                {
                    lastMovingDirection = "down";
                    direction = lastMovingDirection;
                }
                // Get the next node from out node controller using our current direction
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);

                // If we can move in the desire direction
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
                    if (newNode != null)
                    {
                        currentNode = newNode;
                    }

                }
            }

        }
        // We are not in the center of a node
        else
        {
            canWarp = true;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }

}

