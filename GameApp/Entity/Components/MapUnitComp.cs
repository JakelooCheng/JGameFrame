using Game.Base.EC;
using Game.Base.LifeCycle;
using UnityEngine;
using UnityEngine.AI;
using Game.Base.ObjectPool;
using GameApp.Cache;
using Game.Frame.PlayerData;

namespace GameApp.Entitys
{
    public class MapUnitComp : ComponentBase, IShutdown, IInitAfter
    {
        public MapUnitData MapUnitData { get; private set; }


        public virtual void InitAfter()
        {
            OnMapEntityChange();
        }

        public virtual void SetData(MapUnitData data)
        {
            MapUnitData = data;
            GameCache.Map.AddMapEntity(this);
        }

        /// <summary>
        /// 当地图状态发生变化时
        /// </summary>
        public virtual void OnMapEntityChange()
        {
            if (MapUnitData == null)
            {
                return;
            }

            var state = MapUnitData.EntityExtData == null ? EntityState.Default : MapUnitData.EntityExtData.State;

            /// 控制外显
            var gameObjectComp = GetComp<GameObjectComp>();
            if (gameObjectComp != null)
            {
                gameObjectComp.SetActive(state != EntityState.Hide);
            }
            Entity.IsActive = state != EntityState.Hide;

            /// 控制测距
            var measureComp = GetComp<MeasureComp>();
            if (measureComp != null)
            {
                measureComp.IsActive = state != EntityState.Hide;
            }
        }

        public virtual void Shutdown()
        {
            if (MapUnitData != null)
            {
                GameCache.Map.RemoveMapEntity(this);
                MapUnitData.Release();
            }
        }
    }
}