using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerRedScreen : MonoBehaviour
{
    public GhostNodeStateEnum startGhostNodeState;

    public GameObject[] scatterNodes;
    public bool isRespawning = false;
    public bool isFrightened = false;
    public bool isVisible = true;
    public int scatterNodeIndex;
    public GameObject ghostPrefab;
    public GameObject startingNode;
    public GameManagerRedScreen gameManager;
    public SpriteRenderer ghostSprite;
    public SpriteRenderer eyesSprite;
    public Animator animator;
    public Color color;
    public enum GhostNodeStateEnum
    {
        respawning,
        movingInNodes
    }
    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }
    public GhostType ghostType;

    public GhostNodeStateEnum ghostNodeState;


    public MovementControllerRedScreen movementController;
    string hexaColor = "00FFDC";
    public Color newColor;
    public Color alphaColor = new Color(1f, 1f, 1f, 1f);

    void Awake()
    {
        gameManager = GameObject.Find("GameManagerRedScreen").GetComponent<GameManagerRedScreen>();
        movementController.currentNode = startingNode;
        startGhostNodeState = GhostNodeStateEnum.movingInNodes;

        scatterNodeIndex = 0;
    }

    public void Start()
    {

        animator = GetComponent<Animator>();

        if (ColorUtility.TryParseHtmlString(hexaColor, out newColor))
        {
            ghostSprite.color = newColor;
        }

        eyesSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        scatterNodeIndex = 0;
        ghostSprite.sortingOrder = 4;
        eyesSprite.sortingOrder = 5;

    }

    public void Setup()
    {
        //Debugging
        if (gameObject.GetComponent<Animator>() == null)
        {
            animator = GetComponent<Animator>();
        }

        if (startingNode == null)
        {
            startingNode = GameObject.Find("PlaceHolderNode");
        }
        animator.SetBool("moving", true);
        ghostNodeState = startGhostNodeState;


        //Reset our ghosts back to their home position
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;

        movementController.direction = "";
        movementController.lastMovingDirection = "";

        scatterNodeIndex = 0;

        //Reset isFrightened
        isFrightened = false;
        ghostSprite.enabled = true;

        SetVisible(true);
    }

    void Update()
    {

        if (ghostNodeState != GhostNodeStateEnum.movingInNodes || !gameManager.isPowerPelletRunning)
        {
            isFrightened = false;
            animator.speed = 0.3f;
        }
        else
        {
            animator.speed = 1;
        }

        //Show our sprites
        if (isVisible)
        {
            if (ghostNodeState == GhostNodeStateEnum.respawning)
            {
                ghostSprite.enabled = false;
            }
            else
            {
                ghostSprite.enabled = true;
                eyesSprite.enabled = true;

            }
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
            ghostSprite.color = alphaColor;
            animator.SetBool("frightenedBlinking", false);
        }
        else
        {
            animator.SetBool("frightened", false);
            ghostSprite.color = newColor;


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

        if (isRespawning == true)
        {

            ghostNodeState = GhostNodeStateEnum.respawning;
            isRespawning = false;

        }

        if (movementController.currentNode.GetComponent<NodeControllerRedScreen>().isSideNode == true)
        {
            if (ghostNodeState == GhostNodeStateEnum.respawning)
            {
                movementController.SetSpeed(5f);
            }
            else movementController.SetSpeed(1f);

        }
        else if (isFrightened)
        {
            movementController.SetSpeed(1);
        }
        else if (ghostNodeState == GhostNodeStateEnum.respawning)
        {
            movementController.SetSpeed(5);
        }
        else
        {
            movementController.SetSpeed(2);
        }
    }
    public void SetFrightened(bool newIsFrightened)
    {
        isFrightened = newIsFrightened;
    }

    //Movimiento
    public void ReachedCenterofNode(NodeControllerRedScreen nodeController)
    {
        if (ghostNodeState == GhostNodeStateEnum.movingInNodes)
        {
            //Scatter Mode
            if (gameManager.currentGhostMode == GameManagerRedScreen.GhostMode.scatter)
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
            {
                DetermineRedGhostDirection();
            }

        }
        else if (ghostNodeState == GhostNodeStateEnum.respawning)
        {
            string direction = "";
            {
                // Determine quickest direct to home   
                direction = GetClosestDirection(startingNode.transform.position);
                movementController.SetDirection(direction);
            }
        }
    }


    string GetRandomDirection()
    {
        List<string> possibleDirections = new List<string>();
        NodeControllerRedScreen nodeController = movementController.currentNode.GetComponent<NodeControllerRedScreen>();

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
        if (isFrightened && (nodeController.isWarpLeftNode || nodeController.isWarpRightNode))
        {
            direction = movementController.lastMovingDirection;
            return direction;

        }
        else

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

        GameObject redGhost = gameManager.redGhostPrefab;
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
        NodeControllerRedScreen nodeController = movementController.currentNode.GetComponent<NodeControllerRedScreen>();

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
