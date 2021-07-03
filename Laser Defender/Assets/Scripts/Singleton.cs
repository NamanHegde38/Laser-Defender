using UnityEngine;

public class Singleton : MonoBehaviour {
    
    private void Awake() {
        var numberOfObjects = FindObjectsOfType<Singleton>().Length;
        if (numberOfObjects > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
