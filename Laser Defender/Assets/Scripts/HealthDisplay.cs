using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour {
    
    private TextMeshProUGUI _healthText;
    private Player _player;

    private void Start() {
        _healthText = GetComponent<TextMeshProUGUI>();
        _player = FindObjectOfType<Player>();
    }

    private void Update() {
        _healthText.text = _player.GetHealth().ToString();
    }
}
