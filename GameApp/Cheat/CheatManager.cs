using Game.Base.LifeCycle;
using Game.Base.Module;

namespace GameApp.Cheat
{
    public partial class CheatManager : GameModuleBase, IInit
    {
        public static CheatManager Instance;

        public void Init()
        {
            Instance = this;
        }
    }
}