using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManagerRedScreen : MonoBehaviour
{
    public enum GhostMode
    {
        chase,
        scatter
    }
    public GhostMode currentGhostMode;
    public List<NodeControllerRedScreen> nodeControllerList = new List<NodeControllerRedScreen>();
    public string screenPositions = "BlueScreen";
    public GameObject pacman;
    public GameObject redGhostPrefab;
    public EnemyControllerRedScreen GhostPrefabController;
    public Text scoreText;
    public Text gameOverText;
    public Image blackBackground;
    public string screenType;
    public int currentMunch = 0;
    public int score;
    public int totalPellets;
    public int pelletsLeft;
    public int pelletsCollectedOnThisLife;
    public int powerPelletMultiplier = 1;
    public int lives = 3;
    public int currentLevel = 1;
    public float currentPowerPelletTimer = 0;
    public float ghostModeTimer;
    public float powerPelletTimer = 8f;
    public bool isPowerPelletRunning = false;
    public bool hadDeathOnThisLevel = false;
    public bool isGameRunning;
    public bool newGame;
    public bool clearedLevel = false;
    public bool runningTimer;
    public bool completedTimer;
    public int[] ghostModeTimers = new int[] { 7, 20, 7, 20, 5, 20, 5 };
    public int ghostModeTimersIndex;
    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public AudioSource powerPelletSound;
    public AudioSource respawningAudio;
    public AudioSource ghostEatenAudio;
    public AudioSource startGameAudio;
    public AudioSource deathSound;

    void Awake()
    {
        blackBackground.enabled = false;
        newGame = true;
        clearedLevel = false;
        isPowerPelletRunning = false;
        pacman = GameObject.Find("Player");
     
    }

    void Start()
    {
        StartCoroutine(Setup());
    }

    public IEnumerator Setup()
    {

        ghostModeTimer = 0;
        ghostModeTimersIndex = 0;
        completedTimer = false;
        runningTimer = true;
        gameOverText.enabled = false;

        //If player clears a level, a background will appear covering the level, and the game will pause for 0.1 seconds
        if (clearedLevel)
        {
            blackBackground.enabled = true;
            //Activate the background
            yield return new WaitForSeconds(0.1f);
        }
        blackBackground.enabled = false;

        pelletsCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        isGameRunning = false;
        currentMunch = 0;

        float gameTimer = 1f;

        if (clearedLevel || newGame)
        {
            pelletsLeft = totalPellets;
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
        pacman.GetComponent<PlayerControllerRedScreen>().Setup();

        //Ghosts respawn
        GhostPrefabController.Setup();

        newGame = false;
        clearedLevel = false;

        yield return new WaitForSeconds(gameTimer);

        StartGame();
    }

    void Update()
    {
        if (!isGameRunning)
        {
            return;
        }

        if (GhostPrefabController.ghostNodeState == EnemyControllerRedScreen.GhostNodeStateEnum.respawning)
        {
            if (!respawningAudio.isPlaying)
            {
                respawningAudio.Play();
            }
            else
            {
                if (respawningAudio.isPlaying)
                {
                    respawningAudio.Stop();
                }
            }
        }

      
        if (isPowerPelletRunning)
        {
            currentPowerPelletTimer += Time.deltaTime;
            if (currentPowerPelletTimer >= powerPelletTimer)
            {
                isPowerPelletRunning = false;
                currentPowerPelletTimer = 0;
                powerPelletSound.Stop();
                siren.Play();
                powerPelletMultiplier = 1;
            }
        }
    }
    // Cesar me comenta (jaja) que use estos metodos para acceder a la variable
    /* public void SetGameMode(GhostMode mode)
     {
         currentGhostMode = mode;
     }

     public GhostMode GetGameMode()
     {
         return currentGhostMode;
     }*/

    void StartGame()
    {
        isGameRunning = true;
        siren.Play();
    }

    void StopGame()
    {
        isGameRunning = false;
        siren.Stop();
        pacman.GetComponent<PlayerControllerRedScreen>().Stop();
        respawningAudio.Stop();
        powerPelletSound.Stop();
    }


    public void GotPelletFromNodeController(NodeControllerRedScreen nodeController)
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
    public IEnumerator CollectedPellet(NodeControllerRedScreen nodeController)
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

        AddToScore(10);


        //Check if there are any pellets left
        if (pelletsLeft == 0)
        {
            currentLevel++;
            clearedLevel = true;
            StopGame();
            yield return new WaitForSeconds(1);
            StartCoroutine(Setup());
        }
        //Check how many pellets are left

        //Powerpellets
        if (nodeController.isPowerPellet)
        {
            siren.Stop();
            powerPelletSound.Play();
            isPowerPelletRunning = true;
            currentPowerPelletTimer = 0;


            GhostPrefabController.SetFrightened(true);
        }
    }

    public IEnumerator PauseGame(float timeToPause)
    {
        isGameRunning = false;
        yield return new WaitForSeconds(timeToPause);
        isGameRunning = true;
    }


    public void GhostEaten(EnemyControllerRedScreen enemyController)
    {
        ghostEatenAudio.Play();
        AddToScore(400 * powerPelletMultiplier);
        powerPelletMultiplier++;
        enemyController.ghostSprite.enabled = false;
        StartCoroutine(PauseGame(1f));
    }

    public IEnumerator PlayerEaten()
    {
        hadDeathOnThisLevel = true;
        StopGame();
        yield return new WaitForSeconds(1f);

        GhostPrefabController.SetVisible(false);

        pacman.GetComponent<PlayerController>().Death();
        deathSound.Play();
        yield return new WaitForSeconds(3);

        lives--;
        if (lives <= 0)
        {
            newGame = true;
            //Display GameOverlay
            gameOverText.enabled = true;
            yield return new WaitForSeconds(3);
        }

        StartCoroutine(Setup());
    }


}
