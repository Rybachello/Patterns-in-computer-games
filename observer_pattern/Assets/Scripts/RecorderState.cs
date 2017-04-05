using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts
{
    public abstract class RecorderState
    {

        public virtual void Enter()
        {
        }

        public virtual RecorderState Play()
        {
            return InputRecorder.Instance.RecorderIdleState;
        }

        public virtual RecorderState Record()
        {
            return InputRecorder.Instance.RecorderIdleState;
        }

        public virtual void Add(Command command)
        {
            
        }
    }
}

public class RecorderIdleState : RecorderState
{
    
    public override RecorderState Play()
    {
        return InputRecorder.Instance.RecorderPlayState; //switch to play state
    }

    public override RecorderState Record()
    {
        return InputRecorder.Instance.RecorderRecordState;//switch to record state
    }
}

public class RecorderPlayState : RecorderState
{
    public override void Enter()
    {
        InputRecorder.Instance.PlayBack();
    }

    public override RecorderState Play()
    {
       return InputRecorder.Instance.RecorderIdleState;
    }

    public override RecorderState Record()
    {
        return InputRecorder.Instance.RecorderIdleState;//switch to idle state
    }
}

public class RecorderRecordState : RecorderState
{
    public override void Enter()
    {
        InputRecorder.Instance.StartRecording();
    }

    public override RecorderState Play()
    {
        return InputRecorder.Instance.RecorderPlayState;//switch to play state
    }

    public override RecorderState Record()
    {
        return InputRecorder.Instance.RecorderIdleState;
    }

    public override void Add(Command command)
    {
        if (command is RecordCommand) return; // if it is recordCommand 
        var playBackCommand = new PlayBackCommand(command, Time.time - InputRecorder.Instance.TimeStamp);
        InputRecorder.Instance.Commands.Enqueue(playBackCommand);
        Debug.Log("Added:: " + playBackCommand.GetType() + "time :" + playBackCommand.Time);
    }
}
