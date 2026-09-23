using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExplosionVFXManager : MonoBehaviour, IPool
{
    private Queue<IPool> _PoolRef;
    private SpriteRenderer _sR;

    private void Awake() => _sR = GetComponent<SpriteRenderer>();
    public GameObject GetGameObject() => this.gameObject;

    public void ResetObject(Vector2 pos, Queue<IPool> pool)
    {
       transform.SetPositionAndRotation(pos, Quaternion.identity);
        if (PoolManagment.Instance.IsEmptyORNotPool(_PoolRef))
            PoolManagment.Instance.AddMember(_PoolRef, this);
        _sR.sprite = null;
        gameObject.SetActive(false);
    }
    public void ExplodedPlayFrames(Sprite[] frames, float frameRate)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayFrames(frames, frameRate));
    }
    private IEnumerator PlayFrames(Sprite[] frames,float frameRate)
    {
        foreach(var frame in frames)
        {
            _sR.sprite = frame;
            yield return new WaitForSeconds(frameRate);
        }
        ResetObject(Vector2.zero,_PoolRef);
    }
    public void SetPoolRef(Queue<IPool> pool) => _PoolRef = pool;
}