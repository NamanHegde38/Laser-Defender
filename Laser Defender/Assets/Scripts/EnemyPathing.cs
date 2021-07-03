using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

    private WaveConfig _waveConfig;
    private List<Transform> _waypoints;
    private int _waypointIndex;

    private void Start() {
        _waypoints = _waveConfig.GetWaypoints();
        transform.position = _waypoints[_waypointIndex].transform.position;
    }

    private void Update() {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig) {
        _waveConfig = waveConfig;
    }
    
    private void Move() {
        if (_waypointIndex <= _waypoints.Count - 1) {
            var targetPosition = _waypoints[_waypointIndex].transform.position;
            var movementThisFrame = _waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition) {
                _waypointIndex++;
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
