using Game.Base.EC;
using Game.Base.LifeCycle;
using UnityEngine;
using System.Collections.Generic;
using Game.Base.Logs;
using GameApp.Cache;
using GameApp.Utils;
using GameApp.UI.System;
using Game.Frame.Timer;

namespace GameApp.Entitys
{
    public class MapAreaComp : ComponentBase, IInitAfter, IShutdown
    {
        private static Dictionary<int, MapAreaComp> compCache = new Dictionary<int, MapAreaComp>();

        private GameObjectComp gameObjectComp;

        private int MapAreaId { get; set; }


        public void InitAfter()
        {
            gameObjectComp = Entity.GetComp<GameObjectComp>();

            /// 初始化数据
            var unitComp = Entity.GetComp<MapUnitComp>();
            //MapAreaId = unitComp.MapUnitData.Config.ItemId;

            /// 加入缓存
            if (!compCache.TryAdd(MapAreaId, this))
            {
                Log.Error($"Debug MapAreaComp Error 地图区域 ID 重复 {MapAreaId}");
            }
        }

        /// <summary>
        /// 设置可见性
        /// </summary>
        public void SetVisiable(bool visiable)
        {
            gameObjectComp.SetActive(visiable);
        }

        public void Shutdown()
        {
            compCache.Remove(MapAreaId);
        }

        /// <summary>
        /// 尝试进入或离开地图区域
        /// </summary>
        public static void TryEnterOrLeaveMapArea(int areaId, bool enter)
        {
            if (compCache.TryGetValue(areaId, out var comp))
            {
                comp.SetVisiable(enter);
            }
        }
    }
}