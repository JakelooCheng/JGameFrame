using Game.Base.Logs;
using TableData;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GameApp.Entitys;
using Game.Base.EC;

namespace GameApp.Map
{
    /// <summary>
    /// 游戏逻辑场景
    /// </summary>
    public partial class MapScene
    {
        public MapConfig MapConfig { get; private set; }

        private Scene scene;

        public Transform Root { get; private set; }

        public CameraMovementMono CameraMovement { get; private set; }

        public EntityManager RunTimeEntitys { get; private set; }

        public static MapScene Make(int mapId)
        {
            var mapScene = new MapScene();
            mapScene.MapConfig = MapConfigManager.Instance.GetItem(mapId);
            if (mapScene.MapConfig == null)
            {
                Log.Error($"MapScene Error at mapId {mapId}");
            }
            return mapScene;
        }
    }
}