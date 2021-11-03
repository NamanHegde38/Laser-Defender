using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private List<ReduxWaveConfig> waveConfigs;
    [SerializeField] private bool looping;

    private const int StartingWave = 0;
    private AudioListener _audioListener;

    private IEnumerator Start() {
        _audioListener = FindObjectOfType<AudioListener>();
        do {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves() {
        for (var waveIndex = StartingWave; waveIndex < waveConfigs.Count; waveIndex++) {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        } 
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnAllEnemiesInWave(ReduxWaveConfig waveConfig) {
        for (var enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++) {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetPath().GetComponent<PathCreator>().path.GetPoint(0),
                Quaternion.identity);
            if (waveConfig.GetSpawnSound()) {
                AudioSource.PlayClipAtPoint(waveConfig.GetSpawnSound(), _audioListener.transform.position, waveConfig.GetSpawnVolume());
            }
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
