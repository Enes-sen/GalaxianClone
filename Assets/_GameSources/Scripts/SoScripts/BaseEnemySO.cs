using UnityEngine;
[CreateAssetMenu(fileName = "EnemySO", menuName = "Objects/EnemySO")]
public class BaseEnemySO : ScriptableObject
{
    [Header("Enemy Settings")]
    public int ScoreValue=100;
    public GameObject Prefab;
    public float Speed=5f;
    public int fireRate=3;
    [Header("Explosion Settings")]
    public ExplosionType ExpType;
    public float FrameRate = 0.05f;
    public Sprite[] Frames;
}
