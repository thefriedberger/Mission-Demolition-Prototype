using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    static public bool goalMet = false;

    void OnTriggerEnter (Collider other) {
        //when the trigger is hit by something
        //check to see if it's a projectile
        if (other.gameObject.tag == "Projectile") {
            Goal.goalMet = true;
            Color c = GetComponent<Renderer>().material.color;
            c.a = 1;
            GetComponent<Renderer>().material.color = c;
        }
    }
}
