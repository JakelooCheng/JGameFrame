using Game.Base.LifeCycle;
using Game.Base.Logs;
using Game.Base.Module;
using Game.Frame.Timer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Game.Frame.PlayerData
{
    public class PlayerDataInfo
    {
        public string Name;

        public DateTime SavedTime;

        public FileInfo RealFile;
    }

    /// <summary>
    /// 玩家数据保存
    /// </summary>
    public class PlayerDataManager : GameModuleBase, IInitAfter, IShutdown
    {
        /// <summary>
        /// 当前选择的玩家
        /// </summary>
        public PlayerData Player { get; private set; }

        /// <summary>
        /// 存档根目录
        /// </summary>
        private static readonly string rootPath = Application.streamingAssetsPath + "/PlayerData/";

        private ITimer updateTimer;


        public void InitAfter()
        {
            updateTimer = AppFacade.Timer.Run(1000, 0, OnIntervalUpdate);
        }

        /// <summary>
        /// 间隔更新
        /// </summary>
        private void OnIntervalUpdate()
        {
            if (Player != null && Player.IsDirty)
            {
                Player.IsDirty = false;
                try
                {
                    Save(Player);
                    Log.Info("PlayerDataManager Debug 已自动保存数据。");
                }
                catch (Exception ex)
                {
                    Log.Error($"PlayerDataManager Error 自动保存数据出错，{ex}");
                }
            }
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        public void GetAllInfos(List<PlayerDataInfo> result)
        {
            DirectoryInfo direction = new DirectoryInfo(rootPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.EndsWith(".json"))
                {
                    continue;
                }
                PlayerDataInfo dataInfo = new PlayerDataInfo();
                dataInfo.Name = files[i].Name.Split('.')[0];
                dataInfo.SavedTime = files[i].LastWriteTime;
                dataInfo.RealFile = files[i];
                result.Add(dataInfo);
            }
        }

        /// <summary>
        /// 用路径信息获取详细内容
        /// </summary>
        /// <param name="dataInfo"></param>
        public PlayerData Get(PlayerDataInfo dataInfo)
        {
            try
            {
                var file = dataInfo.RealFile.OpenText();
                var json = file.ReadToEnd();
                file.Close();
                var data = JsonConvert.DeserializeObject<PlayerData>(json);
                data.AfterRead();
                data.DataInfo = dataInfo;
                return data;
            }
            catch (Exception ex)
            {
                Log.Error($"PlayerDataManager Error 获取文件出错 {ex}");
                return null;
            }
        }

        /// <summary>
        /// 开始游戏的入口
        /// </summary>
        /// <param name="dataInfo"></param>
        public void StartGame(PlayerDataInfo dataInfo)
        {
            Player = Get(dataInfo);
        }

        /// <summary>
        /// 结束游戏的入口
        /// </summary>
        public void QuitGame()
        {
            Player = null;
        }

        /// <summary>
        /// 创建存档
        /// </summary>
        public PlayerData Create(string name)
        {
            var data = new PlayerData();
            data.IsDirty = true;
            data.PlayerName = name;
            var dataInfo = new PlayerDataInfo();
            dataInfo.Name = name;
            dataInfo.SavedTime = DateTime.Now;
            dataInfo.RealFile = new FileInfo(rootPath + $"{name}.json");
            data.DataInfo = dataInfo;
            Save(data);
            return data;
        }

        /// <summary>
        /// 删除游戏数据
        /// </summary>
        public void Delete(PlayerDataInfo playerData)
        {
            playerData.RealFile.Delete();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save(PlayerData playerData)
        {
            playerData.BeforeWrite();
            string json = JsonConvert.SerializeObject(playerData);
            var file = playerData.DataInfo.RealFile;
            using (FileStream writer = file.Open(FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                writer.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 异步保存
        /// </summary>
        public CancellationToken SaveAsync(PlayerData playerData, Action callback)
        {
            var token = new CancellationToken();
            playerData.BeforeWrite();
            string json = JsonConvert.SerializeObject(playerData);
            var file = playerData.DataInfo.RealFile;
            using (var writer = file.OpenWrite())
            {
                Encoder e = Encoding.UTF8.GetEncoder();
                var byData = new byte[json.Length * 2];
                e.GetBytes(json.ToCharArray(), 0, json.Length, byData, 0, true);
                writer.WriteAsync(byData, token);
            }
            return token;
        }

        public void Shutdown()
        {
            if (updateTimer != null)
            {
                AppFacade.Timer.Cancel(ref updateTimer);
            }
        }
    }
}