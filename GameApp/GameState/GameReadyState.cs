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
using GameApp.Map;

namespace GameApp.GameState
{
    /// <summary>
    /// 游戏准备状态，仅作分发使用
    /// </summary>
    public class GameReadyState : FsmStateBase<GameStateManager>
    {
        public override void OnEnter(IFsm<GameStateManager> fsm)
        {
            // 有游戏数据的时候跳转状态
            if (AppFacade.PlayerData.Player != null)
            {
                fsm.Owner.CurMapScene = MapScene.Make(AppFacade.PlayerData.Player.MapId);
                ChangeState<GameLoadState>(fsm);
            }
            // 否则回大地图
            else
            {
                ChangeState<GameStartState>(fsm);
            }
        }

        public override void OnLeave(IFsm<GameStateManager> fsm, bool isShutdown)
        {

        }
    }
}