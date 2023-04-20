using Game.Base.Module;
using GameApp.GameState;
using System.Collections;
using System.Collections.Generic;
namespace GameApp.Cache
{
    public static class GameCache
    {
        private static List<IGameCache> caches = new List<IGameCache>();

        private static T Make<T>() where T : IGameCache, new()
        {
            var result = new T();

            caches.Add(result);
            return result;
        }

        #region 注册组件
        public static PlayerCache Player = Make<PlayerCache>();
        public static BagCache Bag = Make<BagCache>();
        public static DropCache Drop = Make<DropCache>();
        public static InteractionCache Interaction = Make<InteractionCache>();
        public static MapCache Map = Make<MapCache>(); 
        #endregion

        #region 缓存 Manager
        public static GameStateManager GameState;
        #endregion

        public static void Create()
        {
            GameState = GameModuleHelper.GetModule<GameStateManager>();
            foreach (var cache in caches)
            {
                cache.Create();
            }
        }

        public static void Clear()
        {
            GameState = null;
            foreach (var cache in caches)
            {
                cache.Clear();
            }
        }
    }
}
