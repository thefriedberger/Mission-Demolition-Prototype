using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour {
    static public MissionDemolition S;

    public GameObject[] castles;
    public Text gtLevel;
    public Text gtScore;
    public Vector3 castlePos;

    public bool ________________;

    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";
    public float timeRemaining = 0.0f;

	// Use this for initialization
	void Start () {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
	}

    void StartLevel() {
        //get rid of the old castle if it exists
        if (castle != null) {
            Destroy(castle);
        }

        //destroy old porjectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos) {
            Destroy(pTemp);
        }

        //instantiate new castle
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reset camera
        SwitchView("Both");
        ProjectileLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }

    void ShowGT () {
        //showing the data in GUITexts
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }
	
	// Update is called once per frame
	void Update () {
        ShowGT();

        //check for level end
        if (mode == GameMode.playing && Goal.goalMet) {
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Both");
            //start next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
        if (FollowCam.S.poi != null) {
            GameObject poi = FollowCam.S.poi;
            if (poi.GetComponent<Rigidbody>().IsSleeping()) {
                SwitchView("Slingshot");
            }
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) {
                SwitchView("Slingshot");
                ProjectileLine.S.poi = null;
            }
        }
	}

    void NextLevel () {
        level++;
        if (level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    void OnGUI () {
        //draw the ui button for view switching at the top of the screen
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

        switch(showing) {
            case "Slingshot":
                if(GUI.Button(buttonRect, "Show Castle")) {
                    SwitchView("Castle");
                }
                break;
            case "Castle":
                if(GUI.Button(buttonRect, "Show Both")) {
                    SwitchView("Both");
                }
                break;
            case "Both":
                if(GUI.Button(buttonRect, "Show Slingshot")) {
                    SwitchView("Slingshot");
                }
                break;

        }
    }

    //static method that allows code anywhere to request a view change
    static public void SwitchView (string eView) {
        S.showing = eView;
        switch (S.showing) {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    public static void ShotFired() {
        S.shotsTaken++;
        S.timeRemaining = 7.0f;
    }
}
