using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField] private float backgroundScrollSpeed = 0.5f;

    private Material _myMaterial;
    private Vector2 _offset;
    private Vector2 _currentOffset;
    private static readonly int Offset = Shader.PropertyToID("_Offset");

    private void Start() {
        _myMaterial = GetComponent<Renderer>().material;
        _offset = new Vector2(0f, backgroundScrollSpeed);
    }

    private void Update() {
        _currentOffset += _offset * Time.deltaTime;
        _myMaterial.SetVector(Offset, _currentOffset);
    }
}
