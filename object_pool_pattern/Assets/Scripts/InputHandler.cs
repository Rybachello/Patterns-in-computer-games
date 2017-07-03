using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

/// <summary>
/// This class reads keyboard through the unity Events normally associated with the now deprecated
/// old Unity OnGUI method for drawing GUI. May cause abnormal behavior if used together
/// with the pre 4.3 GUI system, due to consuming the input events.
/// </summary>

public class InputHandler : MonoBehaviour
{

    [SerializeField]
    private TileBasedMovement currentActor;

    private KeyCode currentKey;
    private List<KeyCode> keysDown = new List<KeyCode>();
    private List<TileBasedMovement> allActors;

    private Event current = new Event();

    private IEnumerator<TileBasedMovement> iterator;

    private Dictionary<KeyCode, Command> keymap = new Dictionary<KeyCode, Command>();

    // Use this for initialization
    void Start()
    {
        allActors = new List<TileBasedMovement>();
        foreach(var go in GameObject.FindGameObjectsWithTag("Head"))
        {
            allActors.Add(go.GetComponent<TileBasedMovement>());
        }
        iterator = allActors.GetEnumerator();

        while(iterator.Current != currentActor)
        {
            iterator.MoveNext();
        }

        //TODO: Add commands to the dictionary
        /*
        
        ...
        */
        keymap.Add(KeyCode.W, new MoveCommand(Vector3.forward));
        keymap.Add(KeyCode.A, new MoveCommand(Vector3.left));
        keymap.Add(KeyCode.S, new MoveCommand(Vector3.back));
        keymap.Add(KeyCode.D, new MoveCommand(Vector3.right));

    }



    // Update is called once per frame
    void Update()
    {
        //Get the keycode of key pressed this frame, return if no key is pressed
        PopEvent();
        currentKey = ReadKeyDown();
        //currentKey = ReadKey(); //ReadKey() reads keycodes until the button is released
        if (currentKey == KeyCode.None) return;

        /**
        TODO: 
            If the currentKey is in the keymap then execute the command it is paired with.
        */
        if(keymap.ContainsKey(currentKey))
        {
            keymap[currentKey].Execute(currentActor);
        }

    }

    #region Helper code for the exercises

    /// <summary>
    /// Pop the OnGUI input event and create local reference for it
    /// </summary>
    protected void PopEvent()
    {
        current = new Event();
        Event.PopEvent(current);
    }
    /// <summary>
    /// Returns the keycode of a keyboard button down event
    /// </summary>
    /// <returns></returns>
    protected KeyCode ReadKeyDown()
    {
        if (current.type == EventType.keyDown && !keysDown.Contains(current.keyCode))
        {
            keysDown.Add(current.keyCode);
            return current.keyCode;
        }
        else if (current.type == EventType.keyUp)
        {
            keysDown.Remove(current.keyCode);
        }
        return KeyCode.None;
    }
    /// <summary>
    /// Returns the keycode of last held keyboard button
    /// </summary>
    protected KeyCode ReadKey()
    {
        return current.keyCode;
    }

    /// <summary>
    /// Enters rebind state and tries to rebind Command to next currentKey that is not KeyCode.None
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    IEnumerator EnterRebindState(Command cmd)
    {
        Debug.Log("Started rebind coroutine");
        while (currentKey == KeyCode.None)
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
        if (keymap.ContainsKey(currentKey))
            return false;

        //Remove old instance of the command
        Debug.Log(keymap.Count);
        var pair = keymap.FirstOrDefault(kvp => kvp.Value.Equals(command));
        keymap.Remove(pair.Key);

        keymap.Add(currentKey, command);
        Debug.Log("Command bound to " + currentKey);
        return true;
    }

    
    #endregion
}

