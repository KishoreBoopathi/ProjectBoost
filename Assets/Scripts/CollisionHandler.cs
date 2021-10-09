using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other) 
    {
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("No worries. Keep going");
                break;
            case "Fuel":
                Debug.Log("Yaaaay! Can cover some extra distance.");
                break;
            case "LandingPad":
                Debug.Log("Nailed the landing.");
                break;
            default:
                Debug.Log("Dude! You suck.");
                break;
        }    
    }
}
