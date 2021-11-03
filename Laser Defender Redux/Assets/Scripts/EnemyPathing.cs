using PathCreation;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

    private ReduxWaveConfig _waveConfig;
    private VertexPath _path;

    private float _distanceTravelled;

    private void Start() {
        _path = _waveConfig.GetPath().GetComponent<PathCreator>().path;
        transform.position = _path.GetPoint(0);
    }

    private void Update() {
        Move();
    }

    public void SetWaveConfig(ReduxWaveConfig waveConfig) {
        _waveConfig = waveConfig;
    }
    
    private void Move() {

        _distanceTravelled += _waveConfig.GetMoveSpeed() * Time.deltaTime;
        transform.position = _path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
        
        if (transform.position == _path.GetPoint(_path.NumPoints - 1)) {
            Destroy(gameObject);
        }

        /*if (_waypointIndex <= _path.Count - 1) {
            var targetPosition = _path[_waypointIndex].transform.position;
            var movementThisFrame = _waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition) {
                _waypointIndex++;
            }
        }
        else {
            Destroy(gameObject);
        }*/
    }
}
