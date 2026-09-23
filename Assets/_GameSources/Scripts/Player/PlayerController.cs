using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDieable
{
    [SerializeField] private BasePlayerSO _playerso;
    private Rigidbody2D _rb;
    private IMove _mv;
    private PlayerShootSystem _Shootsys;
    private bool _isdied;
    public static event Action<BasePlayerSO,Vector2> OnDied;
    public static event Action<int,BasePlayerSO> WhenExtraLifeUsed;
    private int _CurrentH;
    private bool _canTDo;
    public bool Isdied => _isdied;
    private void Awake()
    {
        _isdied = false;
        _canTDo = false;
        _CurrentH = _playerso.MaxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _mv = GetComponent<IMove>();
        _Shootsys = GetComponent<PlayerShootSystem>();
    }
    private void OnEnable()
    {
        _isdied = false;

        GameController.OnGameStateChanged += HandleGameStateChange;
        
    }
    private void OnDisable()
    {
        GameController.OnGameStateChanged -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState state)
    {
        if (state == GameState.GameOver || state == GameState.Stop)
            _canTDo = true;
        else
            _canTDo = false;
    }

    private void Update()
    {
        if (InputManager.Instance.Shoot() && _Shootsys.CanShoot && !Isdied && !_canTDo)
        {
            _Shootsys.Shoot();
        }
        if (_CurrentH > _playerso.MinHealth && _CurrentH != _playerso.MinHealth)
        {
            SetDied(false);
        }
    }

    private void FixedUpdate()
    {
        if (!Isdied && !_canTDo)
        {
            _mv.Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.TryGetComponent<IDieable>(out var _en))
        {
            _en.Die();
            Die();
        }
    }

    public bool SetDied(bool _con) => _isdied = _con;

    #region HelpFunctions
    public Rigidbody2D PLb() => _rb;
    public BasePlayerSO PLSo() => _playerso;

    public void Die()
    {
        if (Isdied) return;
        SetDied(true);
        _CurrentH--;
        if (_CurrentH > _playerso.MinHealth)
        {
            WhenExtraLifeUsed?.Invoke(_CurrentH, _playerso);
            transform.position = new Vector2(0, transform.position.y);
            _rb.velocity = Vector2.zero;
            SetDied(false);
        }
        else
        {
            gameObject.SetActive(false);
            WhenExtraLifeUsed?.Invoke(_CurrentH, _playerso);
            OnDied?.Invoke(_playerso, transform.position);
        }
    }
    #endregion
}
