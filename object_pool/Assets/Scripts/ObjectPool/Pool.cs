using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ObjectPool
{
    public class Pool:MonoBehaviour {
        
        private readonly Stack<PooledObject> pool = new Stack<PooledObject>();
        private GameObject _prefab = null;
        
        private PooledObject CreatePooledObject()
        {
            var prefab = Instantiate(_prefab);
            prefab.transform.SetParent(this.transform);
            var po = new PooledObject(prefab, _prefab);
            po.Die();
            return po;
        }

        public PooledObject GetObjectPool()
        {
            if (pool.Count == 0)
            {
                pool.Push(CreatePooledObject());
            }
            var pooledObject = pool.Pop();
            return pooledObject;
        }

        public void KillPooledObject(PooledObject po)
        {
            if(po == null) return;
            po.Die();
            pool.Push(po);
        }

        public void InitPool(GameObject prefab, int count)
        {
            _prefab = prefab;
            for (int i = 0; i < count; i++)
            {
                pool.Push(CreatePooledObject());
            }
        }
    }
}
