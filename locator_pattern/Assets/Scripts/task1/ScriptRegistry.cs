using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.task1
{
    public class ScriptRegistry
    {
        private static readonly List<MonoBehaviour> Service = new List<MonoBehaviour>();

        public static void Provide<T>(T test) where T : MonoBehaviour
        {
            if (!Service.OfType<T>().Any())
                Service.Add(test);
            else
            {
                Debug.Log("this script already exist");
                Service.RemoveAll(item => item.GetType() == typeof(T));
                Debug.Log("add ");
                Service.Add(test);
            }
        }
        
        public static MonoBehaviour GetScript<T>() where T : MonoBehaviour
        {
            if (Service.OfType<T>().Any())
                return Service.OfType<T>().First();
            return new NullMonoBehaviour();
        }
    }
    public class NullMonoBehaviour : MonoBehaviour { }

}

    
