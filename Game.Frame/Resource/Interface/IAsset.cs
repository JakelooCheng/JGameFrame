using System;


namespace Game.Frame.Resource.Interface
{
    public interface IAsset
    {
        /// <summary>
        /// ��ԴΨһid
        /// </summary>
        int Id { get; }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        string Path { get; }

        /// <summary>
        /// ʵ����Դ
        /// </summary>
        UnityEngine.Object RealAsset { get; }
    }
}
