using UnityEngine;

namespace Assets.Scripts.Factory
{
    public class MazeGame : MonoBehaviour {

        protected Maze maze;

        protected IMazeFactory _MazeFactory;
      
        // Use this for initialization
        void Start ()
        {

            _MazeFactory = Locator.GetMazeFactory;
            CreateMaze(_MazeFactory);
        }

    
        public virtual void CreateMaze(IMazeFactory factory)
        {
            maze = factory.CreateMaze();

            Room room1 = factory.CreateRoom1();
            Room room2 = factory.CreateRoom2();

            Portal portal1 = factory.CreatePortal(room1, new Vector3(0, 0, 2f));
            Portal portal2 = factory.CreatePortal(room2, new Vector3(2f, 0, 0));

            portal1.SetExit(portal2);
            portal2.SetExit(portal1);

            maze.AddRoom(room1);
            maze.AddRoom(room2);

            maze.Load();
        }
    }
}
