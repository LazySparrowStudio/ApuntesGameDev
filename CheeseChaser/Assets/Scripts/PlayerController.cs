using UnityEngine;
using System.Collections;
using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{

    MovementController movementController;
    public SpriteRenderer sprite;
    public Animator animator;
    public GameObject startNode;
    public Vector2 startPosition;
    public GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPosition = new Vector2(0f, -0.17f);
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        movementController = GetComponent<MovementController>();
        startNode = movementController.currentNode;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameRunning == false)
        {
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
        movementController.currentNode = startNode;
        movementController.lastMovingDirection = "left";
        transform.position = startPosition;
        animator.SetBool("moving", false);
    }
}
