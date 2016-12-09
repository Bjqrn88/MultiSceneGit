using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SelectSceneScript : MonoBehaviour {

    private AsyncOperation result;
    private GvrViewer gvr;

    // Use this for initialization
    void Start() {
        gvr = new GvrViewer();
        gvr.Recenter();
        // Preloading the scene 
        result = SceneManager.LoadSceneAsync("MoveCamScene");
        result.allowSceneActivation = false;
	}
	
	// Update is called once per frame
	void Update () {
        // Quit on back button pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void ChangeScene() {
        result.allowSceneActivation = true;
    }

    public void ChangeScene2() {
        SceneManager.LoadScene("SwipeMoveScene");
    }

    public void ChangeScene3() {
        SceneManager.LoadScene("TilingScene");
    }
}
