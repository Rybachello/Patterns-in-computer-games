using Assets.Scripts;
using Assets.Scripts.Factory;
using Assets.Scripts.ObjectPool;
using UnityEngine;

public class Locator
{
    private static IStats _statsService = new NullStats();
    private static IMazeFactory _mazeFactoryService = new NullMazeFactory();
    private static IObjectPool _objectPoolService = new NullObjectPool();

    public static void Provide(IStats stats)
    {
        _statsService = stats ?? new NullStats();
    }

    public static void Provide(IMazeFactory mazeFactory)
    {
        _mazeFactoryService = mazeFactory ?? new NullMazeFactory();
    }

    public static void Provide(IObjectPool objectPool)
    {
        _objectPoolService = objectPool ?? new NullObjectPool();
    }

    #region getters

    public static IStats GetStats
    {
        get { return _statsService; }
    }

    public static IMazeFactory GetMazeFactory
    {
        get { return _mazeFactoryService; }
    }

    public static IObjectPool GetObjectPool
    {
        get { return _objectPoolService; }
    }

    #endregion
}
//interfaces 
public interface IStats { }
//null classes 
public class NullStats : IStats { }