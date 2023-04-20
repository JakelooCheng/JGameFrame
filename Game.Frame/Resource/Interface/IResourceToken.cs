using System;

namespace Game.Frame.Resource.Interface
{
    public interface IResourceToken
    {
        /// <summary>
        /// ȡ��ʱִ��
        /// </summary>
        event Action OnCancel;

        /// <summary>
        /// �Ƿ���ȡ��
        /// </summary>
        bool IsCancel { get; }

        /// <summary>
        /// ȡ����Դ����
        /// </summary>
        void Cancle();

        /// <summary>
        /// �����Դ
        /// </summary>
        void AddAsset(IAsset asset);

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        void Release(IResourceManager manager);
    }
}