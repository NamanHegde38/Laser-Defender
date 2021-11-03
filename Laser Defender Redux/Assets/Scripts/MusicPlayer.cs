using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    
    private void Awake() {
        var numberOfObjects = FindObjectsOfType<MusicPlayer>().Length;
        if (numberOfObjects > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
}