using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCameraScript : MonoBehaviour {

    // Camera travel speed and zoom speed
    public int camSpeed = 5;
    public int zoomSpeed = 5;
    // Angle for moving the camera
    public float upDownAngle = 0.25f;
    public float leftRightAngle = 0.35f;

    public Camera cam;
    public GameObject camObj;
    public GameObject image;

    private GameObject canvas;
    private GvrViewer gvr;

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
        StartCoroutine(getHighRes());
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
                    
                }
                else {
                    if (image.transform.position.z < 10f) {
                        image.transform.position += Vector3.forward * Time.deltaTime * zoomSpeed;
                    }
                }
            }

            if (mTouch.tapCount > 2) {
                gvr.Recenter();
            }

            if (mTouch.deltaTime > 5f) {
                SceneManager.LoadScene("StartScene");
            }
        }

        // Move the camera up or down
        if (cam.transform.rotation.x < -upDownAngle && camObj.transform.position.y < 10f) {
            camObj.transform.position += Vector3.up * Time.deltaTime * camSpeed;
        }
        if (cam.transform.rotation.x > upDownAngle && camObj.transform.position.y > -10f) {
            camObj.transform.position += Vector3.down * Time.deltaTime * camSpeed;
        }

        // Move the camera left or right
        if (cam.transform.rotation.y < -leftRightAngle && camObj.transform.position.x > -15f) {
            camObj.transform.position += Vector3.left * Time.deltaTime * camSpeed;
        }
        if (cam.transform.rotation.y > leftRightAngle && camObj.transform.position.x < 15f) {
            camObj.transform.position += Vector3.right * Time.deltaTime * camSpeed;
        }
    }

    IEnumerator getHighRes() {
        SetImageScript sis = image.GetComponent<SetImageScript>();
        StartCoroutine(sis.SetImage());
        yield return null;
    }
}
