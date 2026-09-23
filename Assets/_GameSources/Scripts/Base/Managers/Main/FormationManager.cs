using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FormationManager : MonoBehaviour
{
    #region Formation Settings
    [Header("Formation Move Settings ")]
    [SerializeField] private Vector2 Abspos;
    [SerializeField] private PlayerController _Ppos;
    [SerializeField] private float _Timer;
    #endregion
    #region EnemySpawn Settings
    [Header("EnemySpawn Settings")]
    [SerializeField] private WaveLayoutSO _enemyTypes;
    [SerializeField] private int _rows=6;
    [SerializeField] private float _spacingX = 2.5f;
    [SerializeField] private float _spacingY = 2.5f;
    private int _colums=6;
    private Queue<IPool> _Armypool;
    private int _activeCount;
    public static event Action<int> OnActivateDone;
    #endregion

    private void Awake() { _Armypool = new(); GenerateFormedArmy(); }
    private void Start() => StartCoroutine(FormationMove());
    private void OnEnable() => WaveManager.OnStartWave+=ActivateWave;
    private void ActivateWave(float _multi) => ActivateEnemies(_multi);

    private void OnDisable() => WaveManager.OnStartWave -= ActivateWave;
    #region FormationMove
    private IEnumerator FormationMove()
    {
        transform.position = new Vector2(0,8f);
        while (true)
        {
            Vector2 rightTarget = new(Abspos.x, transform.position.y);
            while (Vector2.Distance(transform.position, rightTarget) > 0.01f)
            {
                rightTarget.y = transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, rightTarget, _Timer * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            Vector2 leftTarget = new(-Abspos.x, transform.position.y);
            while (Vector2.Distance(transform.position, leftTarget) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, leftTarget, _Timer * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            float distanceY = transform.position.y - _Ppos.transform.position.y;
            if (distanceY >= 2.0f)
            {
                transform.position += Vector3.down * 0.3f;
            }
        }
    }
    #endregion
    #region EnemyGenerateInFormation
    public void GenerateFormedArmy()
    {
        var totalH = (_rows - 1) * _spacingY;
        for (int row = 0; row < _rows; row++)
        {
            var totalW = (_colums - 1) * _spacingX;
            var StartPos = new Vector2(-totalW / 2f, totalH / 2f);
            if (row >= _enemyTypes.RawEnemies.Length) break;
            var enemyFab = _enemyTypes.RawEnemies[row].Prefab;
            for (int col = 0; col < _colums; col++)
            {
                Vector2 spawnpos = new((StartPos.x + (col * _spacingX)), (StartPos.y - (row * _spacingY)));
                var newEnemy = Instantiate(enemyFab,transform);
                newEnemy.transform.localPosition = spawnpos;
                if (newEnemy.TryGetComponent<IEnemy>(out var _solider))
                {

                    _solider.SetPoolRef(_Armypool);
                    _solider.Formationpos = spawnpos;
                    _solider.SetFParrent(this);
                    _solider.SetTargetP(_Ppos);
                    PoolManagment.Instance.AddMember(_Armypool, _solider);
                    newEnemy.SetActive(false);
                }


            }
            _colums = _colums != 10 ? _colums + 2 : _colums;
        }
    }
    private void ActivateEnemies (float _difficultyMultiplier)
    {
        _activeCount = 0;
        transform.position = Vector2.zero;
        while (PoolManagment.Instance.TryFetchMember(_Armypool, out var member))
        {
            var solider = member.GetGameObject();
            solider.transform.SetParent(transform);
            if (member is IEnemy enemy)
            { solider.transform.localPosition = enemy.Formationpos;  enemy.SetPoolRef(_Armypool); enemy.ApplyDifficulty(_difficultyMultiplier); }
            solider.SetActive(true);
            _activeCount++;
        }
        OnActivateDone?.Invoke(_activeCount);
    }
    #endregion

}