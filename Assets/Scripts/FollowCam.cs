using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public FollowCam S; //a FollowCam Singleton

    //fields set in Unity inspector pane
    public float easing = 0.05f;
    public Vector2 minXY;
    public bool ____________________;
    //fields set dynamically
    public GameObject poi; //point of interest
    public float camZ; //desired z pos

    void Awake () {
        S = this;
        camZ = this.transform.position.z;
    }

    void FixedUpdate () {
        Vector3 destination;
        //if there is no POI, return to 0,0,0
        if (poi == null) {
            destination = Vector3.zero;
        } else {
            //get the position of poi
            destination = poi.transform.position;
            //if poi is a projectile, check to see if it's at rest
            if (poi.GetComponent<Rigidbody>().IsSleeping() ) {
                //return default view
                poi = null;
                return;
            }
        }
        //limit the X & Y to min value
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpolate from the current camera position toward dest
        destination = Vector3.Lerp(transform.position, destination, easing);
        //retain a destination.z of camZ
        destination.z = camZ;
        //set camera to destination
        transform.position = destination;
        //set orthographicSize of camera to keep ground in view
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
    }
}
