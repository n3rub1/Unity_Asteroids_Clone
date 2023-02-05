using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplodeAudio : MonoBehaviour
{
    [SerializeField] private float playerExplodeVolume;     //playerexplode volume
    public AudioClip playerExplode;                         //the audio clip for player explode
    private AudioSource playerExplodeAudio;                 //the audio source for the player


    // Start is called before the first frame update
    void Start()
    {
        playerExplodeAudio = GetComponent<AudioSource>();       //get the audiosource
        playerExplodeVolume = 1.0f;                            //set the volume to 1
        playerExplodeAudio.PlayOneShot(playerExplode, playerExplodeVolume); //play the clip
    }

}
