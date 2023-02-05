using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnEnemies : MonoBehaviour
{
    //create an array of enemy gameobjects
    public GameObject[] differentAsteroids;

    //holds the x and z coordinated of the screen
    [SerializeField] private float xBounds = 21;
    [SerializeField] private float zBounds = 14;
    [SerializeField] private float xBoundsPowerUps = 18;
    [SerializeField] private float zBoundsPowerUps = 10;

    private int spawnWaves;                 //this varibale holds the next amount to spawn
    private int enemySpawn;                 //this variable holds the amount of enemies on the screen
    private int differentEnemiesRandom;     //this holds the randomness of the type of asteroid to spawn
    private int randomLocation;             //this holds the randomness of where each enemy will spawn
    private int bossSpawn;                  //this keeps track so that every 5 levels the boss will spawn
    private bool SpawnBoss;                 //spawn the boss boolean
    private bool isGamePause;               //is the game paused boolean

    //These variables holds the min and max of the spawn types and spawn range across the screen
    private int minimumRandomSpawn;
    private int maximumRandomSpawn;
    private int minimumRandomLocationSpawn;
    private int maximumRandomLocationSpawn;
    private int randomPowerUps;
    private Vector3 randomSpawnPositionPowerUps;
    private bool creditsScrollUp;


    public bool gameIsActive;                   //this varibale holds the state of the game
    public TextMeshProUGUI mainTitleText;       //this is the main title text
    public Button startButton;                  //this is the start button
    public TextMeshProUGUI levelNumber;         //this displays the level number to the player
    public TextMeshProUGUI helpText;            //this is the help text
    public TextMeshProUGUI creditsText;         //this is the credits text
    public GameObject[] powerUpPrefabs;         //this is an array for the powerup prefabs
    public Button helpButton;                   //this is the help button
    public Button exitButton;                   //this is the exit button
    public Button backFromHelp;                 //this is the back from help and credits button
    public Button creditsButton;                //this is the credits button
    public TextMeshProUGUI scoreText;           //this is the score text
    public TextMeshProUGUI liveText;            //this is the life text
    public TextMeshProUGUI gamePausedText;      //this is the pause game text
    public TextMeshProUGUI bossSpawnedText;     //this is the spawned boss text
    public Material backgroundMaterialMenu;     //this is the backgrounds change between main menu and levels
    public Material backgroundMaterialGameplay; 
    public Renderer backGroundObject;
    public GameObject player;                   //this is the player gameobject
    public GameObject boss;                     //this is the boss gameobject


    // Start is called before the first frame update
    void Start()
    {
        //the amount to spawn starts at 1 and increase each time there are no more enemies
        spawnWaves = 1;
        //this makes sure the random spawn is correct with the array list of objects  =3
        minimumRandomSpawn = 0;
        maximumRandomSpawn = 3;
        bossSpawn = 0;
        SpawnBoss = false;
        minimumRandomLocationSpawn = 0;
        maximumRandomLocationSpawn = 3;
        //the game state is set to false on the start of the game.  When this is set to true, the spawning starts
        gameIsActive = false;
        isGamePause = false;
    }

    // Update is called once per frame
    void Update()
    {
        //on each update, get the amount of enemies on the screen
        enemySpawn = FindObjectsOfType<Enemy>().Length;

        //spawn another set of enemies, if the amount is <0 and the game is active.
        if (enemySpawn <= 0 && gameIsActive)
        {
            StartCoroutine(levelIndicatorCountDown());
            for (int i = 0; i < spawnWaves; i++)
            {
                levelNumber.gameObject.SetActive(true);
                levelNumber.text = "Level " + spawnWaves;
                spawnEnemies(SpawnBoss);         //spawn enemies
            }

            //after spawning the correct amount of enemies, increase the amount of the next spawn by 1
            spawnPowerUps();        //spawn a powerup
            spawnWaves++;
            bossSpawn++;
            if(bossSpawn%5 == 0)    //spawns a boss every 5 levels
            {
                bossSpawnedText.gameObject.SetActive(true);
                SpawnBoss = true;
                spawnEnemies(SpawnBoss);
                SpawnBoss = false;
            }
        

        }

        //pause game
        if (Input.GetKeyDown(KeyCode.Escape) && isGamePause == false  && gameIsActive)
        {
            pauseGame();
            isGamePause = true;
            gamePausedText.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);         //show the exit button
        }
        //unpause game
        else if ((Input.GetKeyDown(KeyCode.Escape) && isGamePause == true && gameIsActive))
        {
            unPauseGame();
            isGamePause = false;
            gamePausedText.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);         //remove the exit button
        }

        //roll credits
        if (creditsScrollUp)
        {
            creditsText.transform.Translate(Vector3.up / 3.0f);
        }

    }


    public void spawnEnemies(bool spawnBoss)
    {
        //generate a random number from 0 to the length of the array(3 indexes)
        differentEnemiesRandom = Random.Range(minimumRandomSpawn, maximumRandomSpawn);
        //create a random location
        randomLocation = Random.Range(minimumRandomLocationSpawn, maximumRandomLocationSpawn);
        spawnLocation(randomLocation, differentEnemiesRandom, spawnBoss);

    }

    public void spawnLocation(int randomLocation, int differentEnemiesRandom, bool spawnBoss)
    {

        //this will randomly spawn an asteroid in any corner of the screen
        if (randomLocation == 0)
        {
            Vector3 randomPositionLeft = new Vector3(xBounds, 0, Random.Range(zBounds, -zBounds));
            Instantiate(differentAsteroids[differentEnemiesRandom], randomPositionLeft, transform.rotation);
            if (spawnBoss)
            {
                Instantiate(boss, randomPositionLeft, transform.rotation);
            }
        }
        else if (randomLocation == 1)
        {
            Vector3 randomPositionRight = new Vector3(-xBounds, 0, Random.Range(zBounds, -zBounds));
            Instantiate(differentAsteroids[differentEnemiesRandom], randomPositionRight, transform.rotation);
            if (spawnBoss)
            {
                Instantiate(boss, randomPositionRight, transform.rotation);
            }
        }
        else if (randomLocation == 2)
        {
            Vector3 randomPositionUp = new Vector3(Random.Range(xBounds, -xBounds), 0, zBounds);
            Instantiate(differentAsteroids[differentEnemiesRandom], randomPositionUp, transform.rotation);
            if (spawnBoss)
            {
                Instantiate(boss, randomPositionUp, transform.rotation);
            }
        }
        else if (randomLocation == 3)
        {
            Vector3 randomPositionDown = new Vector3(Random.Range(xBounds, -xBounds), 0, -zBounds);
            Instantiate(differentAsteroids[differentEnemiesRandom], randomPositionDown, transform.rotation);
            if (spawnBoss)
            {
                Instantiate(boss, randomPositionDown, transform.rotation);
            }
        }
    }

    public void startGame()
    {
        makePlayerVisible();
        //this is used to put it in the onClick() of the start button
        mainTitleText.gameObject.SetActive(false);      //remove the titlescreen
        startButton.gameObject.SetActive(false);        //remove the start button
        helpButton.gameObject.SetActive(false);         //remove the help button
        exitButton.gameObject.SetActive(false);         //remove the exit button
        helpText.gameObject.SetActive(false);            //remove help
        backFromHelp.gameObject.SetActive(false);        //show back button
        creditsButton.gameObject.SetActive(false);      //remove credits button
        scoreText.gameObject.SetActive(true);        //show back button
        liveText.gameObject.SetActive(true);        //show back button
        player.gameObject.SetActive(true);          //show the player
        creditsText.gameObject.SetActive(false);    //remove credits text
        backGroundObject.material = backgroundMaterialGameplay;
        
        gameIsActive = true;                            //set the game as active
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     //restart the scene from scratch
    }

    //exit the application
    public void exitApplication()
    {
        //exit application
        Debug.Log("EXIT");
        Application.Quit();
    }
    IEnumerator levelIndicatorCountDown()
    {

    //indicate the level
        yield return new WaitForSeconds(2);
        levelNumber.gameObject.SetActive(false);
        bossSpawnedText.gameObject.SetActive(false);
    }

    private void spawnPowerUps()
    {
        //spawn a power up
        randomPowerUps = Random.Range(0, 3);
        randomSpawnPositionPowerUps = new Vector3(Random.Range(-xBoundsPowerUps, xBoundsPowerUps), 0, Random.Range(-zBoundsPowerUps, zBoundsPowerUps));
        Instantiate(powerUpPrefabs[randomPowerUps], randomSpawnPositionPowerUps, powerUpPrefabs[randomPowerUps].transform.rotation);
        
    }

    public void showHelp()
    {
        //show help 
        mainTitleText.gameObject.SetActive(false);      //remove the titlescreen
        startButton.gameObject.SetActive(false);        //remove the start button
        helpButton.gameObject.SetActive(false);         //remove the help button
        exitButton.gameObject.SetActive(false);         //remove the exit button
        creditsButton.gameObject.SetActive(false);      //remove credits button
        helpText.gameObject.SetActive(true);            //show help
        backFromHelp.gameObject.SetActive(true);        //show back button
        creditsText.gameObject.SetActive(false);    //remove credits text
        
    }

    public void showCredits()
    {
        //roll credits
        mainTitleText.gameObject.SetActive(false);      //remove the titlescreen
        startButton.gameObject.SetActive(false);        //remove the start button
        helpButton.gameObject.SetActive(false);         //remove the help button
        exitButton.gameObject.SetActive(false);         //remove the exit button
        creditsButton.gameObject.SetActive(false);      //remove credits button
        helpText.gameObject.SetActive(false);            //show help
        backFromHelp.gameObject.SetActive(true);        //show back button
        creditsText.gameObject.SetActive(true);    //remove credits text
        creditsText.transform.position = new Vector3(800f, -200f, 0);       //restart the position of the text to these coordinates
        creditsScrollUp = true;         //make sure the credits can roll up
    }

    public void mainMenu()
    {
        //how the main menu should look
        mainTitleText.gameObject.SetActive(true);      //remove the titlescreen
        startButton.gameObject.SetActive(true);        //remove the start button
        helpButton.gameObject.SetActive(true);         //remove the help button
        exitButton.gameObject.SetActive(true);         //remove the exit button
        creditsButton.gameObject.SetActive(true);

        creditsText.gameObject.SetActive(false);    //remove credits text
        helpText.gameObject.SetActive(false);            //show help
        backFromHelp.gameObject.SetActive(false);        //show back button
        creditsScrollUp = false;
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void unPauseGame()
    {
        Time.timeScale = 1;
    }

    public void makePlayerVisible()
    {
        //make player visible upon starting the game
        for (int i = 0; i < player.transform.childCount; ++i)
        {
            player.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

}
