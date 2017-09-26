using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {
    static public ProjectileLine S;

    public float minDist = 0.1f;
    public bool ___________________;

    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

	// Use this for initialization
    void Awake () {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
	}

    public GameObject poi {
        get {
            return (_poi);
        }
        set {
            _poi = value;
            if (_poi != null) {
                //when _poi is set to somethin new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                Addpoint();
            }
        }
    }

    public void Clear() {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
	
    public void Addpoint() {
        //this is called to add a point to the line
        Vector3 pt = _poi.transform.position;

        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
            return;
        }

        if (points.Count == 0) {
            //if this is the launch point
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launcPosDiff = pt - launchPos;
            //it add an extra bit of line to aid aiming later
            points.Add(pt + launcPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //enables LineRenderer
            line.enabled = true;
        } else {
            //normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint {
        get {
            if (points == null) {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
		if (poi == null) {
            //if there is no poi, search for one
            if(FollowCam.S.poi != null) {
                if (FollowCam.S.poi.tag == "Projectile") {
                    poi = FollowCam.S.poi;
                } else {
                    return;
                }
            } else {
                return;
            }
        }
        Addpoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping()) {
            //once the poi is sleeping, it's cleared
            poi = null;
        }
	}
}
