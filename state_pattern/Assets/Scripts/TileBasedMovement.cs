using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileBasedMovement : MonoBehaviour
    {
        public TileBasedMovement Tail;

        protected float StepSize = 1f;
        protected float StepTime = 0.3f;
        protected bool IsMoving;

        public void Walk(Vector3 to)
        {
            if(!IsMoving)
            {
                if (Tail)
                    Tail.Walk(transform.position);
                StartCoroutine(LinearMove(transform.position, to * StepSize, 0.3f));
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
