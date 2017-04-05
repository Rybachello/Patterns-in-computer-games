using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class UIStepCounter : MonoBehaviour, global::Observer {
        private readonly Dictionary<TileBasedMovement, int> _actorSteps = new Dictionary<TileBasedMovement, int>();
        private readonly Dictionary<TileBasedMovement, StepCard> _actorCards = new Dictionary<TileBasedMovement, StepCard>();

        [SerializeField]
        StepCard _cardPrefab;
        [SerializeField]
        Transform _container;

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
            if (mover == null)
            {
                return;
            }
            if (!_actorSteps.ContainsKey(mover))
            {
                _actorSteps.Add(mover, 0);
                var card = Instantiate(_cardPrefab, _container);
                card.SetName(mover.name);
                card.SetSteps(_actorSteps[mover]);
                _actorCards.Add(mover, card);
            }
            _actorCards[mover].SetSteps(++_actorSteps[mover]);
        }
    }
}
