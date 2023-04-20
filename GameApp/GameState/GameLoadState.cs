using Game.Base.Module;
using Game.Base.LifeCycle;
using Game.Frame.Fsm;
using Game.Frame;
using GameApp.UI.Start;
using GameApp.UI.System;
using GameApp.Table;

namespace GameApp.GameState
{
    public class GameLoadState : FsmStateBase<GameStateManager>
    {
        public override void OnEnter(IFsm<GameStateManager> fsm)
        {
            fsm.Owner.CurMapScene.LoadState = Map.LoadState.Loading;
            AppFacade.UI.OpenWindow<SceneLoadingWindow>();
            AppFacade.UI.OpenWindow<CommonUITransitionWindow>();
        }

        public override void OnUpdate(IFsm<GameStateManager> fsm)
        {
            if (fsm.Owner.CurMapScene.LoadState == Map.LoadState.Loaded)
            {
                ChangeState<GameMapState>(fsm);
            }
        }

        public override void OnLeave(IFsm<GameStateManager> fsm, bool isShutdown)
        {
            AppFacade.UI.CloseWindow<SceneLoadingWindow>();
        }
    }
}
