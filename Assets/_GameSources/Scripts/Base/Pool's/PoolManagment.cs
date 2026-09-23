using System.Collections.Generic;
using UnityEngine;

public class PoolManagment : MonoBehaviour
{
    public static PoolManagment Instance { get; private set; }

    private void Awake()
    {
        Instance = Instance != null ? Instance : this;
    }

    public void AddMember(Queue<IPool> pool, IPool member) => pool.Enqueue(member);
    public bool IsEmptyORNotPool(Queue<IPool> pool) => pool.Count == 0 || pool == null;
    public bool TryFetchMember(Queue<IPool> pool, out IPool member)
    {
        return pool.TryDequeue(out member);
    }

}
