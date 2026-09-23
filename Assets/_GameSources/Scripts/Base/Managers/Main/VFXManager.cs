using System.Collections.Generic;
using UnityEngine;
public class VFXManager : MonoBehaviour
{
    [SerializeField] private ExplosionVFXManager _basePrefab;
    [SerializeField] private int _poolSize;
    private Queue<IPool> _explosivePool;
    private void Awake() {_explosivePool = new(); }

    private void OnEnable() { EnemyController.OnEnemyDied += WheneDied; PlayerController.OnDied += WhenPlayerDead; }

    private void OnDisable() { EnemyController.OnEnemyDied -= WheneDied; PlayerController.OnDied -= WhenPlayerDead; }
    private void WhenPlayerDead(BasePlayerSO player, Vector2 _pos)
    {
        var _Frames = player.Frames;
        var _FrameRate = player.FrameRate;
        FetchExploded(_pos, _Frames,_FrameRate);
    }
    private void WheneDied(BaseEnemySO DiedOne,Vector2 _pos)
    {
        var _Frames = DiedOne.Frames;
        var _FrameRate = DiedOne.FrameRate;
        FetchExploded(_pos,_Frames,_FrameRate);
    }

    private void FetchExploded(Vector2 _pos,Sprite[] frames,float _fps)
    {
        if (!PoolManagment.Instance.IsEmptyORNotPool(_explosivePool))
        {
            if (PoolManagment.Instance.TryFetchMember(_explosivePool, out var _Exploded))
            {
                if (_Exploded.GetGameObject().TryGetComponent<ExplosionVFXManager>(out var manage))
                {
                    manage.transform.position = _pos;
                    manage.ExplodedPlayFrames(frames,_fps);
                }
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var _poolmem = Instantiate<GameObject>(_basePrefab.gameObject, transform.position, Quaternion.identity);
            _poolmem.GetComponent<ExplosionVFXManager>().SetPoolRef(_explosivePool);
            PoolManagment.Instance.AddMember(_explosivePool, _poolmem.GetComponent<IPool>());
            _poolmem.SetActive(false);
        }
    }
}
