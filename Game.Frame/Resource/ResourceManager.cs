using Game.Base.LifeCycle;
using Game.Base.Module;
using Game.Base.Logs;
using Game.Frame.UI.Interface;
using UnityEngine;
using System.Collections;
using Game.Frame.UI.Core;
using Game.Frame.Resource.Interface;
using Game.Frame.Resource.Core;
using System;

namespace Game.Frame.Resource
{
    public class ResourceManager : GameModuleBase, IResourceManager
    {
        public T LoadAsset<T>(string path, IResourceToken token) where T : UnityEngine.Object
        {
            return LoadAsset(path, token).RealAsset as T;
        }

        public IAsset LoadAsset(string path, IResourceToken token)
        {
            var obj = Resources.Load(path);
            if (obj == null)
            {
                Log.Error($"ResourceManager Error 资源不存在 {path}！");
            }
            var asset = Asset.Make(path, obj);
            token.AddAsset(asset);
            return asset;
        }

        public void LoadAsset<T>(string path, Action<IAsset> callback, IResourceToken token)
        {
            LoadAsset(path, callback, token);
        }

        public void LoadAsset(string path, Action<IAsset> callback, IResourceToken token)
        {
            AppFacade.Coroutine.StartCoroutine(LoadAssetAsync(path, callback, token));
        }

        public void UnloadAsset(IAsset asset)
        {
            // Resource 无法完成卸载，先注释掉，之后用 AB 包实现
            // Resources.UnloadAsset(asset.RealAsset);
        }

        private IEnumerator LoadAssetAsync(string path, Action<IAsset> callback, IResourceToken token)
        {
            UnityEngine.Object result = null;
            try
            {
                if (callback == null)
                {
                    yield break;
                }

                result = Resources.Load(path);
                if (result == null)
                {
                    Log.Error($"ResourceManager Error 资源不存在 {path}！");
                }
                if (token.IsCancel)
                {
                    Resources.UnloadAsset(result);
                    yield break;
                }
                else
                {
                    var asset = Asset.Make(path, result);
                    token.AddAsset(asset);
                    callback.Invoke(asset);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.ToString());
            }
            yield break;
        }

        public IResourceToken GetToken()
        {
            return ResourceToken.Make();
        }

        public void ReleaseToken(IResourceToken token)
        {
            token.Release(this);
        }
    }
}