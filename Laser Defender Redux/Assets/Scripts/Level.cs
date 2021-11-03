using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class Level : MonoBehaviour {

    [SerializeField] private float delayInSeconds;

    [SerializeField] private MMFeedbacks startFeedback, quitFeedback;
    [SerializeField] private MMFeedbacks loadStartFeedback, loadEndFeedback;

    private void Start() {
        loadEndFeedback.Initialization();
        PlayLoadFeedbacks();
    }
    
    private void PlayLoadFeedbacks() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex != 1) {
            loadEndFeedback?.PlayFeedbacks();
        }
        else {
            var splashScreen = FindObjectOfType<SplashScreen>();
            if (!splashScreen) {
                loadEndFeedback?.PlayFeedbacks();
                Debug.Log(loadEndFeedback.IsPlaying);
            }
            else {
                Destroy(splashScreen);
                startFeedback?.PlayFeedbacks();
            }
        }
    }
    
    public void LoadStartScene() {
        FindObjectOfType<GameSession>().ResetGame();
        StartCoroutine(LoadScene(1));
    }

    public void LoadGame() {
        StartCoroutine(LoadScene(2));
    }
    
    public void LoadGameOver() {
        StartCoroutine(WaitAndLoad());
    }
    
    private IEnumerator WaitAndLoad() {
        yield return new WaitForSeconds(delayInSeconds);
        StartCoroutine(LoadScene(3));
    }
    
    private IEnumerator LoadScene(int sceneIndex) {
        loadStartFeedback?.PlayFeedbacks();
        if (loadStartFeedback is { })
            yield return new WaitForSeconds(loadStartFeedback.TotalDuration);
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame() {
        StartCoroutine(Quit());
    }
    
    private IEnumerator Quit() {
        quitFeedback?.PlayFeedbacks();
        if (quitFeedback is { })
            yield return new WaitForSeconds(quitFeedback.TotalDuration);
        Application.Quit();
    }
}
