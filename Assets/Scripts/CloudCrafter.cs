using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour {
    public int numClouds = 40;
    public GameObject[] cloudPrefabs;
    public Vector3 cloudPosMin;
    public Vector3 cloudPosMax;
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 5;
    public float cloudSpeedMult = 0.5f;

    public bool _______________________;

    public GameObject[] cloudInstances;
    
    // Use this for initialization
    void Awake() {
        //make an array large enough to hold all cloud instances
        cloudInstances = new GameObject[numClouds];
        //find cloudanchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        //iterate through make cloud_s
        for (int i = 0; i < numClouds; i++) {
            //pick random int between 0 and cloudPrefabs.length - 1
            //random.range will not ever pick as high as the top number
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            //make an instance
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller clouds (with smaller scaleU) should be closer to ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //smaller clouds should be furth away
            cPos.z = 100 - 90 * scaleU;
            //apply these transofmrs to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //make cloud child of anchor
            cloud.transform.parent = anchor.transform;
            //add cloud to cloudInstances
            cloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update() {
        foreach(GameObject cloud in cloudInstances) {
            //get the cloud scale and pos
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //if cloud moved too far to left 
            if (cPos.x <= cloudPosMin.x) {
                cPos.x = cloudPosMax.x;
            }
            cloud.transform.position = cPos;
        }
    }
}
