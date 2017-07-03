using UnityEngine;

namespace Assets.Scripts.task1
{
    public class Result : MonoBehaviour {

        // Use this for initialization
        void Start ()
        {
            ScriptRegistry.Provide<Test>(new Test());
            print(ScriptRegistry.GetScript<Test1>());
            print(ScriptRegistry.GetScript<Test>());
        }
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
