using System.Collections;
using UnityEngine;
using Game.Base.Module;
using Game.Frame;
using GameApp.GameState;
using GameApp.Table;
using GameApp.Quest;
using GameApp.Cheat;

namespace Game
{
    /// <summary>
    /// 游戏入口脚本
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        public static bool IsGaming { get; private set; }

        private GameStateManager GameState { get; set; }
        private DesignTableManager TableManager { get; set; }
        private QuestManager QuestManager { get; set; }

        private CheatManager CheatManager { get; set; }

        private void Awake()
        {
            IsGaming = true;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            AppFacade.Init(gameObject);
            StartCoroutine(InitializeGame());
        }

        private void Update()
        {
            GameModuleHelper.Update();
        }

        private void LateUpdate()
        {
            GameModuleHelper.LateUpdate();
        }

        private void FixedUpdate()
        {
            GameModuleHelper.FixedUpdate();
        }

        private IEnumerator InitializeGame()
        {
            /// 初始化游戏表格
            TableManager = GameModuleHelper.GetModule<DesignTableManager>();
            /// 初始化任务系统
            QuestManager = GameModuleHelper.GetModule<QuestManager>();
            /// 初始化作弊指令
            CheatManager = GameModuleHelper.GetModule<CheatManager>();
            /// 初始化游戏状态
            GameState = GameModuleHelper.GetModule<GameStateManager>();

            yield return null;
        }

        private void OnApplicationQuit()
        {
            IsGaming = false;
        }

        /// <summary>
        /// 模块调用协程
        /// </summary>
        public void CallCoroutine(IEnumerator function)
        {
            StartCoroutine(function);
        }
    }
}
