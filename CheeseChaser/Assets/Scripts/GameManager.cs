using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public int score;
    public Text scoreText;
    void Awake(){
        
        score = 0;
        currentMunch = 0;
        siren.Play();
        
    }  

    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }
    public void CollectedPellet(NodeController nodeController)
    {
        if(currentMunch == 0)
        {
            munch1.Play();
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }

        AddToScore(10);
        //Add to our score

        //Check if there are any pellets left

        //Check how many pellets are left

        //Powerpellets
    }
}
