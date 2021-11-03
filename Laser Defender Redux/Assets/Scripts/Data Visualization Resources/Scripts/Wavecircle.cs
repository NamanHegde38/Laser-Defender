using UnityEngine;
using UnityEngine.UI;

public class Wavecircle : MonoBehaviour
{
    private float _no1;

    public Transform wave;
    public Transform s, e;

    public Text theText;
    public float animSpeed;

    private HealthDisplay _healthDisplay;
    private int _oldHealth;

    void Start() {
        _healthDisplay = theText.gameObject.GetComponent<HealthDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePercent(_healthDisplay.GetHealth());
    }

    void UpdatePercent(int f) {
        var health = f;

        if (_oldHealth != health) {
            var targetPosition = s.position + (e.position - s.position) * health / 100;
            wave.position = targetPosition;

            theText.text = Mathf.RoundToInt(health).ToString();
            _oldHealth = health;
        }
    }
}
