using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _source;
    public static SFXManager Instance {  get; private set; }
    private void Awake()
    {
        Instance = Instance == null ? this : Instance;
        _source = GetComponent<AudioSource>();
    }
    private void OnEnable() { GameController.OnGameStateChanged += WhenStateChanged;
        PlayerController.OnDied += WhenDiedSound; EnemyController.OnEnemyDied += WhenEnemyDiedSound; PlayerController.WhenExtraLifeUsed += ExtraUsedSound; }
    private void OnDisable() { GameController.OnGameStateChanged -= WhenStateChanged;
        PlayerController.OnDied -= WhenDiedSound; EnemyController.OnEnemyDied -= WhenEnemyDiedSound; PlayerController.WhenExtraLifeUsed -= ExtraUsedSound; }

    private void ExtraUsedSound(int _, BasePlayerSO _1)
    {
        _source.PlayOneShot(_clips[2]);
    }

    private void WhenStateChanged(GameState state)
    {
        if (state == GameState.Start)
            PlayOnStart();
    }

    private void WhenDiedSound(BasePlayerSO _, Vector2 _p) { _source.pitch = Random.Range(0.9f, 1.1f); _source.PlayOneShot(_clips[1]); }

    private void WhenEnemyDiedSound(BaseEnemySO enemy,Vector2 _)
    {
        _source.pitch = Random.Range(0.9f, 1.1f);
        if (enemy.ExpType == ExplosionType.Boss)
            _source.PlayOneShot(_clips[4]);
        else
            _source.PlayOneShot(_clips[5]);
    }

    public void PlayOnShot() { _source.pitch = Random.Range(0.9f, 1.1f); _source.PlayOneShot(_clips[6]); }
    public void PlayOnStart() { _source.pitch = 1f; _source.PlayOneShot(_clips[^1]); }
    public void WhenDive(){ _source.pitch = Random.Range(0.9f, 1.1f); _source.PlayOneShot(_clips[3]); }

}
