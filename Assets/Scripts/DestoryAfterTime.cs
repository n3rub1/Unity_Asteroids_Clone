using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    [SerializeField] int destoryAfterTime = 2;


    // Start is called before the first frame update
    void Start()
    {
        //make sure gameobject is destroyed after 2 seconds.  This is applied on animations to make sure that
        //they do not stay in the Hierarchy
        Destroy(gameObject, destoryAfterTime); 
    }

}
