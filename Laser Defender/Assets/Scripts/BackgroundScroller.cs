using System;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField] private float backgroundScrollSpeed = 0.5f;

    private Material _myMaterial;
    private Vector2 _offset;

    private void Start() {
        _myMaterial = GetComponent<Renderer>().material;
        _offset = new Vector2(0f, backgroundScrollSpeed);
    }

    private void Update() {
        _myMaterial.mainTextureOffset += _offset * Time.deltaTime;
    }
}
