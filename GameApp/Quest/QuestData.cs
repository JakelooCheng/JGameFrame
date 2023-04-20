using Game.Base.Logs;
using Game.Frame;
using System;
using TableData;

namespace GameApp.Quest
{
    /// <summary>
    /// 任务缓存
    /// </summary>
    public class QuestData
    {
        /// <summary>
        /// 任务 ID
        /// </summary>
        public int ID => QuestConfig.ID;

        /// <summary>
        /// 任务配置
        /// </summary>
        public QuestConfig QuestConfig { get; private set; }

        /// <summary>
        /// 当前任务边
        /// </summary>
        public QuestEdgeConfig CurEdgeConfig { get; set; }

        /// <summary>
        /// 当前任务边跳转条件
        /// </summary>
        public ConditionBase CurJumpCondition { get; private set; }

        /// <summary>
        /// 当前正在运行的行为
        /// </summary>
        public BehaviorBase CurRunningBehavior { get; private set; }

        /// <summary>
        /// 当前行为 Index
        /// </summary>
        private int curBehaviorIndex = 0;


        public QuestData(int questId)
        {
            QuestConfig = QuestConfigManager.Instance.GetItem(questId);
            JumpToEdge(QuestConfig.RootEdge);
        }

        public QuestData(int questId, int startEdge)
        {
            QuestConfig = QuestConfigManager.Instance.GetItem(questId);
            JumpToEdge(startEdge, false);
        }

        public void OnUpdate()
        {
            /// 准备跳转边
            if (CurRunningBehavior == null)
            {
                TryJumpEdge();
            }
            /// 等待行为执行
            else
            {
                if (CurRunningBehavior.IsFinish)
                {
                    CurRunningBehavior = null;
                    curBehaviorIndex++;
                    TryJumpBehavior();
                }
            }
        }

        /// <summary>
        /// 尝试跳行为，跳不到下一次准备跳边
        /// </summary>
        private void TryJumpBehavior()
        {
            /// 判断行为列表是否执行到最后
            if (curBehaviorIndex < CurEdgeConfig.BehaviorList.Count)
            {
                var behaviorId = CurEdgeConfig.BehaviorList[curBehaviorIndex];
                try
                {
                    CurRunningBehavior = BehaviorBase.Make(behaviorId);
                    Log.Info($"Debug QuestManager 执行行为 任务:{ID} 边:{CurEdgeConfig.ID} 行为:{behaviorId}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Debug QuestManager Error 行为执行失败 {ex}");
                    CurRunningBehavior = null;
                }
            }
        }

        /// <summary>
        /// 尝试跳转边
        /// </summary>
        private void TryJumpEdge()
        {
            /// 不跳转直接结束掉
            if (CurJumpCondition == null)
            {
                Log.Info($"Debug QuestManager 任务完成 {ID}");
                QuestManager.Instance.CloseQuest(ID);
            }
            /// 判断要不要跳转
            else
            {
                try
                {
                    int nextEdge = CurJumpCondition.Check();
                    if (nextEdge > 0)
                    {
                        JumpToEdge(nextEdge);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Debug QuestManager Error 条件判断失败 {ex}，直接结束任务 {ID}");
                    CurJumpCondition = null;
                }
            }
        }

        /// <summary>
        /// 跳转到对应边
        /// </summary>
        /// <param name="edgeId"></param>
        public void JumpToEdge(int edgeId, bool needUpdate = true)
        {
            var edgeConfig = QuestEdgeConfigManager.Instance.GetItem(edgeId);
            if (edgeConfig == null)
            {
                Log.Error($"Debug QuestManager Error QuestEdgeConfig 不存在 ID {edgeId}");
            }
            Log.Info($"Debug QuestManager 跳转边 任务:{ID} 边:{edgeId}");
            CurEdgeConfig = edgeConfig;
            CurJumpCondition = ConditionBase.Make(CurEdgeConfig.JumpCondition);
            CurRunningBehavior = null;
            curBehaviorIndex = 0;
            TryJumpBehavior();

            if (needUpdate)
            {
                QuestManager.Instance.UpdatePlayerQuestData(ID, edgeId);
            }
        }

        #region Compare
        public override bool Equals(object obj)
        {
            return ID == (obj as QuestData)?.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        #endregion
    }
}
