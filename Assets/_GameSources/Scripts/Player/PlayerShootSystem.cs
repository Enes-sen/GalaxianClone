using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootSystem : MonoBehaviour,IShoot
{
    private Queue<IPool> _bulletsPool = new();
    [Header("Settings")]
    [SerializeField] private int Poolsize;
    [SerializeField] private int BulletSpeed;
    [Header("Referances")]
    [SerializeField] private Transform Shotpos;
    [SerializeField] private Bullet Bulletfab;
    private bool _canShoot;
    private readonly WaitForSeconds _fixedDelay = new(0.3f);
    public bool CanShoot => _canShoot;
    private void Start()
    {
        _canShoot = true;
        for (int index = 0; index < Poolsize; index++)
        {
            var _poolmem = Instantiate<GameObject>(Bulletfab.gameObject,Shotpos.position,Quaternion.identity);
            _poolmem.GetComponent<Bullet>().SetPoolRef(_bulletsPool);
            PoolManagment.Instance.AddMember(_bulletsPool, _poolmem.GetComponent<IPool>());
            _poolmem.SetActive(false);
        }
    }
    public void Shoot()
    {
        if (PoolManagment.Instance.TryFetchMember(_bulletsPool,out var member))
        {
            member.GetGameObject().transform.position = Shotpos.position;
            member.GetGameObject().SetActive(true);
            member.SetPoolRef(_bulletsPool);
            if (member is Bullet _bullet)
            {
                _bullet.SetPos(Shotpos.position);
                _bullet.SetBullet(Vector2.up, BulletSpeed);
            }
            _canShoot = false;
            StartCoroutine(Firedelay());
            
        }
    }

    private IEnumerator Firedelay()
    {
        yield return _fixedDelay;
        _canShoot = true;
    }
}
