using Game.Base.Module;
using Game.Base.LifeCycle;
using Game.Frame.Fsm;
using Game.Frame;
using GameApp.UI.Start;
using GameApp.UI.System;
using GameApp.UI.Map;
using UnityEngine;
using GameApp.Cache;
using GameApp.UI.HUD;
using GameApp.Quest;

namespace GameApp.GameState
{
    public class GameMapState : FsmStateBase<GameStateManager>
    {
        public override void OnEnter(IFsm<GameStateManager> fsm)
        {
            AppFacade.UI.OpenWindow<MapWindow>();
            AppFacade.UI.OpenWindow<HUDWindow>();
            AppFacade.UI.OpenWindow<ClickWindow>();
            AppFacade.UI.OpenWindow<ToastWindow>();

            /// 上线开启所有任务
            QuestManager.Instance.InitAllQuest();
        }

        public override void OnUpdate(IFsm<GameStateManager> fsm)
        {
            if (fsm.Owner.CurMapScene.LoadState == Map.LoadState.Unloaded)
            {
                fsm.Owner.CurMapScene = null;
                ChangeState<GameReadyState>(fsm);
            }
        }

        public override void OnLeave(IFsm<GameStateManager> fsm, bool isShutdown)
        {
            AppFacade.UI.CloseWindow<ToastWindow>();
            AppFacade.UI.CloseWindow<ClickWindow>();
            AppFacade.UI.CloseWindow<HUDWindow>();
            AppFacade.UI.CloseWindow<MapWindow>();
        }
    }
}
