using Game.Base.ObjectPool;
using Game.Frame;
using Game.Frame.PlayerData;
using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;

namespace GameApp.Entitys
{
    public class MapUnitData
    {
        public int Guid;

        public string Name => $"{EntityType}_{EntityConfig.CustomID}_{Guid}";

        public EntityType EntityType => EntityConfig.EntityType;

        public IEntityConfig EntityConfig;

        /// <summary>
        /// 本地缓存的标记信息，可能为空，为空则是默认值
        /// </summary>
        public EntityExt EntityExtData;


        public static MapUnitData Make(MapEntityConfig mapEntityConfig)
        {
            var unitData = GlobalObjectPool<MapUnitData>.Get();
            unitData.EntityConfig = mapEntityConfig;
            CheckExtData(unitData);
            return unitData;
        }

        public static MapUnitData Make(MapPosConfig mapPosConfig)
        {
            var unitData = GlobalObjectPool<MapUnitData>.Get();
            unitData.EntityConfig = mapPosConfig;
            CheckExtData(unitData);
            return unitData;
        }

        private static void CheckExtData(MapUnitData unitData)
        {
            int checkId = unitData.EntityConfig.ID;

            if (checkId != 0 && AppFacade.PlayerData.Player.Entitys.TryGetValue(checkId, out var extData))
            {
                unitData.EntityExtData = extData;
            }
        }

        public void Release()
        {
            GlobalObjectPool<MapUnitData>.Release(this);
        }
    }

    public static class EntityDataFactory
    {

    }
}
