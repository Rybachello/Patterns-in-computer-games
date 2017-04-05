using UnityEngine;

namespace Assets.Scripts
{
    public class MouseInputHandler : MonoBehaviour
    {
        private Clonable _instanceToCreate = null;

        void Update()
        {
            if (UpdateLeftMouseClick()) return;
            if (UpdateRightMouseClick()) return;
        }

        private bool UpdateLeftMouseClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                print(" Left mouse clicked");
                var objectToCreate = GetTargetObject();
                if (objectToCreate)
                {
                    _instanceToCreate = objectToCreate;
                    print("selected to clone:: " + _instanceToCreate);
                }
                return true;
            }
            return false;
        }

        private Clonable GetTargetObject()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            const int maxDistance = 1000;
            var layerMask = 1 << LayerMask.NameToLayer("Item"); //get prefab searching by layer target //todo: can remove it and remove from the raycast paramater
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, maxDistance, layerMask)) return null;
            return hit.transform.GetComponent<Clonable>() ? hit.transform.GetComponent<Clonable>() : null;
        }

        private bool UpdateRightMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 positionToInstantiate = Vector3.zero;
                if (Physics.Raycast(ray, out hit)) //todo: make only for flat? adding layer
                {
                    positionToInstantiate = hit.point;
                }
                if (_instanceToCreate)
                    _instanceToCreate.CreateInstance(positionToInstantiate);
                return true;
            }
            return false;
        }
    }
}
