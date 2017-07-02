using UnityEngine;
using System.Collections;


public class TileBasedMovement : MonoBehaviour
{
    [SerializeField]
    protected float stepSize = 1f;
    [SerializeField]
    protected float stepTime = 0.3f;
    [SerializeField]
    protected TileBasedMovement tail;

    protected bool isMoving;
    protected MoveCommand lastCommand;

    private Coroutine coroutine;

    /// <summary>
    /// Walks to adjacent tile based on direction
    /// </summary>
    /// <param name="direction"></param>
    public void Walk(Vector3 direction)
    {
        if (!isMoving)
        {
            if (lastCommand != null && tail != null)
                lastCommand.Execute(tail);
            coroutine = StartCoroutine(LinearMove(transform.position, transform.position + direction * stepSize, 0.3f));
            lastCommand = new MoveCommand(direction);
        }
    }

    protected IEnumerator LinearMove(Vector3 from, Vector3 to, float duration)
    {
        if (duration < float.Epsilon)
        {
            transform.position = to;
            yield break;
        }

        isMoving = true;
        float agregate = 0;
        while (agregate < 1f)
        {
            agregate += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(from, to, agregate);
            yield return null;
        }
        isMoving = false;
    }

    public void SetPosition(Vector3 pos)
    {
        StopCoroutine(coroutine);
        isMoving = false;
        transform.position = pos;
    }
}
