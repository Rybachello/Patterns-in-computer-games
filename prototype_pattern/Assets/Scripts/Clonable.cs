using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Clonable : MonoBehaviour {

        public void CreateInstance( Vector3 positionToInstantiate)
        {
            Instantiate(this.gameObject, positionToInstantiate, this.gameObject.transform.rotation);
        }
    }
}
