using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileBasedMovement : MonoBehaviour
    {
        [SerializeField]
        protected float StepSize = 1f;
        [SerializeField]
        protected float StepTime = 0.3f;

        protected bool IsMoving;
    
        public void Walk(Vector3 direction)
        {
            if (!IsMoving)
            {
                StartCoroutine(LinearMove(transform.position, transform.position + direction * StepSize, 0.3f));
            }
        }

        protected IEnumerator LinearMove(Vector3 from, Vector3 to, float duration)
        {
            if (duration < float.Epsilon)
            {
                transform.position = to;
                yield break;
            }

            IsMoving = true;
            float agregate = 0;
            while (agregate < 1f)
            {
                agregate += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(from, to, agregate);
                yield return null;
            }
            IsMoving = false;
        }
    }
}
