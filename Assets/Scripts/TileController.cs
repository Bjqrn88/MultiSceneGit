using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileController : MonoBehaviour {

    public Renderer t0;
    public Renderer t1;
    public Renderer t2;
    public Renderer t3;
    public Renderer t4;
    public Renderer t5;
    public Renderer t6;
    public Renderer t7;
    public Renderer t8;
    public Renderer t9;
    public Renderer t10;
    public Renderer t11;
    public int zoomLvl = 2;
    public int maxZoomLvl = 1;
    public int row = 1;
    public int column = 1;
    public GameObject Tiles;
    public GameObject Controller;
    public Light LightSource;
    public float rotateSpeed;
    public int curRow = 0;
    public int curColum = 0;

    private GvrViewer gvr;

    private Texture2D t0Tex;
    private Texture2D t1Tex;
    private Texture2D t2Tex;
    private Texture2D t3Tex;
    private Texture2D t4Tex;
    private Texture2D t5Tex;
    private Texture2D t6Tex;
    private Texture2D t7Tex;
    private Texture2D t8Tex;
    private Texture2D t9Tex;
    private Texture2D t10Tex;
    private Texture2D t11Tex;
    private bool zIn = false;
    private bool zOut = false;
    private bool maxZoomLvlReached = false;
    private bool scroll = false;
    private bool left = false;
    private bool moved = true;
    private bool up = false;

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        t0.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom0xx0") as Texture2D;
        t1.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom0xx1") as Texture2D;
        t2.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom0xx2") as Texture2D;
        t3.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom0xx3") as Texture2D;

        t4.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom1xx0") as Texture2D;
        t5.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom1xx1") as Texture2D;
        t6.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom1xx2") as Texture2D;
        t7.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom1xx3") as Texture2D;

        t8.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom2xx0") as Texture2D;
        t9.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom2xx1") as Texture2D;
        t10.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom2xx2") as Texture2D;
        t11.material.mainTexture = Resources.Load("Images/" + zoomLvl + "/zoom2xx3") as Texture2D;
    }

    // Use this for initialization
    void Start() {
        gvr = new GvrViewer();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.touchCount > 0) {
            Touch mTouch = Input.GetTouch(0);

            if (mTouch.phase == TouchPhase.Stationary && mTouch.tapCount > 1) {
                if (mTouch.position.x > Screen.width / 2) {
                    ZoomIn();
                }
                else {
                    ZoomOut();
                }
            }

            if (mTouch.tapCount > 2) {
                gvr.Recenter();
            }

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
                            else {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f) {
                                if (swipeType.x > 0.0f) {
                                    // MOVE RIGHT
                                    ScrollRight();
                                }
                                else {
                                    // MOVE LEFT
                                    ScrollLeft();
                                }
                            }

                            if (swipeType.y != 0.0f) {
                                if (swipeType.y > 0.0f) {
                                    // MOVE UP
                                    ScrollUp();
                                }
                                else {
                                    // MOVE DOWN
                                    ScrollDown();
                                }
                            }
                        }
                        break;
                }
            }
        }

        if (zIn) {
            StartCoroutine(CoGetTiles());
            StartCoroutine(CoCheckBounds());
            StartCoroutine(CoSetTiles());
            zIn = false;
        }

        if (zOut) {
            StartCoroutine(CoGetTiles());
            StartCoroutine(CoCheckBounds());
            StartCoroutine(CoSetTiles());
            zOut = false;
        }

        if (scroll) {
            scroll = false;
            StartCoroutine(CoGetTiles());
            StartCoroutine(CoCheckBounds());
            StartCoroutine(CoSetTiles());
            moved = true;
        }
    }

    public void ZoomOut() {
        if (!(zoomLvl >= 2)) {
            zoomLvl += 1;
            column = column / 2;
            row = row / 2;
            zOut = true;
        }
    }

    public void ZoomIn() {
        zoomLvl -= 1;
        if (zoomLvl < maxZoomLvl) {
            maxZoomLvlReached = true;
            zoomLvl = maxZoomLvl;
        }
        else {
            row = row + (curRow * 2);
            column = column + (curColum * 2);
            zIn = true;
        }
    }

    // Sets the row
    public void SetRow(int rowNum) {
        if (!maxZoomLvlReached) {
            curRow = rowNum;
        }
    }

    // Set the column
    public void SetColumn(int colNum) {
        if (!maxZoomLvlReached) {
            curColum = colNum;
        }
    }

    // Loads the tiles from the Resources folder
    IEnumerator CoGetTiles() {
        t0Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row - 1) + "xx" + (column - 1)) as Texture2D;
        t1Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row - 1) + "xx" + column) as Texture2D;
        t2Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row - 1) + "xx" + (column + 1)) as Texture2D;
        t3Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row - 1) + "xx" + (column + 2)) as Texture2D;

        t4Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + row + "xx" + (column - 1)) as Texture2D;
        t5Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + row + "xx" + column) as Texture2D;
        t6Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + row + "xx" + (column + 1)) as Texture2D;
        t7Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + row + "xx" + (column + 2)) as Texture2D;

        t8Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row + 1) + "xx" + (column - 1)) as Texture2D;
        t9Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row + 1) + "xx" + column) as Texture2D;
        t10Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row + 1) + "xx" + (column + 1)) as Texture2D;
        t11Tex = Resources.Load("Images/" + zoomLvl + "/zoom" + (row + 1) + "xx" + (column + 2)) as Texture2D;
        yield return null;
    }

    IEnumerator CoSetTiles() {
        t0.material.mainTexture = t0Tex;
        t1.material.mainTexture = t1Tex;
        t2.material.mainTexture = t2Tex;
        t3.material.mainTexture = t3Tex;

        t4.material.mainTexture = t4Tex;
        t5.material.mainTexture = t5Tex;
        t6.material.mainTexture = t6Tex;
        t7.material.mainTexture = t7Tex;

        t8.material.mainTexture = t8Tex;
        t9.material.mainTexture = t9Tex;
        t10.material.mainTexture = t10Tex;
        t11.material.mainTexture = t11Tex;
        yield return null;
    }

    public void ScrollLeft() {
        left = true;
        column -= 1;
        scroll = true;
    }

    public void ScrollRight() {
        column += 1;
        scroll = true;
    }

    public void ScrollUp() {
        if (moved) {
            moved = false;
            up = true;
            --row;
            scroll = true;
        }
    }

    public void ScrollDown() {
        if (moved) {
            moved = false;
            ++row;
            scroll = true;
        }
    }

    IEnumerator CoCheckBounds() {
        if (t0Tex == null && left) {
            column += 1;
            left = false;
            StartCoroutine(CoGetTiles());
        }
        else if (t0Tex == null && up) {
            row += 1;
            up = false;
            StartCoroutine(CoGetTiles());
        }
        else if (t3Tex == null) {
            column -= 1;
            StartCoroutine(CoGetTiles());
        }
        else if (t8Tex == null) {
            row -= 1;
            StartCoroutine(CoGetTiles());
        }
        yield return null;
    }
}
