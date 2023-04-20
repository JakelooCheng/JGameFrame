using Game.Frame.Coroutine;
using Game.Frame;
using Game.Frame.Timer;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameApp.Entitys;
using GameApp.Cache;

namespace GameApp.Map
{
    public enum LoadState
    {
        Init, // 初始化
        Loading, // 加载中
        Loaded, // 加载完成
        Unload, // 卸载场景
        Unloaded
    }

    /// <summary>
    /// 游戏逻辑场景 加载相关
    /// </summary>
    public partial class MapScene
    {
        private LoadState loadState = LoadState.Init;
        public LoadState LoadState
        {
            get => loadState;
            set
            {
                OnLoadStateChange(value);
                loadState = value;
            }
        }

        private void OnLoadStateChange(LoadState curState)
        {
            switch (curState)
            {
                case LoadState.Loading:
                    StartLoad(); // 开始加载
                    break;
                case LoadState.Loaded:
                    break;
                case LoadState.Unload:
                    StartUnload(); // 卸载场景
                    break;
                case LoadState.Unloaded:
                    break;
            }
        }

        private void StartLoad()
        {
            AppFacade.Coroutine.StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            var loader = SceneManager.LoadSceneAsync(MapConfig.ScenePath, LoadSceneMode.Additive);
            while (!loader.isDone)
            {
                yield return new AwaitFrame(1);
            }

            /// 加入场景根节点
            scene = SceneManager.GetSceneByName(MapConfig.ScenePath);

            /// 获取组件
            foreach (var go in scene.GetRootGameObjects())
            {
                switch (go.tag)
                {
                    case "MainCamera":
                        CameraMovement = go.GetComponent<CameraMovementMono>();
                        GameCache.Player.SetCamera(CameraMovement);
                        break;
                    case "RunTimeEntitys":
                        RunTimeEntitys = go.GetComponent<EntityManager>();
                        break;
                }
            }

            /// 设置 Player
            var playerEntity = EntityFactory.CreatePlayer();
            RunTimeEntitys.Add(playerEntity);
            while (playerEntity.GetComp<PrefabComp>().LoadState != PrefabLoadState.LoadSuccess)
            {
                yield return new AwaitFrame(1); // 等待 Player 加载完成
            }
            var playerGo = playerEntity.GetComp<GameObjectComp>();
            CameraMovement.Target = playerGo.Transform.Find("CameraPoint");
            playerGo.Transform.position = AppFacade.PlayerData.Player.PlayerPos;
            GameCache.Player.PlayerEntity = playerEntity;

            /// 把场景相机移过来
            CameraMovement.transform.position = playerGo.Transform.position;

            /// 进入地图前的准备工作
            OnEnterMap();

            yield return new AwaitFrame(1);
            LoadState = LoadState.Loaded;
        }

        private void StartUnload()
        {
            AppFacade.Coroutine.StartCoroutine(UnloadScene());
        }

        private IEnumerator UnloadScene()
        {
            RunTimeEntitys.enabled = false; // 暂停大地图
            CameraMovement.enabled = false; // 暂停摄像机
            GameCache.Player.PlayerEntity = null; // 移除 Player

            /// 卸载场景
            var loader = SceneManager.UnloadSceneAsync(scene);
            while (!loader.isDone)
            {
                yield return new AwaitFrame(1);
            }

            /// 处理自身

            yield return new AwaitFrame(1);
            LoadState = LoadState.Unloaded;
        }
    }
}