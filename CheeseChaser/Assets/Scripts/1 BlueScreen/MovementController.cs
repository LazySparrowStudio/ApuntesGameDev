using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject currentNode;
    public GameObject nodeToTheLeft;
    public GameObject nodeToTheRight;
    [SerializeField] public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";

    public bool canWarp;
    public bool isGhost = false;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //nodeToTheLeft = currentNode.GetComponent<NodeController>().nodeLeft;
        // nodeToTheRight = currentNode.GetComponent<NodeController>().nodeRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameRunning == false)
        {
            return;
        }
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

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
                GetComponent<EnemyController>().ReachedCenterofNode(currentNodeController);
            }

            // If we reached the center of the left warp, warp to the right warp
            if (currentNodeController.isWarpLeftNode && canWarp)
            {
                /*currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
                */
            }   // If we reached the center of the right warp, warp to the left warp
            else if (currentNode == currentNodeController.isWarpRightNode && canWarp)
            {
                /*  currentNode = gameManager.leftWarpNode;
                  direction = "right";
                  lastMovingDirection = "right";
                  transform.position = currentNode.transform.position;
                  canWarp = false;
              */ 
                int currentScene = SceneManager.GetActiveScene().buildIndex;
                int nextScene = currentScene + 1;

                if (nextScene == SceneManager.sceneCountInBuildSettings)
                {
                    nextScene = 0;
                }
                SceneManager.LoadScene(nextScene);
            }

            // Otherwise, find the next node we are going to be moving towards
            else
            {
                //If we are not a ghost that is respawning, and we are on the start node, and we are trying to move down, stop
                if (currentNodeController.isGhostStartingNode && direction == "down"
                && (!isGhost || GetComponent<EnemyController>().ghostNodeState != EnemyController.GhostNodeStateEnum.respawning))
                {
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

