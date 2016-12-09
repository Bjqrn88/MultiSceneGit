using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeMoveCamScirpt : MonoBehaviour {

    public Camera cam;
    public GameObject camObj;
    public GameObject image;
    
    private GvrViewer gvr;
    private float swipeSpeed = 5f;
    private float zoomSpeed = 5f;
    private Vector3 nextCamPos;

    // Use this for initialization
    void Start() {
        gvr = new GvrViewer();
        gvr.Recenter();
        if (cam == null)
            cam = Camera.main;
        if (camObj == null)
            camObj = GameObject.Find("CameraObject");
        if (image == null)
            image = GameObject.Find("ImagePlane");
    }

    // Update is called once per frame
    void Update () {
        // Quit on back button pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.touchCount > 0) {
            Touch mTouch = Input.GetTouch(0);
            if (mTouch.phase == TouchPhase.Stationary && mTouch.tapCount > 1) {
                if (mTouch.position.x > Screen.width / 2) {
                    if (image.transform.position.z > 2f) {
                        image.transform.position += Vector3.back * Time.deltaTime * zoomSpeed;
                    }
                } else {
                    if (image.transform.position.z < 10f) {
                        image.transform.position += Vector3.forward * Time.deltaTime * zoomSpeed;
                    }
                }
            }

            if (camObj.transform.position.y <= 10f && 
                camObj.transform.position.y >= -10f && 
                camObj.transform.position.x >= -15f && 
                camObj.transform.position.x <= 15f) {
                nextCamPos = camObj.transform.position + new Vector3(mTouch.deltaPosition.x, mTouch.deltaPosition.y, 0) * Time.deltaTime * swipeSpeed;
                if (nextCamPos.x > 15f) nextCamPos.x = 15f;
                if (nextCamPos.x < -15f) nextCamPos.x = -15f;
                if (nextCamPos.y > 10f) nextCamPos.y = 10f;
                if (nextCamPos.y < -10f) nextCamPos.y = -10f;
                camObj.transform.position = nextCamPos;
            }

            if (mTouch.deltaTime > 5f) {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}