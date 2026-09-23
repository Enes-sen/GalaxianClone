using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemy
{
    #region Enemy Serializeable Fields
    [Header("Enemy Controller Settings")]
    [SerializeField] private BaseEnemySO _basedata;
    [SerializeField] private bool _isdied;
    [SerializeField] private EnemyState _CurrentState = EnemyState.Formation;
    [SerializeField] private int _BulletPoolSize;
    [SerializeField] private float _WaitTime;
    [SerializeField] private Bullet _BulletFab;
    [SerializeField] private Transform _ShotPos;
    [SerializeField] private PlayerController _pl;
    [SerializeField] private Collider2D _Collider;
    #endregion

    #region Enemy Controled settings
    private FormationManager _FormationParrent;
    private Queue<IPool> poolref;
    private Queue<IPool> _BulletPool;
    private Rigidbody2D _rb;
    private readonly float _limitY = 21.5f;
    private readonly float _minTime = 20f;
    private readonly float _maxtime = 80f;
    private float _Multiplier=1f;
    private Vector2 _dir;
    private bool _shouldStop;
    public static Action<BaseEnemySO,Vector2> OnEnemyDied;
    #endregion

    #region Accessors
    public BaseEnemySO Enemydata => _basedata;
    public Vector2 Formationpos { get; set; }
    #endregion
    private void Awake() { _BulletPool = new(); _rb = GetComponent<Rigidbody2D>(); }
    private void Start()
    {
        _Multiplier = 1f;
        _CurrentState = EnemyState.Formation;
        _dir = Vector2.zero;
        SetTime();
        for (int index = 0; index < _BulletPoolSize; index++)
        {
            var _poolmem = Instantiate<GameObject>(_BulletFab.gameObject, transform.position, Quaternion.identity);
            _poolmem.GetComponent<Bullet>().SetPoolRef(_BulletPool);
            PoolManagment.Instance.AddMember(_BulletPool, _poolmem.GetComponent<IPool>());
            _poolmem.SetActive(false);
        }
    }
    public void SetTargetP(PlayerController pl) => _pl = pl; 
    private void SetTime() => _WaitTime = UnityEngine.Random.Range(_minTime, _maxtime);
    private void OnEnable() { _isdied = false;  GameController.OnGameStateChanged += HandleGameStateChange; }
    private void OnDisable() => GameController.OnGameStateChanged -= HandleGameStateChange;
    private void HandleGameStateChange(GameState state)
    {
       if(state == GameState.GameOver || state == GameState.Stop)
            _shouldStop = true;
       else _shouldStop = false;
    }
    private void FixedUpdate() => Move();
    public GameObject GetGameObject() => this.gameObject;

    public void Move()
    {
        if (_shouldStop) return;
        switch (_CurrentState)
        { 
            case EnemyState.Dive:
                ExecuteDive(_dir);
                break;
            case EnemyState.Return: 
                ExecuteReturn();
                break;
            case EnemyState.Formation:
            default:
                _WaitTime = _WaitTime > 0f ? _WaitTime - Time.fixedDeltaTime : 0f;
                if (_WaitTime <= 0f)
                   SetState(EnemyState.Dive);
                break;
        }
    }
    
    private void SetState( EnemyState state)
    {
        _CurrentState = state;
        if (_isdied) return;
        switch (state)
        {
            case EnemyState.Formation:
                _rb.velocity = Vector2.zero;
                transform.up = _FormationParrent.transform.up;
                _Collider.enabled = true;
                SetTime();
                break;
            case EnemyState.Dive:
                SFXManager.Instance.WhenDive();
                _rb.velocity = Vector2.zero;
                _Collider.enabled = true;
                 _dir = _pl != null && !_pl.Isdied ? (_pl.transform.position - transform.position).normalized:Vector2.down;
                if (transform.parent != null)
                    transform.SetParent(null);
                transform.up = _dir;
                InvokeRepeating(nameof(Shoot), 0f, Enemydata.fireRate/ _Multiplier);
                break;
            case EnemyState.Return:
                _Collider.enabled = false;
                CancelInvoke(nameof(Shoot));
                if (transform.parent == null)
                    transform.SetParent(_FormationParrent.transform);
                _rb.velocity = Vector2.zero;
                break;
            default:
                break;
        }
    }
    private void ExecuteReturn()
    {
        _Collider.enabled = false;
        var formationps = _FormationParrent.transform.TransformPoint(Formationpos);
        Vector2 target = ((Vector2)formationps - (Vector2)transform.position).normalized;
        transform.up = target;
        _rb.velocity = _basedata.Speed*_Multiplier *target;
        if (Vector2.Distance((Vector2)transform.position, (Vector2)formationps) <= 0.5f)
        {
            transform.localPosition = Formationpos;
            SetState(EnemyState.Formation);
        }
    }
    private void ExecuteDive(Vector2 _target)
    {   
        _rb.velocity = _basedata.Speed* _Multiplier * _target;
        if (transform.position.y <= -_limitY)
        {
            transform.position = new(transform.position.x, _limitY, transform.position.z);
            SetState(EnemyState.Return);
        }
        
    }

    public void ResetObject(Vector2 pos, Queue<IPool> pool)
    {
        if (transform.parent == null)
            transform.SetParent(_FormationParrent.transform);
        transform.localPosition = pos;
        _CurrentState = EnemyState.Formation;
        this.gameObject.SetActive(false);
        if (pool != null)
            PoolManagment.Instance.AddMember(pool, this);
    }

    public void Shoot()
    {
        if (PoolManagment.Instance.TryFetchMember(_BulletPool, out var member))
        {
            member.GetGameObject().transform.position = _ShotPos.position;
            member.GetGameObject().SetActive(true);
            member.SetPoolRef(_BulletPool);
            if (member is Bullet _bullet)
            {
                _bullet.SetPos(_ShotPos.position);
                _bullet.SetBullet(Vector2.down, Enemydata.fireRate * _Multiplier);
            }

        }
    }
    public void SetPoolRef(Queue<IPool> pool) =>poolref = pool;

    public void Die()
    {
        if (_isdied) return;
        OnEnemyDied?.Invoke(Enemydata,transform.position);
        _isdied = true;
        _rb.velocity = Vector2.zero;
        CancelInvoke(nameof(Shoot));
        ResetObject(Formationpos, poolref);
        
    }
    public void ApplyDifficulty(float DifucultyInc) => _Multiplier += DifucultyInc;
    public void SetFParrent(FormationManager parrent) => _FormationParrent = parrent; 
}
