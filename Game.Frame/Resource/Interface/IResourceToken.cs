using System;

namespace Game.Frame.Resource.Interface
{
    public interface IResourceToken
    {
        /// <summary>
        /// 取消时执行
        /// </summary>
        event Action OnCancel;

        /// <summary>
        /// 是否已取消
        /// </summary>
        bool IsCancel { get; }

        /// <summary>
        /// 取消资源加载
        /// </summary>
        void Cancle();

        /// <summary>
        /// 添加资源
        /// </summary>
        void AddAsset(IAsset asset);

        /// <summary>
        /// 释放资源
        /// </summary>
        void Release(IResourceManager manager);
    }
}