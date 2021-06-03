using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

    [SerializeField]
    private Texture2D fadeOutTexture;
    [SerializeField]
    private float fadeSpeed = 0.8f;

    private readonly int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture );
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        BeginFade(-1);
    }
    void OnEnable()
    {
        //Tell our 'BeginFade' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnFinishedLoading;
    }
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnFinishedLoading;
    }
}
