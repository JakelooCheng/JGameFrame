using Game.Base.LifeCycle;
using Game.Base.ObjectPool;
using System;
using System.Collections.Generic;

namespace Game.Base.EC
{
    public class Entity : IEntity
    {
        public IEntityManager Manager { get; set; }

        public PoolDictionary<Type, IComponent> compDic
        {
            get;
            set;
        }
        public PoolList<IUpdate> updateList;

        public int Id { get; set; }
        public string Name { get; set; }

        private bool isActive;
        public bool IsActive
        {
            set
            {
                isActive = value;
            }
            get
            {
                return CheckActive == null ? isActive : CheckActive();
            }
        }
        public Func<bool> CheckActive;

        private int tempCompCount = 0;
        private static int instanceId = 0;

        public static Entity Make(string name)
        {
            var entity = GlobalObjectPool<Entity>.Get();
            entity.compDic = PoolDictionary<Type, IComponent>.Get();
            entity.updateList = PoolList<IUpdate>.Get();

            entity.Id = instanceId++;
            entity.Name = name;
            entity.CheckActive = null;
            return entity;
        }

        public void Init()
        {
            IsActive = true;
        }

        /// <summary>
        /// 在添加完所有组件后手动调用
        /// </summary>
        public void Start()
        {
            foreach (var comp in compDic.Values)
            {
                if (comp is IInitAfter initAfterComp)
                {
                    initAfterComp.InitAfter();
                }
            }
        }

        public T AddComp<T>() where T : IComponent, new()
        {
            var comp = new T();
            comp.Entity = this;
            compDic.Add(typeof(T), comp);

            if (comp is IInit initComp)
            {
                initComp.Init();
            }
            if (comp is IUpdate updateComp)
            {
                updateList.Add(updateComp);
            }
            return comp;
        }

        public void OnUpdate()
        {
            if (IsActive)
            {
                tempCompCount = updateList.Count;
                for (int index = 0; index < tempCompCount; index++)
                {
                    updateList[index].OnUpdate();
                }
            }
        }

        public T GetComp<T>() where T : IComponent
        {
            return (T)GetComp(typeof(T));
        }

        public IComponent GetComp(Type type)
        {
            if (!compDic.TryGetValue(type, out var comp))
            {
                foreach (var com in compDic.Values)
                {
                    if (com.GetType().IsSubclassOf(type))
                    {
                        return com;
                    }
                }
                comp = null;
            }
            return comp;
        }

        public bool RemoveComp<T>() where T : IComponent
        {
            if (compDic.TryGetValue(typeof(T), out var comp))
            {
                if (comp is IShutdown shutdownComp)
                {
                    shutdownComp.Shutdown();
                }
                tempCompCount--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Shutdown()
        {
            foreach (var comp in compDic)
            {
                if (comp.Value is IShutdown shutdownComp)
                {
                    shutdownComp.Shutdown();
                }
            }
            compDic.Dispose();
            compDic = null;
            updateList.Dispose();
            updateList = null;
            GlobalObjectPool<Entity>.Release(this);
        }

        public override bool Equals(object obj)
        {
            return (obj as Entity).Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
