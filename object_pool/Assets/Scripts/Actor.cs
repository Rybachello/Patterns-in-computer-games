using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{

    TileBasedMovement movementStrategy;

    void Start()
    {
        movementStrategy = GetComponent<TileBasedMovement>();
    }

    public void SetPosition (Vector3 pos)
    {
        movementStrategy.SetPosition(pos);
    }
}
