using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        private List<TileBasedMovement> _allActors;
        private TileBasedMovement _currentActor;

        private readonly List<KeyCode> _keysDown = new List<KeyCode>();
        private KeyCode _currentKey;

        private Event _current = new Event();

        private readonly Dictionary<KeyCode, Command> _keymap = new Dictionary<KeyCode, Command>();

        private static InputHandler _instance; // singleton
        
        void Awake()
        {
            Init();
        }
        
        void OnDestroy()
        {
            _instance = null;
        }

        private void Init()
        {
            //get reference
            _instance = this;

            InitActors();
            InitKeyMap();
        }

        private void InitActors()
        {
            _allActors = FindObjectsOfType<TileBasedMovement>().Where(x => x.tag == "Actor").ToList();
            _currentActor = _allActors.First();
            Camera.main.GetComponent<CameraFollow>().Follow(_currentActor.transform);
        }

        private void InitKeyMap()
        {
            _keymap.Add(KeyCode.W, new MoveCommand(Vector3.forward));
            _keymap.Add(KeyCode.S, new MoveCommand(Vector3.back));
            _keymap.Add(KeyCode.A, new MoveCommand(Vector3.left));
            _keymap.Add(KeyCode.D, new MoveCommand(Vector3.right));

            _keymap.Add(KeyCode.Tab, new SwitchCommand());

            _keymap.Add(KeyCode.P, new PlayCommand());
            _keymap.Add(KeyCode.R, new Record());
        }

        void Update()
        {
            PopEvent();
            _currentKey = ReadKeyDown();

            if (_currentKey == KeyCode.None) return;

            if (_keymap.ContainsKey(_currentKey))
            {
                var command = _keymap.FirstOrDefault(x => x.Key == _currentKey).Value;
                if (_currentActor)
                {
                    command.Execute(_currentActor); //execute commnad 
                    InputRecorder.Instance.Add(command); //add command 
                    
                }
            }
        }

        #region Executing Commands

        public void Switch(TileBasedMovement currentActor)
        {
            _currentActor = GetNextActor();
            Camera.main.GetComponent<CameraFollow>().Follow(_currentActor.transform);
        }

        private TileBasedMovement GetNextActor()
        {
            var nextActor = _allActors[0]; //as default

            for (var i = 0; i < _allActors.Count; i++)
            {
                if (_currentActor == _allActors[i] && i + 1 != _allActors.Count)
                {
                    return _allActors[i + 1];
                }
            }
            return nextActor;
        }
        #endregion

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


        public static InputHandler Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                else
                {
                    var dataGameOb = GameObject.Find("InputManager");
                    if (dataGameOb)
                    {
                        _instance = dataGameOb.GetComponent<InputHandler>();
                        return _instance;
                    }
                    var gameob = new GameObject {name = "InputManager"};
                    _instance = gameob.AddComponent<InputHandler>();
                    return _instance;
                }
            }
        }

        public TileBasedMovement CurrentActor
        {
            get { return _currentActor; }
        }
    }
}

