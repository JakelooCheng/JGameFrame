using Game.Base.Module;
using Game.Base.Logs;
using Game.Frame.Event;
using Game.Frame.Resource;
using Game.Frame.UI;
using UnityEngine;
using Game.Frame.Timer;
using Game.Frame.Fsm;
using Game.Base.EC;
using Game.Frame.Coroutine;
using Game.Frame.PlayerData;

namespace Game.Frame
{
    public static class AppFacade
    {
        public static EventManager Event { get; private set; }
        public static ResourceManager Resource { get; private set; }
        public static UIManager UI { get; private set; }
        public static TimerManager Timer { get; private set; }
        public static FsmManager FSM { get; private set; }
        public static CoroutineManager Coroutine { get; private set; }
        public static PlayerDataManager PlayerData { get; private set; }

        public static void Init(GameObject entry)
        {
            // 指定项目 Logger
            Log.Logger = new U3DLogger();

            // 初始化 Modules
            GameModuleHelper.Entry = entry;
            
            Event = GameModuleHelper.GetModule<EventManager>();
            Resource = GameModuleHelper.GetModule<ResourceManager>();
            UI = GameModuleHelper.GetModule<UIManager>();
            Timer = GameModuleHelper.GetModule<TimerManager>();
            FSM = GameModuleHelper.GetModule<FsmManager>();
            Coroutine = GameModuleHelper.GetModule<CoroutineManager>();
            PlayerData = GameModuleHelper.GetModule<PlayerDataManager>();
        }
    }
}