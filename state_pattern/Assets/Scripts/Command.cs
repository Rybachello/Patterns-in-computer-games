﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Command
    {
        public abstract void Execute(TileBasedMovement target);
    }

    public class MoveCommand : Command
    {
        private readonly Vector3 _direction;

        public MoveCommand(Vector3 direction)
        {
            _direction = direction;
        }

        public override void Execute(TileBasedMovement target)
        {
            target.Walk(target.transform.position + _direction);
        }
    }

    public class SwitchCommand : Command
    {
        public override void Execute(TileBasedMovement currentActor)
        {
            InputHandler.Instance.Switch(currentActor);
        }
    }
    public class PlayBackCommand : Command
    {
        private readonly Command _command;
        private readonly float _time;

        public PlayBackCommand(Command command, float time)
        {
            _command = command;
            _time = time;
        }

        public Command Command
        {
            get
            {
                return _command;
            }
        }

        public float Time
        {
            get
            {
                return _time;
            }
        }

        public override void Execute(TileBasedMovement target)
        {
            _command.Execute(target);
        }
    }

    public abstract class RecordCommand : Command
    {
    }
    
    public class PlayCommand : RecordCommand
    {
        public override void Execute(TileBasedMovement target)
        {
            InputRecorder.Instance.Play();
        }
    }
    
    public class Record : RecordCommand
    {
        public override void Execute(TileBasedMovement target)
        {
            InputRecorder.Instance.Record();
        }
    }

}
