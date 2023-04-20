using GameApp.Cache;
using Game.Base.Logs;
using GameApp.Entitys;

namespace GameApp.Quest
{
    /// <summary>
    /// 显示或隐藏 Entity
    /// </summary>
    public class ShowOrHideEntity : BehaviorBase
    {
        public override void Excute()
        {
            /// 设置可见性
            GameCache.Map.SetMapUnitData(
                BehaviorConfig.IntArg1, BehaviorConfig.BoolArg1 ? 
                Game.Frame.PlayerData.EntityState.Default : 
                Game.Frame.PlayerData.EntityState.Hide);
            Finish();
        }
    }
}