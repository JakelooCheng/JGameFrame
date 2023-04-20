using Game.Base.EC;
using Game.Base.LifeCycle;
using UnityEngine;
using System;
using Game.Frame;
using Game.Frame.Resource;
using Game.Frame.Resource.Interface;
using Game.Base.Logs;

namespace GameApp.Entitys
{
    public enum PrefabLoadState
    {
        UnStart,
        Loading,
        LoadSuccess,
        LoadFail,
    }

    public class PrefabComp : ComponentBase, IInit, IInitAfter, IShutdown
    {
        private GameObjectComp GameObjectComp { get; set; }

        private Action<GameObject> OnPrefabLoaded;

        /// <summary>
        /// 预制体路径
        /// </summary>
        public string Path { get; set; }

        public PrefabLoadState LoadState { get; protected set; }

        private IResourceToken token;


        public void Init()
        {
            token = AppFacade.Resource.GetToken();
        }

        public void InitAfter()
        {
            GameObjectComp = GetComp<GameObjectComp>();
            LoadPrefab();
        }

        public void Load(string path)
        {
            Path = path;
            LoadPrefab();
        }

        public void LoadPrefab()
        {
            LoadState = PrefabLoadState.Loading;
            AppFacade.Resource.LoadAsset(Path, InstAsset, token);
        }

        private void InstAsset(IAsset asset)
        {
            if (asset.RealAsset == null)
            {
                LoadState = PrefabLoadState.LoadFail;
                Log.Error($"PrefabComp Error Load {Path} Error!");
                return;
            }

            if (GameObjectComp.GameObject)
            {
                GameObject.Destroy(GameObjectComp.GameObject);
            }

            var go = GameObject.Instantiate(asset.RealAsset as GameObject, (Entity.Manager as EntityManager).transform);

            LoadState = PrefabLoadState.LoadSuccess;
            GameObjectComp.SetGameObject(go);
            go.name = Entity.Name;

            OnPrefabLoaded?.Invoke(go);
        }

        public void Subscribe(Action<GameObject> callback)
        {
            OnPrefabLoaded += callback;
        }

        public void Unsubscribe(Action<GameObject> callback)
        {
            OnPrefabLoaded -= callback;
        }

        public void Shutdown()
        {
            AppFacade.Resource.ReleaseToken(token);
        }
    }
}