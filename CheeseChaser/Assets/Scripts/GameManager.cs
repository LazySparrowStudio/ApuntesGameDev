using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject pacman;
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public int score;
    public Text scoreText;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject pinkGhost;
    public GameObject blueGhost;
    public GameObject orangeGhost;
    public EnemyController redGhostController;

    public EnemyController blueGhostController;

    public EnemyController pinkGhostController;

    public EnemyController orangeGhostController;

    public int totalPellets;
    public int pelletsLeft;
    public int pelletsCollectedOnThisLife;
    public bool hadDeathOnThisLevel = false;
    public bool isGameRunning;

    public List<NodeController> nodeControllerList = new List<NodeController>();
    public bool newGame;
    public bool clearedLevel = false;

    public AudioSource startGameAudio;

    public int lives = 3;
    public int currentLevel = 1;

    public enum GhostMode
    {
        chase,
        scatter
    }

    public GhostMode currentGhostMode;
    void Awake()
    {
        newGame = true;
        clearedLevel = false;

        redGhostController = redGhost.GetComponent<EnemyController>();
        blueGhostController = blueGhost.GetComponent<EnemyController>();
        pinkGhostController = pinkGhost.GetComponent<EnemyController>();
        orangeGhostController = orangeGhost.GetComponent<EnemyController>();

        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        pacman = GameObject.Find("Player");

       

    }
    void Start()
    {
         StartCoroutine(Setup());
    }

    public IEnumerator Setup()
    {
        //If player clears a level, a background will appear covering the level, and the game will pause for 0.1 seconds
        if (clearedLevel)
        {
            //Activate the background
            yield return new WaitForSeconds(0.1f);
        }

        pelletsCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        isGameRunning = false;
        currentMunch = 0;

        float gameTimer = 1f;
        if (clearedLevel || newGame)
        {
            gameTimer = 4f;
            //Pellets will respawn when player clears the level or starts a new game
            for (int i = 0; i < nodeControllerList.Count; i++)
            {
                nodeControllerList[i].RespawnPellet();
            }
        }

        if (newGame)
        {
            //play song
            startGameAudio.Play();
            score = 0;
            scoreText.text = "Score: " + score.ToString();
            lives = 3;
            currentLevel = 1;

        }
        //Player respawn
        pacman.GetComponent<PlayerController>().Setup();

        //Ghosts respawn
        redGhostController.Setup();
        pinkGhostController.Setup();
        blueGhostController.Setup();
        orangeGhostController.Setup();

        newGame = false;
        clearedLevel = false;

        yield return new WaitForSeconds(gameTimer);

        StartGame();
    }

    void StartGame()
    {
        isGameRunning = true;
        siren.Play();
    }


    public void GotPelletFromNodeController(NodeController nodeController)
    {
        nodeControllerList.Add(nodeController);
        totalPellets++;
        pelletsLeft++;
    }
    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }
    public void CollectedPellet(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }

        pelletsLeft--;
        pelletsCollectedOnThisLife++;

        int requiredBluePellets = 0;
        int requiredOrangePellets = 0;

        if (hadDeathOnThisLevel)
        {
            requiredBluePellets = 12;
            requiredOrangePellets = 32;
        }
        else
        {
            requiredBluePellets = 30;
            requiredOrangePellets = 60;
        }
        if (pelletsCollectedOnThisLife >= requiredBluePellets && !blueGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            blueGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }
        if (pelletsCollectedOnThisLife >= requiredOrangePellets && !orangeGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            orangeGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }
        AddToScore(10);
        //Add to our score

        //Check if there are any pellets left

        //Check how many pellets are left

        //Powerpellets
    }
}
