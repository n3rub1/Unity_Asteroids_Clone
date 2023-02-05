using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private int pointsAwarded;         //this is the points awarded on destory
    [SerializeField] private float minForce;            //this is the minimum force an object can have on creation
    [SerializeField] private float maxForce;            //this is the maximum force an object can have on creation       
    [SerializeField] private int numberOfSpawn;         //this is the amount of spawn an enemy can have

    public GameObject[] asteroidPrefab;                 //fill the asteroid prefab after they explode (2 for big, 3 for medium, 0 for small)
    public GameObject pointsPrefab;                     //points prefab
    public TextMesh pointsText;                         //points text
    public ParticleSystem enemyExplode;                 //this is the animation of when an enemy explode       
    public int healthBar;                               //this is the health bar of the enemy object (5 for big, 2 for mediuem, 1 for small)


    private Vector3 enemyLocation;                      //keeps a vector3 of the enemy location at all times in the update() method
    private Rigidbody enemyBody;                        //keeps the rigidbody of the enemy
    private PointsSystem points;                        //get the PointsSystem object from the hierarcy
    private float randomSpawn;                          //holds a random spawn so objects dont spawn on top of each other (Bug fix)
    private Vector3 randomForce;                        //holds  random force


    // Start is called before the first frame update
    void Start()
    {

        points = GameObject.Find("PointsManager").GetComponent<PointsSystem>();     //find the PointManager object's script and put it in variable points
        enemyBody = gameObject.GetComponent<Rigidbody>();                           //get the rigidbody of the this gameObject and put it in varibale enemyBody

        //On creation create a random vector3 in the x and z (leaving y as 0).  Afterwards, with this force
        //put it on the enemy's rigidbody, and keep a track of the original health
        randomForce = new Vector3(Random.Range(minForce, maxForce), 0, Random.Range(minForce, maxForce));
        enemyBody.AddForce(randomForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        //keep track of the enemy location
        enemyLocation = transform.position;

        //when healthbar of this object reaches 0 or less (bug fix) - destory gameObject
        if (healthBar <= 0)
        {
            //when health bar reaches 0, the method to spawn more enemies is called. (2 for large, 3 for medium, 0 for small)
            spawnNewEnemies(numberOfSpawn);

            //start the explotion animation on the enemies locationand destory game object
            Instantiate(enemyExplode, enemyLocation, enemyExplode.gameObject.transform.rotation);
            //mark the amount of points provided to the player
            pointsText.transform.GetComponent<TextMesh>();
            pointsText.text = ("+" + pointsAwarded.ToString());
            Instantiate(pointsPrefab, transform.position, pointsPrefab.transform.rotation);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //reduce one health bar if the asteroid is hit only by a bullet tag
        if (collision.gameObject.CompareTag("Bullet"))
        {
            healthBar--;
        }

    }

    private void OnDestroy()
    {
        //add up the score for destroying this object.  This is using the script PointsSystem
       
        points.pointSystem = pointsAwarded + points.pointSystem;
        points.pointsForLife = pointsAwarded + points.pointsForLife;
        
    }

    private void spawnNewEnemies(int numberOfSpawn)
    {

        randomSpawn = 1;
        //spawn the correct number of enemies at a slightly different location.  This is done not to have asteroids spawn on the same spot
        for (int i = 0; i < numberOfSpawn; i++)
        {
            randomSpawn = randomSpawn + 0.35f;
            Instantiate(asteroidPrefab[i], enemyLocation * randomSpawn, transform.rotation);
        }
    }

}
