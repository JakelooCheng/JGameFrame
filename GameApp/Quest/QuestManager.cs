using Game.Base.Module;
using Game.Base.LifeCycle;
using System.Collections.Generic;
using TableData;
using Game.Base.Logs;
using Game.Frame;
using GameApp.GameState;
using GameApp.Cache;

namespace GameApp.Quest
{
    public class QuestManager : GameModuleBase, IInit, IUpdate, IClear, IShutdown
    {
        public static QuestManager Instance;

        public Dictionary<int, QuestData> questCache = new Dictionary<int, QuestData>();

        private List<int> willRemove = new List<int>();


        public void Init()
        {
            QuestHelper.Init();
            Instance = this;
        }

        public void OnUpdate()
        {
            /// 地图加载完成前不执行任务
            if (GameCache.GameState?.CurMapScene?.LoadState != Map.LoadState.Loaded)
            {
                return;
            }

            /// 更新任务
            foreach (var quest in questCache.Values)
            {
                quest.OnUpdate();
            }

            /// 移除要移除的任务
            foreach (var remove in willRemove)
            {
                questCache.Remove(remove);
            }
            willRemove.Clear();
        }

        /// <summary>
        /// 上线同步任务状态（只在第一次进行同步）
        /// </summary>
        public void InitAllQuest()
        {
            var player = AppFacade.PlayerData.Player;
            if (player == null || questCache.Count != 0)
            {
                return;
            }

            foreach (var quest in player.RunningQuest)
            {
                var questData = new QuestData(quest.Key, quest.Value);
                if (!questCache.TryAdd(quest.Key, questData))
                {
                    Log.Error($"Debug QuestManager Error 重复开启任务，任务ID {quest.Key}");
                }
            }
        }

        /// <summary>
        /// 开始一个任务
        /// </summary>
        public void StartQuest(int questId)
        {
            var questData = new QuestData(questId);
            if (!questCache.TryAdd(questId, questData))
            {
                Log.Error($"Debug QuestManager Error 重复开启任务，任务ID {questId}");
            }
            else
            {
                UpdatePlayerQuestData(questData.ID, questData.CurEdgeConfig.ID);
            }
        }

        /// <summary>
        /// 关闭一个任务
        /// </summary>
        public void CloseQuest(int questId)
        {
            UpdatePlayerQuestData(questId, 0, true);

            willRemove.Add(questId);
        }

        /// <summary>
        /// 更新本地玩家任务数据
        /// </summary>
        public void UpdatePlayerQuestData(int questId, int edgeId, bool isFinish = false)
        {
            var player = AppFacade.PlayerData.Player;
            if (isFinish)
            {
                player.RunningQuest.Remove(questId);
                if (!player.FinishedQuest.Contains(questId))
                {
                    player.FinishedQuest.Add(questId);
                }
            }
            else
            {
                player.RunningQuest[questId] = edgeId;
            }
            player.IsDirty = true;
        }

        public void Clear()
        {
            questCache.Clear();
        }

        public void Shutdown()
        {
            Clear();
        }
    }
}