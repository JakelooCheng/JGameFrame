using System;


namespace Game.Frame.Resource.Interface
{
    public interface IResourceManager
    {
        /// <summary>
        /// ͬ������
        /// </summary>
        T LoadAsset<T>(string path, IResourceToken token) where T : UnityEngine.Object;

        /// <summary>
        /// ͬ������
        /// </summary>
        IAsset LoadAsset(string path, IResourceToken token);

        /// <summary>
        /// �첽�ص�����
        /// </summary>
        void LoadAsset<T>(string path, Action<IAsset> asset, IResourceToken token);

        /// <summary>
        /// �첽�ص�����
        /// </summary>
        void LoadAsset(string path, Action<IAsset> asset, IResourceToken token);

        /// <summary>
        /// ж����Դ
        /// </summary>
        void UnloadAsset(IAsset asset);

        /// <summary>
        /// �� Token
        /// </summary>
        IResourceToken GetToken();

        /// <summary>
        /// �黹 Token
        /// </summary>
        void ReleaseToken(IResourceToken token);
    }
}
