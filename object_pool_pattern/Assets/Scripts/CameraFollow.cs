using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    protected Transform target;
    protected Vector3 offset;
    
    public void Follow(Transform target)
    {
        this.target = target;
    }

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

	void Update () {
        transform.position = target.transform.position + offset;	
	}
}
