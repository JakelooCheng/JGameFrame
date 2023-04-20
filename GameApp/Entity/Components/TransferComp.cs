using Game.Base.EC;
using Game.Base.LifeCycle;
using UnityEngine;
using System.Collections.Generic;
using Game.Base.Logs;
using GameApp.Cache;
using GameApp.Utils;
using GameApp.UI.System;
using Game.Frame.Timer;
using TableData;
using Game.Frame;

namespace GameApp.Entitys
{
    /// <summary>
    /// 传送点组件
    /// </summary>
    public class TransferComp : ComponentBase, IInitAfter, IInteractable
    {
        private MeasureComp measureComp;
        private MapUnitComp mapUnitComp;

        private MapTransConfig selfConfig;
        private MapTransConfig targetConfig;

        private MapConfig selfMap;
        private MapConfig targetMap;
        private MapPosConfig targetMapPosConfig;

        public string GetDesc()
        {
            return $"前往 {targetMap.Name}";
        }

        public void InitAfter()
        {
            mapUnitComp = Entity.GetComp<MapUnitComp>();

            /// 初始化配置
            var entityConfig = mapUnitComp.MapUnitData.EntityConfig;
            selfConfig = MapTransConfigManager.Instance.GetItem(entityConfig.CustomID);
            targetConfig = MapTransConfigManager.Instance.GetItem(selfConfig.TargetID);
            
            var targetMapPosId = MapPosConfigManager.Instance.GetMapIdByTrans(selfConfig.TargetID);
            targetMapPosConfig = MapPosConfigManager.Instance.GetItem(targetMapPosId);
            targetMap = MapConfigManager.Instance.GetItem(targetMapPosConfig.MapID);

            /// 设置测距
            if (entityConfig is MapPosConfig mapPosConfig)
            {
                measureComp = Entity.GetComp<MeasureComp>();
                measureComp.SetPos(new Vector3((float)mapPosConfig.PosX, 0, (float)mapPosConfig.PosZ));
                selfMap = MapConfigManager.Instance.GetItem(mapPosConfig.MapID);
            }
        }

        /// <summary>
        /// 传送到指定地点
        /// </summary>
        public void TransTo()
        {
            CommonUtility.StopPlayer();
            var targetPos = new Vector3((float)targetMapPosConfig.PosX, 0, (float)targetMapPosConfig.PosZ);

            /// 播放传送动画
            CommonUtility.ShowTransIn(TUITransition.DipToBlack, 500, () =>
            {
                /// 同地图传送
                if (targetMapPosConfig.MapID == selfMap.ID)
                {
                    var playerGo = GameCache.Player.PlayerEntity.GetComp<GameObjectComp>();
                    playerGo.GameObject.transform.position = targetPos;

                    AppFacade.Timer.Wait(100, () =>
                    {
                        CommonUtility.RunPlayer();
                    });
                }
                /// 异地图传送
                else
                {
                    AppFacade.Timer.Wait(100, () =>
                    {
                        CommonUtility.ShowTransOut(TUITransition.DipToBlack, 500, () => { });
                    });
                    // 通过设置玩家数据
                    var playerData = AppFacade.PlayerData.Player;
                    playerData.MapId = targetMapPosConfig.MapID;
                    playerData.PlayerPos = targetPos;
                    playerData.IsDirty = true;

                    // 卸载当前地图
                    GameCache.GameState.CurMapScene.LoadState = Map.LoadState.Unload;
                }
            });
        }
    }
}
