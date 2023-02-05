using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float forwardSpeed;        //this is the forward speed for the player
    [SerializeField] private float rotationSpeed;       //this is the rotation speed for the player
    [SerializeField] private AudioClip HeroLaserSound;  //this is the sound the laser makes when fired
    [SerializeField] private float laserVolume = 0.75f;         //this is the volume of the laser
    [SerializeField] private AudioClip powerUpSound;            //this is the sound for powerups
    [SerializeField] private float powerUpVolume = 0.75f;       //this is the volume for powerups
    [SerializeField] private float inFrontOfPlayer;     //this is the multiplier for the bullet to spawn in front of the player
    [SerializeField] private AudioClip playerHit;       //this is the sound for when the player is hit by an asteroid
    [SerializeField] private float playerHitVolume = 1; //this is the volume
    [SerializeField] private float invinsibleTime = 2;  //time to stop being invinsible

    private float horizontalAxis;                       //this keeps track of the input manager (horizontal axis)
    private float verticalAxis;                         //this keeps track of the input manager (vertical axis)
    private Rigidbody rb;                               //this is the player's rigid body reference
    private AudioSource playerAudio;                    //this is to refer to the player's audio source
    private SpawnEnemies spawner;                       //get a reference to the spawner object
    private PointsSystem points;                        //this is used here to increase points for powerup
    private GameObject[] enemyPrefabs;                  //used to reduce the life of all asteroids
    private bool invinsible = false;                    //this is set to true when hit, to have the plyaer invisible for a few seconds

    public int livesRemaining;                          //this is the amount of lives left
    public GameObject bulletPrefab;                     //this is the create bullet prefabs on space or mouse click
    public TextMeshProUGUI livesRemainText;             //this is to update the lives remaining text
    public TextMeshProUGUI gameOverText;                //this is to activate or deactivate the game over text
    public TextMeshProUGUI powerUpText;                 //this is the power up text
    public ParticleSystem playerExplode;                //this is to have an animation when player explodes
    public ParticleSystem muzzleFlash;                  //this is the muzzle flash whenever the player fires
    public ParticleSystem exhaustOutletLeft;            //this is the exhaust outlet of the player
    public ParticleSystem exhaustOutletRight;           //this is the exhaust outlet of the player
    public Button restartButton;                        //this is to activate or deactivate the restart button
    public GameObject playerExplodeAudio;               //used to start the sound for the player explostion
    public GameObject shieldPrefab;                     //the shield prefab

    // Start is called before the first frame update
    void Start()
    {
        //initialize the lives variable
        livesRemaining = 3;
        // get a reference for the player's rigidbody
        rb = GetComponent<Rigidbody>();
        // get a reference for the player's audiosource
        playerAudio = GetComponent<AudioSource>();
        //get a reference to the spawner
        spawner = GameObject.Find("SpawnManager").GetComponent<SpawnEnemies>();
       //find the pointsManager in the heirarcy
        points = GameObject.Find("PointsManager").GetComponent<PointsSystem>();
        //start the particile system as false
        exhaustOutletLeft.gameObject.SetActive(false);
        exhaustOutletRight.gameObject.SetActive(false);

}

    // Update is called once per frame
    void Update()
    {
        //get the input from the keyboard and make it turn and move forward
        
        if (spawner.gameIsActive)
        {
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
            rb.AddRelativeForce(Vector3.forward * verticalAxis * forwardSpeed, ForceMode.Impulse);
            rb.AddRelativeTorque(Vector3.up * horizontalAxis * rotationSpeed, ForceMode.Impulse);
            //gameObject.transform.Rotate(Vector3.up * horizontalAxis * rotationSpeed * Time.deltaTime);
        
            //as long as player is applying force forward, the particle system of the exhaust will be displayed
            if(verticalAxis > 0 || verticalAxis < 0)
            {
                exhaustOutletLeft.gameObject.SetActive(true);
                exhaustOutletRight.gameObject.SetActive(true);
            }
            else
            {
                exhaustOutletLeft.gameObject.SetActive(false);
                exhaustOutletRight.gameObject.SetActive(false);
            }
        
        }

        //spawn bullets from the same position as this gameObject (player) if space OR mouse click is detected
        if (Input.GetKeyDown(KeyCode.Space) && spawner.gameIsActive || Input.GetMouseButtonDown(0) && spawner.gameIsActive)
        {
            Vector3 playerPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            Instantiate(muzzleFlash, transform.position + (transform.forward*inFrontOfPlayer/2), transform.rotation);               //PROBLEM HERE!!  HOW TO MAKE MUZZLE FLASH APPEAR ALWAYS IN FRONT
            Instantiate(bulletPrefab, transform.position + (transform.forward* inFrontOfPlayer), transform.rotation);
            playerAudio.PlayOneShot(HeroLaserSound, laserVolume);
        }

        //if the lifes are 0, call the gameOver method
        if(livesRemaining <= 0)
        {
            shieldPrefab.gameObject.SetActive(false);
            gameOver();
        }

        //keep and update on the amount of lives left and prin them on screen
        livesRemainText.text = "Lives: " + livesRemaining;

        shieldPrefab.transform.position = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //you loose 1 life everytime the player get in contact with the "enemy" tag (asteroids)
        if(collision.gameObject.tag == "Enemy" && invinsible == false)
        {
            shieldPrefab.gameObject.SetActive(true);
            invinsible = true;
            StartCoroutine(shieldRoutine());
            playerAudio.PlayOneShot(playerHit, playerHitVolume);
            livesRemaining -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player collides with a life power up, increase 1 up
        if(other.gameObject.tag == "LifePowerUp")
        {
            playerAudio.PlayOneShot(powerUpSound, powerUpVolume);
            livesRemaining++;
            powerUp("LIFE UP");
        }

        if(other.gameObject.tag == "PointsPowerUp")
        {
            //increase a 1000 points
            playerAudio.PlayOneShot(powerUpSound, powerUpVolume);
            points.pointSystem += 1000;
            powerUp("PLUS 1000 POINTS");
        }

        if(other.gameObject.tag == "ExplodeAllPowerUp")
        {
            playerAudio.PlayOneShot(powerUpSound, powerUpVolume);
            enemyPrefabs = GameObject.FindGameObjectsWithTag("Enemy");
            powerUp("SKIP LEVEL");
            //cycle through all the enemies and destory them
            for (int i =0; i<enemyPrefabs.Length; i++)
            {
                Destroy(enemyPrefabs[i]);
            }
        }


    }


    private void gameOver()
    {
        
        gameOverText.gameObject.SetActive(true);        //show the game over and restart button on death
        restartButton.gameObject.SetActive(true);     
        spawner.gameIsActive = false;                   //make the game inactive

        Instantiate(playerExplode, transform.position, playerExplode.transform.rotation);   //start the death animation
        Instantiate(playerExplodeAudio, transform.position, transform.rotation);            //start the death audio
        Destroy(gameObject);        //destory player
    }

    private void powerUp(string powerUp)
    {
        //this is the method to display the text for different powerups
        powerUpText.text = powerUp;
        powerUpText.gameObject.SetActive(true);
        StartCoroutine(powerUpRoutine());
    }

    IEnumerator powerUpRoutine()
    {
        //leave the text on screen for 3 seconds before setting it to false
        yield return new WaitForSeconds(3);
        powerUpText.gameObject.SetActive(false);

    }

    IEnumerator shieldRoutine()
    {
        //set the shield to false after invinsibleTime expires
        yield return new WaitForSeconds(invinsibleTime);
        invinsible = false;
        shieldPrefab.gameObject.SetActive(false);
    }

}
