using System;
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

    public bool readyToLeaveHome = false;


    public bool testRespawn = false;
    public bool isFrightened = false;
    public GameObject[] scatterNodes;
    public int scatterNodeIndex;
    void Awake()
    {
        scatterNodeIndex = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStateEnum.startNode;
            respawnState = GhostNodeStateEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStateEnum.centerNode;
            respawnState = GhostNodeStateEnum.leftNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStateEnum.leftNode;
            respawnState = GhostNodeStateEnum.rightNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStateEnum.rightNode;
            respawnState = GhostNodeStateEnum.centerNode;
            startingNode = ghostNodeRight;
        }

        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

    }

    void Update()
    {
        if (testRespawn == true)
        {
            ghostNodeState = GhostNodeStateEnum.respawning;
            testRespawn = false;
            readyToLeaveHome = true;
        }

    }

    public void ReachedCenterofNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStateEnum.movingInNodes)
        {
            //Scatter Mode
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
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
            }
            //Frightened mode
            else if (isFrightened)
            {

            }
            //Chase Mode
            else 
            // Determine next game node to go
            if (ghostType == GhostType.red)
            {
                DetermineRedGhostDirection();
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

    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {

    }

    void DetermineBlueGhostDirection()
    {

    }

    void DetermineOrangeGhostDirection()
    {

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
}
