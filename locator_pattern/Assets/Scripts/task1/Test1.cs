using UnityEngine;

namespace Assets.Scripts.task1
{
    public class Test1 : MonoBehaviour {

        // Use this for initialization
        void Start()
        {
            ScriptRegistry.Provide<Test1>(this);
            print(ScriptRegistry.GetScript<Test1>());
            this.gameObject.name = "new one";
            ScriptRegistry.Provide<Test1>(this);
            print(ScriptRegistry.GetScript<Test1>());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
