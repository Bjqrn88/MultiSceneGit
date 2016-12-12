using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseCam : MonoBehaviour {

    public Camera cam;
    public GameObject camObj;

    public int camSpeed = 5;
    public float leftRightAngle = 0.35f;
    private GvrViewer gvr;

    // Use this for initialization
    void Start() {
        gvr = new GvrViewer();
        if (cam == null)
            cam = Camera.main;
        if (camObj == null)
            camObj = GameObject.Find("CameraObject");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0) {
            Touch mTouch = Input.GetTouch(0);
            if (mTouch.tapCount > 2) {
                gvr.Recenter();
            }
        }

        // Move the camera left or right
        if (cam.transform.rotation.y < -leftRightAngle && camObj.transform.position.x > -6.5f) {
            camObj.transform.position += Vector3.left * Time.deltaTime * camSpeed;
        }
        if (cam.transform.rotation.y > leftRightAngle && camObj.transform.position.x < 6.5f) {
            camObj.transform.position += Vector3.right * Time.deltaTime * camSpeed;
        }
    }
}
