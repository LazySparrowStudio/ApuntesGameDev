using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodeStateEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStateEnum ghostNodeState;
    public GhostNodeStateEnum respawnState;
    public GhostNodeStateEnum startGhostNodeState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;
    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;
    public GameObject startingNode;
    public GameManager gameManager;

    public MovementController movementController;
    public GameObject[] scatterNodes;

    public bool readyToLeaveHome = false;
    public bool testRespawn = false;
    public bool isFrightened = false;
    public bool leftHomeBefore = false;
    public bool isVisible = true;
    public int scatterNodeIndex;
    public SpriteRenderer ghostSprite;
    public SpriteRenderer eyesSprite;
    public Animator animator;
    public Color color;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        scatterNodeIndex = 0;

        animator = GetComponent<Animator>();
        ghostSprite = GetComponent<SpriteRenderer>();
        movementController = GetComponent<MovementController>();

        if (ghostType == GhostType.red)
        {
            startGhostNodeState = GhostNodeStateEnum.startNode;
            startingNode = ghostNodeStart;
            respawnState = GhostNodeStateEnum.centerNode;

        }
        else if (ghostType == GhostType.pink)
        {
            startGhostNodeState = GhostNodeStateEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodeStateEnum.leftNode;
        }
        else if (ghostType == GhostType.blue)
        {
            startGhostNodeState = GhostNodeStateEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodeStateEnum.rightNode;
        }
        else if (ghostType == GhostType.orange)
        {
            startGhostNodeState = GhostNodeStateEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodeStateEnum.centerNode;
        }

        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

    }

    public void Setup()
    {
        animator.SetBool("moving", true);
        ghostNodeState = startGhostNodeState;
        readyToLeaveHome = false;

        //Reset our ghosts back to their home position
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

        movementController.direction = "";
        movementController.lastMovingDirection = "";

        scatterNodeIndex = 0;

        //Set isFrightened
        isFrightened = false;

        leftHomeBefore = false;

        //Set readyToLeaveHome to be false if they are red or pink
        if (ghostType == GhostType.red)
        {
            readyToLeaveHome = true;
            leftHomeBefore = true;
        }
        else if (ghostType == GhostType.pink)
        {
            readyToLeaveHome = true;
        }
        SetVisible(true);
    }

    void Update()
    {
        if (ghostNodeState != GhostNodeStateEnum.movingInNodes || !gameManager.isPowerPelletRunning)
        {
            isFrightened = false;
        }
        if (!gameManager.isPowerPelletRunning)
        {
            isFrightened = false;
        }
        //Show our sprites
        if (isVisible)
        {
            if (ghostNodeState != GhostNodeStateEnum.respawning)
            {
                ghostSprite.enabled = true;
            }
            else
            {
                ghostSprite.enabled = false;

            }

            ghostSprite.enabled = true;
            eyesSprite.enabled = true;
        }
        //Hide our sprites
        else
        {
            ghostSprite.enabled = false;
            eyesSprite.enabled = false;
        }

        if (isFrightened)
        {

            animator.SetBool("frightened", true);
            eyesSprite.enabled = false;
            ghostSprite.color = new Color(1f, 1f, 1f, 1f);
            animator.SetBool("frightenedBlinking", false);
        }
        else
        {
            animator.SetBool("frightened", false);
            ghostSprite.color = color;


        }
        if (!gameManager.isGameRunning)
        {
            return;
        }

        if (gameManager.powerPelletTimer - gameManager.currentPowerPelletTimer <= 3)
        {
            animator.SetBool("frightenedBlinking", true);
        }
        else
        {
            animator.SetBool("frightenedBlinking", false);
        }

        animator.SetBool("moving", true);

        if (testRespawn == true)
        {
            readyToLeaveHome = false;
            ghostNodeState = GhostNodeStateEnum.respawning;
            testRespawn = false;

        }

        if (movementController.currentNode.GetComponent<NodeController>().isSideNode == true)
        {
            movementController.SetSpeed(1f);
        }
        else
        {
            if (isFrightened)
            {
                movementController.SetSpeed(1);
            }
            else if (ghostNodeState == GhostNodeStateEnum.respawning)
            {
                movementController.SetSpeed(7);
            }
            else
            {
                movementController.SetSpeed(2);
            }

        }
    }

    public void SetFrightened(bool newIsFrightened)
    {
        isFrightened = newIsFrightened;
    }
    public void ReachedCenterofNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStateEnum.movingInNodes)
        {
            leftHomeBefore = true;
            //Scatter Mode
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                DetermineGhostScatterModeDirection();
            }
            //Frightened mode
            else if (isFrightened)
            {
                string direction = GetRandomDirection();
                movementController.SetDirection(direction);
            }
            //Chase Mode
            else
            // Determine next game node to go
            if (ghostType == GhostType.red)
            {
                DetermineRedGhostDirection();
            }
            else if (ghostType == GhostType.pink)
            {
                DeterminePinkGhostDirection();
            }
            else if (ghostType == GhostType.blue)
            {
                DetermineBlueGhostDirection();
            }
            else if (ghostType == GhostType.orange)
            {
                DetermineOrangeGhostDirection();
            }

        }
        else if (ghostNodeState == GhostNodeStateEnum.respawning)
        {
            string direction = "";
            // We have reached out start node, move to the center node
            float epsilon = 0.1f;
            if (transform.position.x == ghostNodeStart.transform.position.x &&
                Mathf.Abs(transform.position.y - ghostNodeStart.transform.position.y) < epsilon)
            {

                direction = "down";
                movementController.SetDirection(direction);

            }

            // We have reached out center node, respawn or moved to the center node
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStateEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                else if (respawnState == GhostNodeStateEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodeStateEnum.rightNode)
                {
                    direction = "right";
                }

            }
            // If our respawn state is either the left or right node, and we got to that node, leave home again
            else if
            ((transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
            || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y
            ))
            {
                ghostNodeState = respawnState;
            }
            //We are in the gameboard still, locate our start node
            else
            {
                // Determine quickest direct to home   
                direction = GetClosestDirection(ghostNodeStart.transform.position);
                movementController.SetDirection(direction);
            }


        }
        else
        {
            //If we are ready to leave our home
            if (readyToLeaveHome)
            {
                //If we are in the left home node, move to the center
                if (ghostNodeState == GhostNodeStateEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStateEnum.centerNode;
                    movementController.SetDirection("right");
                }
                //If we are in the right home node, move to the center
                else if (ghostNodeState == GhostNodeStateEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStateEnum.centerNode;
                    movementController.SetDirection("left");
                }
                //If we are in the center node, move to the start node
                else if (ghostNodeState == GhostNodeStateEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStateEnum.startNode;
                    movementController.SetDirection("up");
                }
                //If we are in the start node, start moving around in the game
                else if (ghostNodeState == GhostNodeStateEnum.startNode)
                {
                    ghostNodeState = GhostNodeStateEnum.movingInNodes;
                    movementController.SetDirection("left");
                }

            }
        }
    }


    string GetRandomDirection()
    {
        List<string> possibleDirections = new List<string>();
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveDown && movementController.lastMovingDirection != "up")
        {
            possibleDirections.Add("down");
        }
        if (nodeController.canMoveUp && movementController.lastMovingDirection != "down")
        {
            possibleDirections.Add("up");
        }
        if (nodeController.canMoveRight && movementController.lastMovingDirection != "left")
        {
            possibleDirections.Add("right");
        }
        if (nodeController.canMoveLeft && movementController.lastMovingDirection != "right")
        {
            possibleDirections.Add("left");
        }

        string direction = "";
        int randomDirectionIndex = Random.Range(0, possibleDirections.Count - 1);
        direction = possibleDirections[randomDirectionIndex];
        return direction;

    }
    void DetermineGhostScatterModeDirection()
    {
        {
            //If we reached the scatter node, add one to our scatter node index
            if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x && transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y)
            {
                scatterNodeIndex++;

                if (scatterNodeIndex == scatterNodes.Length - 1)
                {
                    scatterNodeIndex = 0;
                }
            }

            string direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
            movementController.SetDirection(direction);
        }
    }
    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {
        string pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmanDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "right")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "up")
        {
            target.y += distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);
    }

    void DetermineBlueGhostDirection()
    {

        string pacmanDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmanDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "right")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "up")
        {
            target.y += distanceBetweenNodes * 2;
        }
        else if (pacmanDirection == "down")
        {
            target.y -= distanceBetweenNodes * 2;
        }

        GameObject redGhost = gameManager.redGhost;
        float xDistance = target.x - redGhost.transform.position.x;
        float yDistance = target.y - redGhost.transform.position.y;

        Vector2 blueTarget = new Vector2(target.x - xDistance, target.y + yDistance);
        string direction = GetClosestDirection(blueTarget);
        movementController.SetDirection(direction);
    }

    void DetermineOrangeGhostDirection()
    {
        float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        float distanceBetweenNodes = 0.35f;
        if (distance < 0)
        {
            distance -= -1;

        }

        //If we are within 8 nodes of pacman, chase him using red´s logic
        if (distance <= distanceBetweenNodes * 8)
        {
            DetermineRedGhostDirection();
        }

        //Otherwise use scatter mode logic
        else
        {
            //Scatter mode
            DetermineGhostScatterModeDirection();
        }

    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        // IF we can move and and we aren´t reversing

        //UP
        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            //Get the node above us
            GameObject nodeUp = nodeController.nodeUp;

            // Get the distance between our top node, and pacman
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            //If this is the shortes distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";

            }
        }

        //DOWN
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            //Get the node below us
            GameObject nodeDown = nodeController.nodeDown;

            // Get the distance between our top node, and pacman
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            //If this is the shortes distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";

            }
        }

        //RIGHT
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            //Get the node above us
            GameObject nodeRight = nodeController.nodeRight;

            // Get the distance between our top node, and pacman
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            //If this is the shortes distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";

            }
        }

        //LEFT
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            //Get the node above us
            GameObject nodeLeft = nodeController.nodeLeft;

            // Get the distance between our top node, and pacman
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            //If this is the shortes distance so far, set our direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";

            }
        }

        return newDirection;

    }

    public void SetVisible(bool newIsVisible)
    {
        isVisible = newIsVisible;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && ghostNodeState != GhostNodeStateEnum.respawning)
        {
            //Get eaten
            if (isFrightened)
            {
                gameManager.GhostEaten(this);
                ghostNodeState = GhostNodeStateEnum.respawning;
            }
            //Eat player
            else
            {
                StartCoroutine(gameManager.PlayerEaten());
            }
        }
    }
}
