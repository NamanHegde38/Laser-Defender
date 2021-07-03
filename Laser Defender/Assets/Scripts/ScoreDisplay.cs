using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour {

    private TextMeshProUGUI _scoreText;
    private GameSession _gameSession;

    private void Start() {
        _scoreText = GetComponent<TextMeshProUGUI>();
        _gameSession = FindObjectOfType<GameSession>();
    }

    private void Update() {
        _scoreText.text = _gameSession.GetScore().ToString();
    }
}
