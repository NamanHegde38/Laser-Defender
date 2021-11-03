using UnityEngine;

public class Background : MonoBehaviour {
    
    private void Awake() {
        var numberOfObjects = FindObjectsOfType<Background>().Length;
        if (numberOfObjects > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
