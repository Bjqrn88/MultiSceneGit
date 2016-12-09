using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        // Quit on back button pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void ChangeToDemo() {
        SceneManager.LoadScene("SelectScene2");
    }

    public void ChangeToWorkingDemo() {
        SceneManager.LoadScene("SelectScene");
    }
}
