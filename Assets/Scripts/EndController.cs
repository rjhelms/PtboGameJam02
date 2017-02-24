using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndController : MonoBehaviour {
    public Image blackout;
    public float fadeTime;
    public Color blackoutColor;
    public Color clearColor;
    public Color currentColor;
    public int sceneIndex = 0;

    // Use this for initialization
	void Start () {
        fadeTime = 0;
        Time.timeScale = 1;
	}

    // Update is called once per frame
    void Update() {
        fadeTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (fadeTime < 2)
        {
            currentColor = Color.Lerp(blackoutColor, clearColor, fadeTime / 2);
            blackout.color = currentColor;
            Debug.Log(blackout.color);
        } else
        {
            if (Input.anyKeyDown)
            {
                ScoreManager.Instance.Reset();
                SceneManager.LoadScene(sceneIndex);
            }
        }
	}
}
