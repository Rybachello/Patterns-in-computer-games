using UnityEngine;
using System.Collections;

public class TileEvent : MonoBehaviour
{
    public MapTile tile;

    public void OnTriggerEnter(Collider other)
    {
        tile.Enter(other.GetComponentInParent<Actor>());
    }
}
