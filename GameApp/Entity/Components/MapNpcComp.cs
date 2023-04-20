using Game.Base.EC;
using Game.Base.LifeCycle;
using GameApp.Utils;
using TableData;
using UnityEngine;
using static Codice.Client.Common.WebApi.WebApiEndpoints;
using UnityEngine.AI;
using GameApp.Cache;
using Game.Frame;
using Game.Frame.Event;

namespace GameApp.Entitys
{
    public class MapNpcComp : MapUnitComp, IInteractable, IDialogAble
    {
        private GameObjectComp gameObjectComp;

        public NpcConfig Config { get; private set; }

        public ModelConfig ModelConfig { get; private set; }

        public float DialogOffset => ModelConfig == null ? 1 : ModelConfig.DialogOffset / 100f;

        public string GetDesc()
        {
            return $"互动 {Config.Name}";
        }

        public void OnDialog()
        {
            
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public override void SetData(MapUnitData data)
        {
            base.SetData(data);

            Config = NpcConfigManager.Instance.GetItem(data.EntityConfig.CustomID);
            ModelConfig = ModelConfigManager.Instance.GetItem(Config.ModelID);

            var prefabComp = GetComp<PrefabComp>();
            if (prefabComp != null)
            {
                prefabComp.Path = ResourceUtility.GetModelPath(ModelConfig.Path);
            }
        }

        public override void InitAfter()
        {
            base.InitAfter();
            
            gameObjectComp = GetComp<GameObjectComp>();
            var prefabComp = GetComp<PrefabComp>();
            if (prefabComp != null)
            {
                prefabComp.Subscribe(OnPrefabLoaded);
            }
            else
            {
                OnPrefabLoaded(gameObjectComp.GameObject);
            }
        }

        /// <summary>
        /// 预制体加载完成
        /// </summary>
        private void OnPrefabLoaded(GameObject target)
        {
            var prefabComp = GetComp<PrefabComp>();
            if (prefabComp != null)
            {
                prefabComp.Unsubscribe(OnPrefabLoaded);
            }
        }

        /// <summary>
        /// 交互时触发
        /// </summary>
        public void OnInteraction()
        {
            GameCache.Interaction.PlayDialog(Config.Chat.ToArray(), this, () =>
            {
                QuestTriggerEvent.SendNow(Config.ID, QuestTriggerEvent.TriggerType.NPC);
            });
        }
    }
}