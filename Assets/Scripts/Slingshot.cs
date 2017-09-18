using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    public GameObject prefabProjectile;
    public float velocityMult = 4f;
    public bool _________________________;
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(true);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter() {
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit () {
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        //the player has pressed mouse down
        aimingMode = true;
        //instantiates projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at launchPoint
        projectile.transform.position = launchPos;
        //set it to isKinematic
        projectile.GetComponent<Rigidbody>().isKinematic = true;

    }

    void Update() {
        if (!aimingMode) return;
        //get current mouse posisition
        Vector3 mousePos2D = Input.mousePosition;
        //convert mouse pos to 3d cords
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //find delta from launchPos to mouse3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //limit delta to raduys if slingshot collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move the projectile to new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            projectile = null;
        }
    }
}
