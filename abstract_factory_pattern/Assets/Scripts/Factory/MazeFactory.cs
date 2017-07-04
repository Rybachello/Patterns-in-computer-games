using Assets.Scripts.ObjectPool;
using UnityEngine;

namespace Assets.Scripts.Factory
{
    public class MazeFactory : MonoBehaviour, IMazeFactory
    {
        [SerializeField] private GameObject _ground;
        [SerializeField] private GameObject _riverRight;
        [SerializeField] private GameObject _riverForward;
        [SerializeField] private GameObject _portal;
       

        void Awake()
        {
            Locator.Provide(this);
            InitPrefabs();
            DontDestroyOnLoad(this);
        }

        private void InitPrefabs()
        {
            _ground = Resources.Load("Prefabs/Ground/Plate_Grass_Dirt_01") as GameObject;
            _riverRight = Resources.Load("Prefabs/Ground/Plate_River_Dirt_Right_01") as GameObject;
            _riverForward = Resources.Load("Prefabs/Ground/Plate_River_Dirt_Forward_01") as GameObject;
            _portal = Resources.Load("Prefabs/Ground/Plate_Grass_Dirt_Portal") as GameObject;
        }

        public Maze CreateMaze()
        {
            Maze maze = new Maze();
            //some cool stuff here
            return maze;
        }

        public Room CreateRoom1()
        {
            Room room = new Room(1);
            //fill room
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 3; j++)
                {
                    
                    MapTile tile = CreateGround1(i, j);
                    room.AddTile(tile);
                }
            }

            return room;
        }

        public Room CreateRoom2()
        {
            Room room = new Room(2);
            //fill room
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    MapTile tile = CreateGround2(i, j);
                    room.AddTile(tile);
                }
            }
            return room;
        }

        public Portal CreatePortal(Room room, Vector3 position)
        {
            Portal portal = new Portal(room, this._portal, position);
            room.AddTile(portal);
            return portal;
        }

        private MapTile CreateGround1(int i, int j)
        {

            MapTile tile = new Ground(j == 1 ? _riverRight : _ground, new Vector3(i, 0, j));
            return tile;
        }

        private MapTile CreateGround2(int i, int j)
        {
            MapTile tile = new Ground(i == 1 ? _riverForward : _ground, new Vector3(i, 0, j));
            return tile;
        }
    }
}