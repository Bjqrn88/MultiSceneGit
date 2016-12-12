using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetImageScript : MonoBehaviour {

    private string fileName = "mua5K.jpg";
    private WWW req;
    private Texture2D texture;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public IEnumerator SetImage() {
        var pathAndroid = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
        //var path = "file://" + Application.streamingAssetsPath + "/" + fileName; // For windows

        req = new WWW(pathAndroid);
        yield return req;
        if (!string.IsNullOrEmpty(req.error)) {
            Debug.Log("Can't read");
        }

        texture = req.texture;
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
