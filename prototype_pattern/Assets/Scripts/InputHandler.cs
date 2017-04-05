using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        private TileBasedMovement _currentActor;
        private float _time;

        private KeyCode _currentKey;
        private readonly List<KeyCode> _keysDown = new List<KeyCode>();

        private List<TileBasedMovement> _allActors;

        private Event _current = new Event();

        private readonly Dictionary<KeyCode, Command> _keymap = new Dictionary<KeyCode, Command>();
        private readonly Queue<PlayBackCommand> _commands = new Queue<PlayBackCommand>();

        private bool _isRecording = false;
        private bool _isPlaying = false;

        private static InputHandler _instance; // singleton
        
        void Awake()
        {

            InitActors();
            InitKeyMap(); 
        }

        void OnDestroy()
        {
            _instance = null;
        }

        private void InitActors()
        {
            _instance = this;

            _allActors = FindObjectsOfType<TileBasedMovement>().Where(x => x.tag == "Head").ToList();
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
            //todo: move this to the method
            if ((_isRecording && !_isPlaying) || (_isPlaying && !_isRecording))
            {
                _time += Time.deltaTime;
            }
            
            PopEvent();
            _currentKey = ReadKeyDown();

            if (_currentKey == KeyCode.None) return;

            if (_keymap.ContainsKey(_currentKey))
            {
                var command = _keymap.FirstOrDefault(x => x.Key == _currentKey).Value;
                if (_currentActor)
                {
                    command.Execute(_currentActor);
                    if (_isRecording & !(command is RecordCommand))
                    {
                        AddRecordedCommand(command);
                    }
                }
            }
        }

        private void AddRecordedCommand(Command command)
        {
            var playBackCommand = new PlayBackCommand(command, _time);
            _commands.Enqueue(playBackCommand);
            Debug.Log("Added:: " + command + "time :" + _time);
        }

        private void ResetRecordedCommands(bool reset)
        {
            if (reset)
            {
                _commands.Clear();
                Debug.Log("Reset");
            }
        }

        public IEnumerator Playback()
        {
            while (true)
            {
                if (!_isPlaying)
                    yield break;
                if (_commands.Count!=0 )
                {
                    if (_commands.Peek().Time < _time)
                    {
                        Debug.Log("Commnad Time::" + _commands.Peek().Time);
                        _commands.Dequeue().Execute(_currentActor);
                    }
                }
                else
                {
                    Debug.Log("PlayBack finished");
                    _isPlaying = false;
                    _time = 0;
                    yield break;
                }
                
                yield return new WaitForSeconds(1f);
            }
        }

        #region Executing Commands

        public void Switch(TileBasedMovement currentActor)
        {
            _currentActor = GetNextActor();
            Camera.main.GetComponent<CameraFollow>().Follow(_currentActor.transform);
        }
       
        public void Play()
        {
            _isPlaying = !_isPlaying;
            if (_isPlaying)
            {
                _time = 0;
            }
            print((_isPlaying ? "Start" : "Stop") + " playing");
            StartCoroutine(Playback());
        }
        

        public void Record()
        {
            _isRecording = !_isRecording;
            ResetRecordedCommands(_isRecording);
            print((_isRecording ? "Start" : "Stop") + " recording");
        }
        /*
        //public void Undo(TileBasedMovement currentActor)
        //{
        //    currentActor.Walk(currentActor.PrevPos);
        //}
        */
        #endregion

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
                    var dataGameOb = GameObject.Find("data");
                    if (dataGameOb)
                    {
                        _instance = dataGameOb.GetComponent<InputHandler>();
                        return _instance;
                    }
                    var gameob = new GameObject();
                    gameob.name = "InputHandler";
                    _instance = gameob.AddComponent<InputHandler>();
                    return _instance;
                }
            }
        }
    }
}

