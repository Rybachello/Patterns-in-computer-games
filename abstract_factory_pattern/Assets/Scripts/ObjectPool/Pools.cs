using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ObjectPool
{
    public class Pools : MonoBehaviour, IObjectPool
    {

        private readonly Dictionary<GameObject, Pool> _pools = new Dictionary<GameObject, Pool>();
        [SerializeField] private GameObject _ground;
        [SerializeField] private GameObject _riverRight;
        [SerializeField] private GameObject _riverForward;
        [SerializeField] private GameObject _portal;

        void Awake()
        {
            Locator.Provide(this);
            InitPrefabs();
            InitPools();
            DontDestroyOnLoad(this.gameObject);
        }

        private void InitPrefabs()
        {
            _ground = Resources.Load("Prefabs/Ground/Plate_Grass_Dirt_01") as GameObject;
            _riverRight = Resources.Load("Prefabs/Ground/Plate_River_Dirt_Right_01") as GameObject;
            _riverForward = Resources.Load("Prefabs/Ground/Plate_River_Dirt_Forward_01") as GameObject;
            _portal = Resources.Load("Prefabs/Ground/Plate_Grass_Dirt_Portal") as GameObject;
        }

        private void InitPools()
        {
            var groundPool = transform.FindChild("[GroundPool]").GetComponent<Pool>();
            groundPool.InitPool(_ground, 1);
            _pools.Add(_ground, groundPool);

            var portalPool = transform.FindChild("[PortalPool]").GetComponent<Pool>();
            portalPool.InitPool(_portal, 1);
            _pools.Add(_portal, portalPool);

            var riverForwardPool = transform.FindChild("[RiverForwardPool]").GetComponent<Pool>();
            riverForwardPool.InitPool(_riverForward, 1);
            _pools.Add(_riverForward, riverForwardPool);

            var riverRightPool = transform.FindChild("[RiverRightPool]").GetComponent<Pool>();
            riverRightPool.InitPool(_riverRight, 1);
            _pools.Add(_riverRight, riverRightPool);

        }

        public PooledObject GetPooledObject(GameObject prefab)
        {

            var pool = (from p in _pools where p.Key == prefab select p.Value).FirstOrDefault();
            if (pool != null) return pool.GetObjectPool();
            Debug.Log("Cannot find pool");
            return null;
        }

        public void KillPooledObject(PooledObject po)
        {
            foreach (var pool in _pools)
            {
                if (pool.Key == po.Prefab)
                {
                    pool.Value.KillPooledObject(po);
                }
            }
        }
    }
}
