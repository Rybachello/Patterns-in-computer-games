using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputRecorder : MonoBehaviour
    {
        private RecorderState _recorderState; //current recorded state

        private readonly Queue<PlayBackCommand> _commands = new Queue<PlayBackCommand>();
        [SerializeField]private float _timeStamp;

        private readonly RecorderIdleState _recorderIdleState = new RecorderIdleState();
        private readonly RecorderPlayState _recorderPlayState = new RecorderPlayState();
        private readonly RecorderRecordState _recorderRecordState = new RecorderRecordState();

        private static InputRecorder _instance; //singleton
        private InputHandler _inputHandler; //reference for InputHandler

        void Awake()
        {
            Init();
        }

        void Init()
        {
            _instance = this;
            _inputHandler = InputHandler.Instance;
            _recorderState = _recorderIdleState;//by default
            _timeStamp = 0;
        }

        void OnDestoroy()
        {
            _instance = null;
        }

        public void EnterState()
        {
            _recorderState.Enter();
        }
        
        public void Play() //start/stop playing
        {
            Debug.Log("play command is pressed");
           _recorderState = _recorderState.Play();
            Debug.Log("In:" + _recorderState.GetType());
            EnterState();
           
        }
        
        public void Record() //starts/stops playback
        {
            Debug.Log("record command is pressed");
            _recorderState = _recorderState.Record();
            Debug.Log("In:" + _recorderState.GetType());
            EnterState();
        }

        public void PlayBack()
        {
            print("Start playbacking");
            _timeStamp = Time.time;
            StartCoroutine(Playback());
        }

        private IEnumerator Playback()
        {
            while (true)
            {
                if (_commands.Count != 0)
                {
                    //print(Time.time + "::" + _commands.Peek().Time +"::" + _timeStamp);
                    if ((Time.time - _timeStamp) > _commands.Peek().Time)
                    {
                        Debug.Log("Commnad Time::" + _commands.Peek().Time);
                        _commands.Dequeue().Execute(_inputHandler.CurrentActor);
                    }
                }
                else
                {
                    Debug.Log("PlayBack finished");
                    _timeStamp = 0;
                    yield break;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void Add(Command command)
        {
            if (command is RecordCommand) return; // if it is recordCommand 
            _recorderState.Add(command);
        }

        public void StartRecording()
        {
            _commands.Clear(); //clean the queue
            _timeStamp = Time.time; //set the time
        }


        public static InputRecorder Instance
        {
            get
            {
                if (_instance) return _instance;
                var dataGameOb = GameObject.Find("InputManager");
                if (dataGameOb)
                {
                    _instance = dataGameOb.GetComponent<InputRecorder>();
                    return _instance;
                }
                var gameob = new GameObject {name = "InputManager"};
                gameob.AddComponent<InputHandler>();
                _instance = gameob.AddComponent<InputRecorder>();
                return _instance;
            }
        }

        public Queue<PlayBackCommand> Commands
        {
            get { return _commands; }
        }

        public RecorderState RecorderIdleState
        {
            get { return _recorderIdleState; }
        }

        public RecorderState RecorderPlayState
        {
            get { return _recorderPlayState; }
        }

        public RecorderState RecorderRecordState
        {
            get { return _recorderRecordState; }
        }

        public float TimeStamp
        {
            get { return _timeStamp; }
        }
    }
}





