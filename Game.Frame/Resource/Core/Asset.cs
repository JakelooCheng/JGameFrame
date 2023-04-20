using Game.Frame.Resource.Interface;
using Game.Base.ObjectPool;
using System;

namespace Game.Frame.Resource.Core
{
    public class Asset : IAsset
    {
        private static int instanceId = 0;

        public int Id { get; private set; }

        public string Path { get; private set; }


        public UnityEngine.Object RealAsset { get; private set; }


        public static Asset Make(string path, UnityEngine.Object realAsset)
        {
            var asset = GlobalObjectPool<Asset>.Get();
            asset.Id = ++instanceId;
            asset.Path = path;
            asset.RealAsset = realAsset;
            return asset;
        }

        public static void Release(Asset asset)
        {
            asset.Id = 0;
            asset.Path = null;
            asset.RealAsset = null;
            GlobalObjectPool<Asset>.Release(asset);
        }
    }
}
