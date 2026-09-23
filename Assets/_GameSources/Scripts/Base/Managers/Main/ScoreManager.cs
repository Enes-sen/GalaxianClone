using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int, int> WhenScoreChnaged;
    private int ActiveScore;
    [SerializeField] private string HText;
    private int HighScore;
    private void Awake() { ActiveScore = 0;}
    private void Start() { HighScore = PlayerPrefs.GetInt(HText, 0); WhenScoreChnaged?.Invoke(ActiveScore, HighScore); }
    private void OnEnable() => EnemyController.OnEnemyDied += AddScore;

    private void AddScore(BaseEnemySO enemy,Vector2 _)
    {
        ActiveScore += enemy.ScoreValue;
        if (ActiveScore > HighScore)
        { HighScore = ActiveScore;  PlayerPrefs.SetInt(HText, HighScore);}
        WhenScoreChnaged?.Invoke(ActiveScore, HighScore);
    }
    private void OnDisable() => EnemyController.OnEnemyDied -= AddScore;
}
