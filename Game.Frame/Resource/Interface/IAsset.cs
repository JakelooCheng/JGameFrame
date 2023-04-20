using System;


namespace Game.Frame.Resource.Interface
{
    public interface IAsset
    {
        /// <summary>
        /// 资源唯一id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 资源路径
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 实际资源
        /// </summary>
        UnityEngine.Object RealAsset { get; }
    }
}
