using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHitAudio : MonoBehaviour
{
    [SerializeField] private float laserHitVolume = 1.0f;       //this is the volume of when the laser hits an asteroid
    private int randomAudio;                                    //this is the hold a random audio clip to play
    public AudioClip [] laserHit;                               //this is an array of all the possible sounds
    private AudioSource laserHitAudio;                          //this is the audiosource of the laser hitting audio


    // Start is called before the first frame update
    void Start()
    {
        laserHitAudio = GetComponent<AudioSource>();            //get the audio source
        randomAudio = Random.Range(0, 3);                       //generate a random number
        playAudio(randomAudio);                                 //call the method playAudio
    }

    private void playAudio(int random)
    {
            laserHitAudio.PlayOneShot(laserHit[random], laserHitVolume);    //play the audio
    }
}
