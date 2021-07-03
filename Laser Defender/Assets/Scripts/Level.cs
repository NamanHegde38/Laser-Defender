using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    [SerializeField] private float delayInSeconds;
    
    public void LoadStartMenu() {
        FindObjectOfType<GameSession>().ResetGame();
        SceneManager.LoadScene(0);
    }

    public void LoadGame() {
        SceneManager.LoadScene("Game");
    }

    public void LoadGameOver() {
        StartCoroutine(WaitAndLoad());
    }

    private IEnumerator WaitAndLoad() {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }
    
    public void QuitGame() {
        Application.Quit();
    }
}
