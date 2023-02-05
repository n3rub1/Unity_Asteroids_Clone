using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    //hold the value of the screen
    [SerializeField] private float xRange;
    [SerializeField] private float zRange;

    private Vector3 currentPosition;        //hold the game object's current position

    // Update is called once per frame
    void Update()
    {
        {
            //Get the current game object's position and store it in the currentPosition varibale
            currentPosition = transform.position;
            if (transform.position.x > xRange)
            {
                //re-assign the xlocation
                currentPosition.x = -xRange;
            }
            else if (transform.position.x < -xRange)
            {
                //re-assign the xlocation
                currentPosition.x = xRange;
            }
            else if (transform.position.z > zRange)
            {
                //re-assign the zlocation
                currentPosition.z = -zRange;
            }
            else if (transform.position.z < -zRange)
            {
                //re-assign the zlocation
                currentPosition.z = zRange;
            }

            //update the position of the game object to wrap around the screen
            transform.position = currentPosition;
        }
    }
}
