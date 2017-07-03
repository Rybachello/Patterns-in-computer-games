using UnityEngine;

namespace Assets.Scripts.ObjectPool
{
    public class PooledObject 
    {

        public GameObject Go;
        public GameObject Prefab;

        private readonly Vector3 _enablePosition = new Vector3(1000,1000,1000);

        public PooledObject(GameObject gameObject, GameObject prefab)
        {
            Go = gameObject;
            Prefab = prefab;
        }

        public void Revive()
        { 
            Go.gameObject.SetActive(true);
        }

        public void Die()
        {
            Go.transform.position = _enablePosition;
            Go.gameObject.SetActive(false);
        }

    }
}
