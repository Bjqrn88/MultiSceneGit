using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartChangeScene : MonoBehaviour {

    public Text textField;
    private GvrViewer gvr;

    // Use this for initialization
    void Start () {
        gvr = new GvrViewer();
    }
	
	// Update is called once per frame
	void Update () {
        // Quit on back button pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.touchCount > 0) {
            Touch mTouch = Input.GetTouch(0);
            if (mTouch.tapCount > 2) {
                gvr.Recenter();
            }
        }
    }
}
