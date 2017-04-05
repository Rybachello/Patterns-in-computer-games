using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Observer
{
    public class ConsoleStepCounter : MonoBehaviour, global::Observer {
        private readonly Dictionary<TileBasedMovement, int> _actorSteps = new Dictionary<TileBasedMovement, int>();

        void Start()
        {
            Subject.AddObserver(this);
        }

        void OnDestroy()
        {
            Subject.RemoveObserver(this);
        }

        public void SubjectUpdate(object sender)
        {
            TileBasedMovement mover = sender as TileBasedMovement;
            if(mover == null)
            {
                return;
            }

            if(!_actorSteps.ContainsKey(mover))
            {
                _actorSteps.Add(mover, 0);
            }

            _actorSteps[mover]++;
            Debug.LogFormat("{0} has made step #{1}",mover.name,_actorSteps[mover]);
            }
      }
 }

    
