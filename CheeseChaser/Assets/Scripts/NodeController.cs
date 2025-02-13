using UnityEngine;

public class NodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
    public bool canMoveRight = false;
    public bool canMoveUp = false;
    public bool canMoveDown = false;

    public GameObject nodeLeft;
    public GameObject nodeRight;
    public GameObject nodeUp;
    public GameObject nodeDown;

    public SpriteRenderer pelletSprite;
    public GameManager gameManager;
    public bool isWarpRightNode = false;
    public bool isWarpLeftNode = false;
    
    //If the node contains a pellet when the game starts
    public bool isPelletNode = false;

    //If the node still has a pellet
    public bool hasPellet = false;
    public bool isGhostStartingNode = false;
    void Awake()
    {
        PelletRayCast();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (transform.childCount > 0)
        {
            hasPellet = true;
            isPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }
     //   OnDrawGizmos();
       
    }

    //Visualizar RayCast
   /* void OnDrawGizmos()
{
    float rayDistace = 0.26f;
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistace);
    Gizmos.DrawLine(transform.position, transform.position + Vector3.up * rayDistace);
    Gizmos.DrawLine(transform.position, transform.position + Vector3.right * rayDistace);
    Gizmos.DrawLine(transform.position, transform.position + Vector3.left * rayDistace);
}*/  
    private void PelletRayCast()
    {
        int pelletLayer = LayerMask.GetMask("Node");
        float rayDistace = 0.3f;
        RaycastHit2D[] hitsDown;
        RaycastHit2D[] hitsUp;
        RaycastHit2D[] hitsRight;
        RaycastHit2D[] hitsLeft;

        //Shoot raycast going DOWN
        hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down, rayDistace, pelletLayer);

        // Loop through the gameobjects that the raycast hits
        for (int i = 0; i < hitsDown.Length; i++)
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < rayDistace)
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
            }
        }
         //Shoot raycast going UP
        hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up, rayDistace, pelletLayer);

        // Loop through the gameobjects that the raycast hits
        for (int i = 0; i < hitsUp.Length; i++)
        {
            float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < rayDistace)
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
            }
        }
         //Shoot raycast going RIGHT
        hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, rayDistace, pelletLayer);

        // Loop through the gameobjects that the raycast hits
        for (int i = 0; i < hitsRight.Length; i++)
        {
            float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < rayDistace)
            {
                canMoveRight = true;
                nodeRight = hitsRight[i].collider.gameObject;
            }
        }
         //Shoot raycast going LEFT
        hitsLeft = Physics2D.RaycastAll(transform.position, -Vector2.right, rayDistace, pelletLayer);

        // Loop through the gameobjects that the raycast hits
        for (int i = 0; i < hitsLeft.Length; i++)
        {
            float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < rayDistace)
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }

        if (isGhostStartingNode)
        {
            canMoveDown = true;
            nodeDown = gameManager.ghostNodeCenter;
        }
    }

    public GameObject GetNodeFromDirection (string direction)
    {
        if (direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }

        else if (direction == "right" && canMoveRight)
        {
            return nodeRight;
        }

        else  if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }

        else if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }else return null;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Player"  && hasPellet)
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            gameManager.CollectedPellet(this);
        }
    }
}
