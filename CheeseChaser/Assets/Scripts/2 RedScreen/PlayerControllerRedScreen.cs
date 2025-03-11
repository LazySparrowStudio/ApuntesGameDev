using UnityEngine;
using System.Collections;
using UnityEditor.Tilemaps;

public class PlayerControllerRedScreen : MonoBehaviour
{

    MovementControllerRedScreen movementController;
    public SpriteRenderer sprite;
    public Animator animator;
    public GameObject startNode;
    private Vector2 startPosition;
    public GameManagerRedScreen gameManager;
    public bool isDead = false;
    void Awake()
    {
        gameManager = GameObject.Find("GameManagerRedScreen").GetComponent<GameManagerRedScreen>();

        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = 2;
        movementController = GetComponent<MovementControllerRedScreen>();
        startPosition = startNode.transform.position;
        




    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isGameRunning)
        {
            if (!isDead)
            {
                animator.speed = 0;
            }
            animator.speed = 1;
            return;
        }

        animator.SetBool("moving", true);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }


        bool flipX = false;
        bool flipY = false;



        if (movementController.lastMovingDirection == "left")
        {
            animator.SetInteger("direction", 0);
        }
        else if (movementController.lastMovingDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }
        else if (movementController.lastMovingDirection == "up")
        {
            animator.SetInteger("direction", 1);
        }
        else if (movementController.lastMovingDirection == "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }

        sprite.flipX = flipX;
        sprite.flipY = flipY;

    }

    public void Setup()
    {
        animator.speed = 1;
        isDead = false;
        animator.SetBool("dead", false);

        transform.position = startPosition;
        movementController.currentNode = startNode;
        movementController.direction = "right";
        movementController.lastMovingDirection = "right";
        sprite.flipX = false;


        animator.SetBool("moving", false);

    }

    public void Stop()
    {
        animator.speed = 0;
    }

    public void Death()
    {
        animator.speed = 1;
        isDead = true;
        animator.SetBool("moving", false);
        animator.SetBool("dead", true);


    }
}
