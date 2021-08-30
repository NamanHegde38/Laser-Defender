using System;
using UnityEngine;

public class GameSession : MonoBehaviour {

    private int _score;
    private int _highScore;
    
    private void Awake() {
        SetupSingleton();
    }
    
    private void SetupSingleton() {
        var numberOfObjects = FindObjectsOfType<GameSession>().Length;
        if (numberOfObjects > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start() {
        _highScore = PlayerPrefs.GetInt("High Score", 0);
    }

    public int GetScore() {
        return _score;
    }

    public void AddToScore(int scoreValue) {
        _score += scoreValue;
    }

    public int GetHighScore() {
        if (_score > _highScore) {
            _highScore = _score;
        }
        PlayerPrefs.SetInt("High Score", _highScore);
        PlayerPrefs.Save();
        return _highScore;
    }
    
    public void ResetGame() {
        Destroy(gameObject);
    }
}
