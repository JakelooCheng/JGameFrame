using System;


namespace Game.Frame.Resource.Interface
{
    public interface IResourceManager
    {
        /// <summary>
        /// 同步加载
        /// </summary>
        T LoadAsset<T>(string path, IResourceToken token) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载
        /// </summary>
        IAsset LoadAsset(string path, IResourceToken token);

        /// <summary>
        /// 异步回调加载
        /// </summary>
        void LoadAsset<T>(string path, Action<IAsset> asset, IResourceToken token);

        /// <summary>
        /// 异步回调加载
        /// </summary>
        void LoadAsset(string path, Action<IAsset> asset, IResourceToken token);

        /// <summary>
        /// 卸载资源
        /// </summary>
        void UnloadAsset(IAsset asset);

        /// <summary>
        /// 拿 Token
        /// </summary>
        IResourceToken GetToken();

        /// <summary>
        /// 归还 Token
        /// </summary>
        void ReleaseToken(IResourceToken token);
    }
}
