using Game.Base.LifeCycle;
using Game.Base.Logs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Base.Module
{
    public static class GameModuleHelper
    {
        /// <summary>
        /// 所有实例模块
        /// </summary>
        private static readonly Dictionary<Type, GameModuleBase> allGameModules = new Dictionary<Type, GameModuleBase>();

        // 所有按帧渲染模块
        private static readonly List<IUpdate> allUpdates = new List<IUpdate>();

        // 所有后帧渲染模块
        private static readonly List<ILateUpdate> allLateUpdates = new List<ILateUpdate>();

        // 所有固定帧渲染模块
        private static readonly List<IFixedUpdate> allFixedUpdates = new List<IFixedUpdate>();

        public static GameObject Entry;

        /// <summary>
        /// 获取模块，如果不存在实例则直接创建
        /// </summary>
        /// <typeparam name="T">继承 GameModuleBase</typeparam>
        /// <returns>模块</returns>
        public static T GetModule<T>() where T : GameModuleBase
        {
            return GetModule(typeof(T)) as T;
        }

        public static GameModuleBase GetModule(Type moduleType)
        {
            if (allGameModules.TryGetValue(moduleType, out var module))
            {
                return module;
            }
            return CreateModule(moduleType);
        }

        private static GameModuleBase CreateModule(Type moduleType)
        {
            GameModuleBase module = (GameModuleBase)Activator.CreateInstance(moduleType);
            allGameModules.Add(moduleType, module);

            // 按生命周期缓存
            if (module is IUpdate update)
            {
                allUpdates.Add(update);
            }

            if (module is ILateUpdate lateUpdate)
            {
                allLateUpdates.Add(lateUpdate);
            }

            if (module is IFixedUpdate fixedUpdate)
            {
                allFixedUpdates.Add(fixedUpdate);
            }

            // 执行初始化生命周期
            if (module is IInit initer)
            {
                initer.Init();
            }

            if (module is IInitAfter initAfter)
            {
                initAfter.InitAfter();
            }

            return module;
        }

        public static void Clear()
        {
            foreach (var module in allGameModules.Values)
            {
                if (module is IClear needClear)
                {
                    needClear.Clear();
                }
            }
        }

        public static void ShutDown()
        {
            foreach (var module in allGameModules.Values)
            {
                if (module is IShutdown needShutDown)
                {
                    needShutDown.Shutdown();
                }
            }

            allGameModules.Clear();
            allUpdates.Clear();
            allLateUpdates.Clear();
            allFixedUpdates.Clear();
        }

        public static void Update()
        {
            foreach (IUpdate module in allUpdates)
            {
                try
                {
                    module.OnUpdate();
                }
                catch (Exception exception)
                {
                    Logs.Log.Error(exception.ToString());
                }
            }
        }

        public static void LateUpdate()
        {
            foreach (ILateUpdate module in allLateUpdates)
            {
                try
                {
                    module.OnLateUpdate();
                }
                catch (Exception exception)
                {
                    Logs.Log.Error(exception.ToString());
                }
            }
        }

        public static void FixedUpdate()
        {
            foreach (IFixedUpdate module in allFixedUpdates)
            {
                try
                {
                    module.OnFixedUpdate();
                }
                catch (Exception exception)
                {
                    Logs.Log.Error(exception.ToString());
                }
            }
        }
    }
}
