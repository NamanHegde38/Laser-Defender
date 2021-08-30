using System;
using UnityEngine;

public class Spinner : MonoBehaviour {

    [SerializeField] private float speedOfSpin = 1f;

    private void Update() {
        transform.Rotate(0, 0, speedOfSpin * Time.deltaTime);
    }
}
