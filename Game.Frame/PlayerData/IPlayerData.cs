using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game.Frame.PlayerData
{
    public interface IPlayerData
    {
        /// <summary>
        /// 是否为脏数据
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// 文件信息
        /// </summary>
        PlayerDataInfo DataInfo { get; }

        /// <summary>
        /// 读取后，进行反序列化
        /// </summary>
        void AfterRead();

        /// <summary>
        /// 写入前，序列化
        /// </summary>
        void BeforeWrite();
    }
}