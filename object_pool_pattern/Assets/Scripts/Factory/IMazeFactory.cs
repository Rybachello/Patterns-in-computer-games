using UnityEngine;

namespace Assets.Scripts.Factory
{
    public interface IMazeFactory
    {
        Maze CreateMaze();
        Room CreateRoom1();
        Room CreateRoom2();
        Portal CreatePortal(Room room1, Vector3 position);
    }

    public class NullMazeFactory : IMazeFactory
    {
        public Maze CreateMaze()
        {
            return null;
        }

        public Room CreateRoom1()
        {
            return null;
        }

        public Room CreateRoom2()
        {
            return null;
        }

        public Portal CreatePortal(Room room1, Vector3 position)
        {
            return null;
        }
    }
}