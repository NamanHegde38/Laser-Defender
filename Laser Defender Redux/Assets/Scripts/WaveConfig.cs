using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float spawnRandomFactor = 0.3f;
    [SerializeField] private int numberOfEnemies = 5;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private AudioClip enemySpawnSFX;
    [Range(0, 1)] [SerializeField] private float spawnVolume;
    
    
    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }

    public List<Transform> GetWaypoints() {
        var waveWaypoints = new List<Transform>();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (Transform child in pathPrefab.transform) {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }

    public float GetTimeBetweenSpawns() {
        return timeBetweenSpawns;
    }

    public float GetSpawnRandomFactor() {
        return spawnRandomFactor;
    }

    public int GetNumberOfEnemies() {
        return numberOfEnemies;
    }

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public AudioClip GetSpawnSound() {
        return enemySpawnSFX;
    }

    public float GetSpawnVolume() {
        return spawnVolume;
    }
}
