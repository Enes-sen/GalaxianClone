using System;
using System.Collections;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    [Header("Manager Settings")]
    [SerializeField] private float _timeBetweenWaves = 3f;
    [SerializeField] private float _difficultyStep = 0.15f;
    private int _CurrentWave = 0;
    private int _RemainingEnemies = 0;
    public static event Action<float> OnStartWave;
    public static event Action<int> OnWaveNumberChanged;
    private void Start()
    {_CurrentWave = 0;_RemainingEnemies = 0; StartNextWave(); }

    private void StartNextWave()
    {
        _CurrentWave++;
        OnWaveNumberChanged?.Invoke(_CurrentWave);
        OnStartWave?.Invoke(_difficultyStep);
    }

    private void OnEnable() { FormationManager.OnActivateDone += SetActive; EnemyController.OnEnemyDied += WhenEnemyDied;  }

    private void SetActive(int remaining) => _RemainingEnemies = remaining;

    private void WhenEnemyDied(BaseEnemySO sO, Vector2 _)
    {
        _RemainingEnemies--;
        if (_RemainingEnemies <= 0)
        {
            _RemainingEnemies = 0;
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    private IEnumerator WaitAndStartNextWave()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);
        StartNextWave();
    }

    private void OnDisable() { FormationManager.OnActivateDone -= SetActive; EnemyController.OnEnemyDied -= WhenEnemyDied; }
}
