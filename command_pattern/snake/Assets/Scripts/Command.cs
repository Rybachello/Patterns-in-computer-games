using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Command
    {
        public abstract void Execute(TileBasedMovement target);

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
            target.Walk(target.transform.position+_direction);
            //target.MoveTail(target.transform.position);
        }
    }

    public class SwitchCommand : Command
    {
        public override void Execute(TileBasedMovement currentActor)
        {
            var inputHandler = GameObject.FindObjectOfType<InputHandler>();
            if(inputHandler)
                inputHandler.Switch(currentActor);
        }
    }
    
}
