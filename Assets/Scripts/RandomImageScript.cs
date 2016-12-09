using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomImageScript : MonoBehaviour {

    private bool updateImages = false;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;
    // Use this for initialization
    void Start () {
        updateImages = true;
	}
	
	// Update is called once per frame
	void Update () {
        // Quit on back button pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (updateImages) {
            foreach (Renderer child in GetComponentsInChildren<Renderer>()) {
                StartCoroutine(getAndSetImage(child));
            }
            updateImages = false;
        }

        if (Input.touchCount > 0) {
            Touch mTouch = Input.GetTouch(0);

            if (mTouch.deltaTime > 5f) {
                SceneManager.LoadScene("StartScene");
            }

            foreach (Touch touch in Input.touches) {
                switch (touch.phase) {
                    case TouchPhase.Began:
                        /* this is a new touch */
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        /* The touch is being canceled */
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:

                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            if (swipeType.x != 0.0f) {
                                updateImages = true ;
                            }
                        }
                        break;
                }
            }
        }

    }

    IEnumerator getAndSetImage(Renderer child) {
        var path = "http://lorempixel.com/200/200/";

        var www = new WWW(path);
        yield return www;
        if (!string.IsNullOrEmpty(www.error)) {
            Debug.Log("Can't read");
        }
        child.material.mainTexture = www.texture;
        yield return null;
    }
}
