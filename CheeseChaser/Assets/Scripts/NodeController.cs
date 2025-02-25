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
    public bool isSideNode = false;
     public bool isPowerPellet = false;
     public float powerPelletBlinkTimer = 0.0f;

    void Awake()
    {


        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (transform.childCount > 0)
        {
            gameManager.GotPelletFromNodeController(this);
            hasPellet = true;
            isPelletNode = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }
        //   OnDrawGizmos();

    }

    void Start()
    {
        PelletRayCast();
        //   OnDrawGizmos();

    }

    //Visualizar RayCast
    // void OnDrawGizmos()
    // {
    //     float rayDistace = 0.26f;

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistace);
    //     Gizmos.DrawLine(transform.position, transform.position + Vector3.up * rayDistace);
    //     Gizmos.DrawLine(transform.position, transform.position + Vector3.right * rayDistace);
    //     Gizmos.DrawLine(transform.position, transform.position + Vector3.left * rayDistace);
    // }

    void Update()
    {
        if(!gameManager.isGameRunning)
        {
            return;
        }

        if (isPowerPellet && hasPellet)
        {
            powerPelletBlinkTimer += Time.deltaTime;
            if(powerPelletBlinkTimer>=0.1f)
            {
                powerPelletBlinkTimer = 0.0f;
                pelletSprite.enabled = !pelletSprite.enabled;
            }
        }
    }
    private void PelletRayCast()
    {
        int pelletLayer = LayerMask.GetMask("Node");
        float rayDistace = 0.3f;

        // Initialize variables to store the closest nodes
        float closestDistanceDown = float.MaxValue;
        float closestDistanceUp = float.MaxValue;
        float closestDistanceRight = float.MaxValue;
        float closestDistanceLeft = float.MaxValue;

        // Shoot raycast going DOWN
        RaycastHit2D[] hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down, rayDistace, pelletLayer);
        foreach (var hit in hitsDown)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance < closestDistanceDown)
            {
                closestDistanceDown = distance;
                canMoveDown = true;
                nodeDown = hit.collider.gameObject;
            }
        }

        // Shoot raycast going UP
        RaycastHit2D[] hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up, rayDistace, pelletLayer);
        foreach (var hit in hitsUp)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance < closestDistanceUp)
            {
                closestDistanceUp = distance;
                canMoveUp = true;
                nodeUp = hit.collider.gameObject;
            }
        }

        // Shoot raycast going RIGHT
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, rayDistace, pelletLayer);
        foreach (var hit in hitsRight)
        {
            float distance = Mathf.Abs(hit.point.x - transform.position.x);
            if (distance < closestDistanceRight)
            {
                closestDistanceRight = distance;
                canMoveRight = true;
                nodeRight = hit.collider.gameObject;
            }
        }

        // Shoot raycast going LEFT
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, rayDistace, pelletLayer);
        foreach (var hit in hitsLeft)
        {
            float distance = Mathf.Abs(hit.point.x - transform.position.x);
            if (distance < closestDistanceLeft)
            {
                closestDistanceLeft = distance;
                canMoveLeft = true;
                nodeLeft = hit.collider.gameObject;
            }
        }

        if (isGhostStartingNode)
        {
            canMoveDown = true;
            nodeDown = gameManager.ghostNodeCenter;
        }
    }

    public void RespawnPellet()
    {
        hasPellet = true;
        pelletSprite.enabled = true;
        StartCoroutine(gameManager.CollectedPellet(this));
    }
    public GameObject GetNodeFromDirection(string direction)
    {
        if (direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }

        else if (direction == "right" && canMoveRight)
        {
            return nodeRight;
        }

        else if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }

        else if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }
        else return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && hasPellet)
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            gameManager.CollectedPellet(this);
            StartCoroutine(gameManager.CollectedPellet(this));
        }
    }
}
