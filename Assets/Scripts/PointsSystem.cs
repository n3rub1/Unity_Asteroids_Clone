using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsSystem : MonoBehaviour
{
    [SerializeField] private int oneUp = 10000; //this is the amount of points required to get a one up

    public int pointSystem;                 //this variable is being used to update the score.  The amount is updated from the enemies script (on destory())            
    public int pointsForLife;               //this varibale is also update from the enemies script.  However when it reaches 10000, it updates the health of player                   
    public TextMeshProUGUI scoreText;       //this is to update the text for the score
    
    private PlayerController playerScript;  //this holds the player's script

    void Start()
    {

        //find the player's script in the game
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        //make points equal to 0 on the start of the game
        pointSystem = 0;
        pointsForLife = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //check if player has reached the value of 10000 or greater(oneUp)
        if(pointsForLife >= oneUp)
        {
            //player receieves one life and score for next life is reset to 0
            playerScript.livesRemaining++;
            pointsForLife = 0;
        }

        //update the score on every frame (the pointSystem is updated from the enemy's script
        scoreText.text = "Score: " + pointSystem;
     }
}
