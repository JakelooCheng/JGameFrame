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
        /// ����ʵ��ģ��
        /// </summary>
        private static readonly Dictionary<Type, GameModuleBase> allGameModules = new Dictionary<Type, GameModuleBase>();

        // ���а�֡��Ⱦģ��
        private static readonly List<IUpdate> allUpdates = new List<IUpdate>();

        // ���к�֡��Ⱦģ��
        private static readonly List<ILateUpdate> allLateUpdates = new List<ILateUpdate>();

        // ���й̶�֡��Ⱦģ��
        private static readonly List<IFixedUpdate> allFixedUpdates = new List<IFixedUpdate>();

        public static GameObject Entry;

        /// <summary>
        /// ��ȡģ�飬���������ʵ����ֱ�Ӵ���
        /// </summary>
        /// <typeparam name="T">�̳� GameModuleBase</typeparam>
        /// <returns>ģ��</returns>
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

            // ���������ڻ���
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

            // ִ�г�ʼ����������
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
