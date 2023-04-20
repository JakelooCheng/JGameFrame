using Game.Base.Module;
using Game.Base.LifeCycle;
using Game.Frame.Fsm;
using Game.Frame;
using GameApp.UI.Start;
using GameApp.UI.System;
using GameApp.Table;
using GameApp.Map;
using GameApp.Cache;
using GameApp.Quest;

namespace GameApp.GameState
{
    /// <summary>
    /// 游戏开始的状态
    /// </summary>
    public class GameStartState : FsmStateBase<GameStateManager>
    {
        public override void OnInit(IFsm<GameStateManager> fsm)
        {
            /// 启动游戏时要加载的表格
            DesignTableManager.Instance.LoadOnStart();
        }

        public override void OnEnter(IFsm<GameStateManager> fsm)
        {
            AppFacade.UI.OpenWindow<StartWindow>();

            // 清理缓存
            GameCache.Clear();
            QuestManager.Instance.Clear();
        }

        public override void OnUpdate(IFsm<GameStateManager> fsm)
        {
            // 有游戏数据的时候跳转状态
            if (AppFacade.PlayerData.Player != null)
            {
                ChangeState<GameReadyState>(fsm);
            }
        }

        public override void OnLeave(IFsm<GameStateManager> fsm, bool isShutdown)
        {
            AppFacade.UI.CloseWindow<StartWindow>();

            /// 游戏进入场景前 Loading 时要加载的表格
            DesignTableManager.Instance.LoadOnEnter();

            /// 初始化游戏数据
            GameCache.Create();
        }
    }
}