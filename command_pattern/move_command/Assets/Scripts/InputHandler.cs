using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class reads keyboard through the unity Events normally associated with the now deprecated
    /// old Unity OnGUI method for drawing GUI. May cause abnormal behavior if used together
    /// with the pre 4.3 GUI system, due to consuming the input events.
    /// </summary>

    public class InputHandler : MonoBehaviour
    {

        [SerializeField]
        private TileBasedMovement _currentActor;

        private KeyCode _currentKey;
        private readonly List<KeyCode> _keysDown = new List<KeyCode>();
        private List<TileBasedMovement> allActors;

        private Event _current = new Event();

        private readonly Dictionary<KeyCode, Command> _keymap = new Dictionary<KeyCode, Command>();

        void Awake()
        {
            allActors = FindObjectsOfType<TileBasedMovement>().ToList();
            InitKeyMap(); //add commands

            if (_currentActor)
                _currentActor = allActors.First();
        }

        private void InitKeyMap()
        {
            _keymap.Add(KeyCode.W, new MoveCommand(Vector3.forward));
            _keymap.Add(KeyCode.S, new MoveCommand(Vector3.back));
            _keymap.Add(KeyCode.A, new MoveCommand(Vector3.left));
            _keymap.Add(KeyCode.D, new MoveCommand(Vector3.right));
        }

        void Update()
        {
            //Get the keycode of key pressed this frame, return if no key is pressed
            PopEvent();
            _currentKey = ReadKeyDown();
            //currentKey = ReadKey(); //ReadKey() reads keycodes until the button is released
            if (_currentKey == KeyCode.None) return;

            if (_keymap.ContainsKey(_currentKey))
            {
                var command = _keymap.FirstOrDefault(x => x.Key == _currentKey).Value;
                if(_currentActor)
                    command.Execute(_currentActor);
            }
        }

        #region Helper code for the exercises

        /// <summary>
        /// Pop the OnGUI input event and create local reference for it
        /// </summary>
        protected void PopEvent()
        {
            _current = new Event();
            Event.PopEvent(_current);
        }
        /// <summary>
        /// Returns the keycode of a keyboard button down event
        /// </summary>
        /// <returns></returns>
        protected KeyCode ReadKeyDown()
        {
            if (_current.type == EventType.keyDown && !_keysDown.Contains(_current.keyCode))
            {
                _keysDown.Add(_current.keyCode);
                return _current.keyCode;
            }
            else if (_current.type == EventType.keyUp)
            {
                _keysDown.Remove(_current.keyCode);
            }
            return KeyCode.None;
        }
        /// <summary>
        /// Returns the keycode of last held keyboard button
        /// </summary>
        protected KeyCode ReadKey()
        {
            return _current.keyCode;
        }

        /// <summary>
        /// Enters rebind state and tries to rebind Command to next currentKey that is not KeyCode.None
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        IEnumerator EnterRebindState(Command cmd)
        {
            Debug.Log("Started rebind coroutine");
            while (_currentKey == KeyCode.None)
            {
                yield return null;
            }
            Debug.Log("Rebinding");
            RebindCommand(cmd);
        }


        /// <summary>
        /// <para>Binds a new KeyCode to a Command, if the KeyCode is unique.</para>
        /// <para>To rebind call EnterRebindState instead:</para>
        /// <para>StartCoroutine(EnterRebindState(new MoveCommand(Vector3.forward)));</para>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool RebindCommand(Command command)
        {
            if (_keymap.ContainsKey(_currentKey))
                return false;

            //Remove old instance of the command
            Debug.Log(_keymap.Count);
            var pair = _keymap.FirstOrDefault(kvp => kvp.Value.Equals(command));
            _keymap.Remove(pair.Key);

            _keymap.Add(_currentKey, command);
            Debug.Log("Command bound to " + _currentKey);
            return true;
        }
        #endregion
    }
}

