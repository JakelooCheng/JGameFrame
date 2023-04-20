using Game.Frame.Resource.Interface;
using Game.Base.ObjectPool;
using System;

namespace Game.Frame.Resource
{
    public class ResourceToken : IResourceToken
    {
        private PoolList<IAsset> assetList;

        public bool IsCancel { get; private set; }

        public event Action OnCancel;

        public void Init()
        {
            assetList = PoolList<IAsset>.Get();
            IsCancel = false;
            OnCancel = null;
        }

        public void Cancle()
        {
            IsCancel = true;
            OnCancel?.Invoke();
        }

        public void AddAsset(IAsset asset)
        {
            assetList.Add(asset);
        }

        public void Release(IResourceManager manager)
        {
            Cancle();

            foreach (var asset in assetList)
            {
                manager.UnloadAsset(asset);
            }
            assetList.Dispose();
            assetList = null;

            GlobalObjectPool<ResourceToken>.Release(this);
        }

        public static ResourceToken Make()
        {
            ResourceToken token = GlobalObjectPool<ResourceToken>.Get();
            token.Init();
            return token;
        }
    }
}
