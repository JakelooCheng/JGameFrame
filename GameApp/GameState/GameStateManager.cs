using Game.Base.Module;
using Game.Base.LifeCycle;
using Game.Frame.Fsm;
using Game.Frame;
using Game.Frame.Event;
using GameApp.Map;

namespace GameApp.GameState
{
    public class GameStateManager : GameModuleBase, IInit, IShutdown
    {
        public MapScene CurMapScene;

        private IFsm<GameStateManager> gameStateFsm;

        public void Init()
        {
            gameStateFsm = AppFacade.FSM.CreateFsm(
                    this,
                    new GameStartState(),
                    new GameReadyState(),
                    new GameLoadState(),
                    new GameMapState()
                );
            gameStateFsm.Start<GameStartState>();
        }

        public void Shutdown()
        {
            
        }
    }
}