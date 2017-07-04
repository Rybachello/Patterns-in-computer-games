using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ObjectPool
{
    public interface IObjectPool
    {
        PooledObject GetPooledObject(GameObject prefab);

        void KillPooledObject(PooledObject po);

    }

    public class NullObjectPool : IObjectPool
    {
        public PooledObject GetPooledObject(GameObject prefab)
        {
            return null;
        }

        public void KillPooledObject(PooledObject po)
        {

        }
    }
}