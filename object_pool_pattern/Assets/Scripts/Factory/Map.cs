using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ObjectPool;

public class Maze
{
    public List<Room> rooms = new List<Room>();

    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    public void Load()
    {
        if(rooms.Count == 0)
        {
            return;
        }
        rooms[0].Load();
    }
}

public abstract class MapSite
{
    public virtual void Enter(Actor actor) { }
    public virtual void Load() { }
    public virtual void Unload() { }
}

public abstract class MapTile : MapSite
{

    protected PooledObject pooledObject;
    protected Vector3 position;
    protected GameObject prefab;
    GameObject go;

    protected const float tileSize = 3f;

    protected MapTile(GameObject pref, Vector3 pos)
    {
        this.position = pos;
        this.prefab = pref;
    }

    public override void Enter(Actor actor)
    {
        Debug.Log(string.Format("{0} entered tile {1}", actor.name, position));
    }
    
    public override void Load()
    {
        pooledObject =  Locator.GetObjectPool.GetPooledObject(prefab);
        pooledObject.Revive();
        this.go = pooledObject.Go;
        go.transform.position = position * tileSize;
        go.GetComponent<TileEvent>().tile = this;
    }

    public override void Unload()
    {
        Locator.GetObjectPool.KillPooledObject(pooledObject);
    }

 }

public class Room : MapSite
{
    public int nr;
    public List<MapTile> tiles = new List<MapTile>();

    public Room(int nr)
    {
        this.nr = nr;
    }
    
    public override void Load()
    {
        Debug.Log("Loading room " + nr);
        var t = Time.realtimeSinceStartup;
        foreach(var tile in tiles)
        {
            tile.Load();
        }
        var time = Time.realtimeSinceStartup;
        Debug.Log("Loading time ::::" + (time - t));
    }

    public void AddTile(MapTile tile)
    {
        tiles.Add(tile);
    }

    public override void Unload()
    {
        foreach (var tile in tiles)
        {
            tile.Unload();
        }
    }
}

public class Ground : MapTile
{
    public Ground(GameObject pref, Vector3 pos) : base (pref, pos)
    {
    }
}

public class Portal : MapTile
{
    Room room;
    Portal exit;

    bool isOpen = false;

    public Portal(Room room, GameObject pref, Vector3 pos) : base (pref, pos)
    {
        this.room = room;
    }

    public void SetExit(Portal exit)
    {
        this.exit = exit;
        isOpen = true;
    }

    public override void Enter(Actor actor)
    {
        Debug.Log(string.Format("{0} entered portal {1}", actor.name, position));
        if (room != exit.room)
        {
            room.Unload();
            exit.room.Load();
        }
        Vector3 cornerOffset = new Vector3(-2f, 0, -3f);
        actor.SetPosition(exit.position * tileSize + cornerOffset);
    }
}