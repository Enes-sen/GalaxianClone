using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour,IPool
{
    [Header("Bullet Settings")]
    [SerializeField] private float MaxY;
    [SerializeField] private Vector2 _sppos;
    Queue<IPool> _refPool;
    Rigidbody2D _rb;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();
    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.y) >= MaxY)
            ResetObject(_sppos, pool: _refPool);
    }
    public GameObject GetGameObject() => this.gameObject;

    public void ResetObject(Vector2 pos, Queue<IPool> pool)
    {
        transform.position = pos;
        if (_rb != null) _rb.velocity = Vector2.zero;

        gameObject.SetActive(false);
        if (pool != null)
        {
            PoolManagment.Instance.AddMember(pool, this);
        }
    }
    public void SetPoolRef(Queue<IPool> pool) => _refPool = pool;
    public void SetBullet(Vector2 dir, float _speed) { SFXManager.Instance.PlayOnShot(); _rb.velocity = dir * _speed; }
    public void SetPos(Vector2 _ps) => _sppos = _ps;
    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.TryGetComponent<IDieable>(out var dieable))
        {
            dieable.Die();
            ResetObject(_sppos, _refPool);
        }

    }
}
