using UnityEngine;

namespace Assets.Scripts.task2
{
    public class Locator
    {
        private static IStats _statsService = new NullStats();
        private static IMapFactory _mapFactoryService = new NullMapFactory();
        private static IObjectPool _objectPoolService = new NullObjectPool();

        public static void Provide(IStats stats)
        {
            _statsService = stats ?? new NullStats();
        }

        public static void Provide(IMapFactory mapFactory)
        {
            _mapFactoryService = mapFactory ?? new NullMapFactory();
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
        public static IMapFactory GetMapFactory
        {
            get { return _mapFactoryService; }
        }
        public static IObjectPool GetObjectPool
        {
            get { return _objectPoolService; }
        }
        #endregion
    }
    //interfaces 
    public interface IStats { }
    public interface IMapFactory { }
    public interface IObjectPool { }
    //null classes 
    public class NullStats : IStats { }
    public class NullMapFactory : IMapFactory { }
    public class NullObjectPool : IObjectPool { }

}
