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
        if (poi == null) return;
        //get position of poi
        Vector3 destination = poi.transform.position;
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
