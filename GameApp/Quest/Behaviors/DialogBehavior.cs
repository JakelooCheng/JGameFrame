using Game.Base.Logs;
using Game.Frame;
using GameApp.Cache;
using GameApp.Entitys;
using GameApp.Quest;
using GameApp.UI.HUD;

namespace GameApp.Quest
{
    public class DialogBehavior : BehaviorBase
    {
        public override void Excute()
        {
            /// 获取目标对象
            MapUnitComp targetComp;
            if (BehaviorConfig.IntArg1 == 1)
            {
                targetComp = GameCache.Player.PlayerEntity.GetComp<PlayerComp>();
            }
            else
            {
                targetComp = GameCache.Map.GetMapUnit(BehaviorConfig.IntArg1);
            }

            if (targetComp == null)
            {
                Log.Error("Quest DialogBehavior Error No Entity {BehaviorConfig.IntArg1}");
                Finish();
            }

            /// 播放对话
            var dialog = BehaviorConfig.StrArg1.Split('|');
            GameCache.Interaction.PlayDialog(dialog, targetComp, () => { Finish(); });
        }
    }
}
