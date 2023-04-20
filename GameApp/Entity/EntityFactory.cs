using Game.Base.EC;
using Game.Base.Logs;
using Game.Frame;
using Game.Frame.Event;
using GameApp.Utils;
using TableData;
using UnityEngine;

namespace GameApp.Entitys
{
    public static class EntityFactory
    {
        /// <summary>
        /// 用 MapUnitData 创建，理应都是创建在地图上的组件
        /// </summary>
        /// <returns></returns>
        public static Entity CreateByEntityData(MapUnitData unitData)
        {
            var entity = Entity.Make(unitData.Name);
            unitData.Guid = entity.Id;

            switch (unitData.EntityType)
            {
                case EntityType.Default:
                    MakeDefaultEntity(entity, unitData);
                    break;
                case EntityType.NPC:
                    MakeNpcEntity(entity, unitData);
                    break;
                case EntityType.Trans:
                    MakeTrans(entity, unitData);
                    break;
                case EntityType.PickUp:
                    MakePickUp(entity, unitData);
                    break;
                case EntityType.FacingGroup:
                    MakeFacingGroup(entity, unitData);
                    break;
                case EntityType.Chair:
                    MakeChair(entity, unitData);
                    break;
            }

            /// 地图默认创建的动态物
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                if (mapEntityConfig.FacingCamera)
                {
                    entity.AddComp<FacingCameraComp>();
                }
            }
            /// 系统创建的动态物
            else
            {

            }

            return entity;
        }

        /// <summary>
        /// 默认动态物
        /// </summary>
        private static Entity MakeDefaultEntity(Entity entity, MapUnitData unitData)
        {
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                entity.AddComp<GameObjectComp>().SetGameObject(mapEntityConfig.GameObject);
            }
            return entity;
        }

        /// <summary>
        /// NPC 实体
        /// </summary>
        private static Entity MakeNpcEntity(Entity entity, MapUnitData unitData)
        {
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                entity.AddComp<GameObjectComp>().SetGameObject(mapEntityConfig.GameObject);
            }
            else if (unitData.EntityConfig is MapPosConfig mapPosConfig)
            {
                entity.AddComp<GameObjectComp>();
                entity.AddComp<PrefabComp>();
            }
            entity.AddComp<MeasureComp>().AddMeasureP2P(InteractionType.NPC, ConstValue.MapNpcMeasureRange);
            entity.AddComp<MapNpcComp>().SetData(unitData);

            return entity;
        }

        /// <summary>
        /// 传送点
        /// </summary>
        private static Entity MakeTrans(Entity entity, MapUnitData unitData)
        {
            entity.AddComp<MeasureComp>().AddMeasureP2P(InteractionType.Trans, ConstValue.MapTransMeasureRange);
            entity.AddComp<TransferComp>();
            entity.AddComp<MapUnitComp>().SetData(unitData);

            return entity;
        }

        /// <summary>
        /// 地图区域
        /// </summary>
        private static Entity MakeMapArea(Entity entity, MapUnitData config)
        {
            //entity.AddComp<GameObjectComp>().SetGameObject(config.GameObject);
            //entity.AddComp<MapAreaComp>();

            return entity;
        }

        /// <summary>
        /// 面向摄像机组
        /// </summary>
        private static Entity MakeFacingGroup(Entity entity, MapUnitData unitData)
        {
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                entity.AddComp<GameObjectComp>().SetGameObject(mapEntityConfig.GameObject);
                entity.AddComp<FacingCameraGroupComp>();
            }
            entity.AddComp<MapUnitComp>().SetData(unitData);
            return entity;
        }

        /// <summary>
        /// 采集物
        /// </summary>
        private static Entity MakePickUp(Entity entity, MapUnitData unitData)
        {
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                entity.AddComp<GameObjectComp>().SetGameObject(mapEntityConfig.GameObject);
            }
            entity.AddComp<AnimationComp>();
            entity.AddComp<MeasureComp>().AddMeasureP2P(InteractionType.PickUp, ConstValue.MapPickUpMeasureRange);
            entity.AddComp<PickUpComp>().SetData(unitData);
            return entity;
        }

        /// <summary>
        /// 椅子
        /// </summary>
        private static Entity MakeChair(Entity entity, MapUnitData unitData)
        {
            if (unitData.EntityConfig is MapEntityConfig mapEntityConfig)
            {
                entity.AddComp<GameObjectComp>().SetGameObject(mapEntityConfig.GameObject);
            }
            entity.AddComp<MeasureComp>().AddMeasureP2P(InteractionType.Chair, ConstValue.MapChairMeasureRange);
            entity.AddComp<MapChairComp>().SetData(unitData);
            return entity;
        }

        /// <summary>
        /// 创建 Player Entity
        /// </summary>
        public static Entity CreatePlayer()
        {
            var entity = Entity.Make("Player");
            entity.AddComp<GameObjectComp>();
            entity.AddComp<PrefabComp>().Path = AppFacade.PlayerData.Player.PlayerPrefab;
            entity.AddComp<PlayerComp>();
            return entity;
        }
    }
}