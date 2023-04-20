using Game.Frame.Coroutine;
using GameApp.Cache;
using GameApp.Entitys;
using System.Collections;
using System.Collections.Generic;
using TableData;

namespace GameApp.Map
{
    /// <summary>
    /// 游戏逻辑场景 进入地图前的创建工作
    /// </summary>
    public partial class MapScene
    {
        private void OnEnterMap()
        {
            /// 初始化动态物
            var mapPosList = MapPosConfigManager.Instance.GetByMapId(MapConfig.ID);
            var mapUnitData = GameCache.Map.GetMapUnitDatas(mapPosList);
            foreach (var unitData in mapUnitData)
            {
                RunTimeEntitys.Add(EntityFactory.CreateByEntityData(unitData));
            }
        }
    }
}