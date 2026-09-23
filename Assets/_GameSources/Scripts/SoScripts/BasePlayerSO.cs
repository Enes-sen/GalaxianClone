using UnityEngine;
[CreateAssetMenu(fileName ="playerSo",menuName ="Objects/playerSo")]
public class BasePlayerSO : ScriptableObject
{
    [Header("Player Settings")]
    public int MaxHealth=3;
    public int MinHealth=0;
    public int Speed=6;
    [Header("Explosion Settings")]
    public ExplosionType ExpType;
    public float FrameRate = 0.05f;
    public Sprite[] Frames;
}
