using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour {

        [SerializeField]
        protected Transform Target;
        protected Vector3 Offset;
    
        public void Follow(Transform target)
        {
            Target = target;
        }

        private void Start()
        {
            Offset = transform.position - Target.transform.position;
        }

        private void Update () {
            transform.position = Target.transform.position + Offset;	
        }
    }
}
