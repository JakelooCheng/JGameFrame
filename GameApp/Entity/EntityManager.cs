using Game.Base.EC;
using Game.Base.LifeCycle;
using Game.Base.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameApp.Cache;

namespace GameApp.Entitys
{
    [Serializable]
    public class EntityManager : MonoBehaviour, IEntityManager
    {
        public bool Running => enabled;

        [Title("Entitys 列表")]
        [TableList(ShowPaging = true), PropertySpace]
        public List<MapEntityConfig> StartEntitys = new List<MapEntityConfig>();

        protected HashSet<Entity> entitys = new HashSet<Entity>();
        private List<Entity> willAdd = new List<Entity>();
        private List<Entity> willRemove = new List<Entity>();

        protected bool isUpdate = false;


        private void Start()
        {
            var unitDataList = GameCache.Map.GetMapUnitDatas(StartEntitys);
            foreach (var unitData in unitDataList)
            {
                Add(EntityFactory.CreateByEntityData(unitData));
            }
        }

        private void Update()
        {
            if (willAdd.Count > 0)
            {
                foreach (var entity in willAdd)
                {
                    if (!entitys.Add(entity))
                    {
                        Log.Error($"EntityManager Error 重复添加 Entity {entity.Id} {entity.Name}");
                    }
                    entity.Start();
                }
                willAdd.Clear();
            }

            if (willRemove.Count > 0)
            {
                foreach (var entity in willRemove)
                {
                    entity.Shutdown();
                    if (!entitys.Remove(entity))
                    {
                        Log.Error($"EntityManager Error 移除失败 Entity {entity.Id} {entity.Name}");
                    }
                }
                willRemove.Clear();
            }

            isUpdate = true;
            if (Running)
            {
                foreach (var entity in entitys)
                {
                    entity.OnUpdate();
                }
            }
            isUpdate = false;
        }

        private void OnDestroy()
        {
            foreach (var add in willAdd)
            {
                add.Shutdown();
            }
            foreach (var remove in willRemove)
            {
                remove.Shutdown();
            }
            foreach (var entity in entitys)
            {
                entity.Shutdown();
            }
            willAdd.Clear();
            willRemove.Clear();
            entitys.Clear();
        }

        public virtual void Add(Entity entity)
        {
            entity.Manager = this;
            entity.Init();
            if (isUpdate)
            {
                willAdd.Add(entity);
            }
            else
            {
                if (!entitys.Add(entity))
                {
                    Log.Error($"EntityManager Error 重复添加 Entity {entity.Id} {entity.Name}");
                }
                entity.Start();
            }
        }

        public virtual void Remove(Entity entity)
        {
            if (isUpdate)
            {
                willRemove.Add(entity);
            }
            else
            {
                entity.Shutdown();
                if (!entitys.Remove(entity))
                {
                    Log.Error($"EntityManager Error 移除失败 Entity {entity.Id} {entity.Name}");
                }
            }
            entity.Manager = null;
        }

        /// <summary>
        /// 通过名称获取物体
        /// </summary>
        public IEntity GetEntityByName(string name)
        {
            foreach (var entity in entitys)
            {
                if (entity.Name == name)
                {
                    return entity;
                }
            }
            return null;
        }
    }
}