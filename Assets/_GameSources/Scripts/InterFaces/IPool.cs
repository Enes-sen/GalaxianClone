using System.Collections.Generic;
using UnityEngine;
public interface IPool
{
    GameObject GetGameObject();
    void ResetObject(Vector2 pos, Queue<IPool> pool);
    void SetPoolRef(Queue<IPool> pool);
}