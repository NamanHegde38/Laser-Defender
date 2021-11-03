using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour {

    private TextMeshProUGUI _highScoreText;
    private GameSession _gameSession;

    private void Start() {
        _highScoreText = GetComponent<TextMeshProUGUI>();
        _gameSession = FindObjectOfType<GameSession>();
    }
    
    private void Update() {
        if (!_gameSession) {
            _gameSession = FindObjectOfType<GameSession>();
        }
        _highScoreText.text = _gameSession.GetHighScore().ToString();
    }
}
