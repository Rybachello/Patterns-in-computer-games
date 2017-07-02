using System;
using UnityEngine;

public abstract class Command {

    public abstract void Execute(object receiver);

}

public class MoveCommand : Command
{
    protected Vector3 direction; 

    public MoveCommand(Vector3 dir)
    {
        direction = dir;
    }

    public override void Execute(object receiver)
    {
        if ((receiver as TileBasedMovement) == null)
            throw new InvalidCastException();
        (receiver as TileBasedMovement).Walk(direction);
    }
}


    /*
    Todo:
        Add an abstract Execute method with an argument for receiver.
        Create a MoveCommand class, which executes Walk method in TileBasedMovement component.

    Explanation:
        While classical version of command pattern ties a receiver to the
        command in the constructor, which is too limiting for us in this situation.

        In our case the commands are stateless, so it will be preferential for use to just reuse
        the same command every time we want to execute it, instead of instantiating another exact
        copy of it. Although we also want to send these commands to different actors, so we 
        will need to move the receiver reference from constructor to the execute method. 

        Read the "Directions for Actors" section from Command chapter in Game Programming Patterns
        if you would like to know more.
    */

