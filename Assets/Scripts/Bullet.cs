using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;     //this is the bullet speed
    public float timeToDestroy = 3f;    //this is the time for the gameobject to be destroyed if it does not hit anything
    public ParticleSystem asteroidHit;  //this is the particle system 
    public GameObject hitAudio;         //this is the hit audio


    // Start is called before the first frame update
    void Start()
    {
        //This method is to start the decay time of the bullet.  After timeToDestroy varibale, DestroyObject method is called
        Invoke("DestoryObject", timeToDestroy);

    }

    // Update is called once per frame
    void Update()
    {
        //make the bullet move forward without the physics engine
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        
    }

    void DestoryObject()
    {
        //once the invoke timer is done, the gameObject (bullet) is destoryed
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //upon colliding with anything destroy the bullet and start the hit animation
        //the hit animation is destoryed automatically by the DestoryAfterTime script

        Instantiate(asteroidHit, transform.position, asteroidHit.transform.rotation);
        Instantiate(hitAudio, transform.position, transform.rotation);
        Destroy(gameObject);

    }

}
