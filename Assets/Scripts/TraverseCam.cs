using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseCam : MonoBehaviour {

    public Camera cam;
    public GameObject camObj;

    public int camSpeed = 5;
    public float leftRightAngle = 0.35f;

    // Use this for initialization
    void Start () {
        if (cam == null)
            cam = Camera.main;
        if (camObj == null)
            camObj = GameObject.Find("CameraObject");
    }
	
	// Update is called once per frame
	void Update () {
        // Move the camera left or right
        if (cam.transform.rotation.y < -leftRightAngle && camObj.transform.position.x > -6.5f) {
            camObj.transform.position += Vector3.left * Time.deltaTime * camSpeed;
        }
        if (cam.transform.rotation.y > leftRightAngle && camObj.transform.position.x < 6.5f) {
            camObj.transform.position += Vector3.right * Time.deltaTime * camSpeed;
        }
    }
}
