using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game.Frame.PlayerData
{
    public class KVInt32Int32
    {
        public int Key;
        public int Value;
    }

    public class KVInt32EntityExt
    {
        public int Key;
        public EntityExt Value;
    }

    public class EntityExt
    {
        public EntityState State;
    }

    /// <summary>
    /// 动态物状态
    /// </summary>
    public enum EntityState
    { 
        Default = 0,
        Hide = 1,
        HadInteraction = 2 // 被交互过
    }

    public class PlayerData : IPlayerData
    {
        [JsonIgnore]
        public bool IsDirty { get; set; } = false;

        [JsonIgnore]
        public PlayerDataInfo DataInfo { get; set; }

        #region 游戏内对象（每一个游戏对象对应一个序列化对象）
        [JsonIgnore]
        public string PlayerName;

        [JsonIgnore]
        public int MapId = 100;

        [JsonIgnore]
        public Vector3 PlayerPos = Vector3.zero;

        [JsonIgnore]
        public string PlayerPrefab = "Art/Prefabs/Character/Player";

        [JsonIgnore]
        public Dictionary<int, int> Bag = new Dictionary<int, int>();

        [JsonIgnore]
        public Dictionary<int, int> RunningQuest = new Dictionary<int, int>();

        [JsonIgnore]
        public List<int> FinishedQuest = new List<int>();

        [JsonIgnore]
        public HashSet<int> PickUps = new HashSet<int>();

        [JsonIgnore]
        public Dictionary<int, EntityExt> Entitys = new Dictionary<int, EntityExt>();
        #endregion

        #region 序列化对象（每一个序列化对象对应一个游戏对象）
        [JsonProperty("PlayerName")]
        private string s_PlayerName;

        [JsonProperty("MapId")]
        private int s_MapId;

        [JsonProperty("PlayerPos")]
        private float[] s_PlayerPos;

        [JsonProperty("PlayerPrefab")]
        private string s_PlayerPrefab;

        [JsonProperty("Bag")]
        private List<KVInt32Int32> s_Bag;

        [JsonProperty("Entitys")]
        private List<KVInt32EntityExt> s_Entitys;

        [JsonProperty("RunningQuest")]
        private List<KVInt32Int32> s_RunningQuest;

        [JsonProperty("FinishedQuest")]
        private List<int> s_FinishedQuest;

        [JsonProperty("PickUps")]
        private List<int> s_PickUps;
        #endregion

        #region 序列化和反序列化操作
        /// <summary>
        /// 读取后，进行反序列化
        /// </summary>
        public void AfterRead()
        {
            PlayerName = s_PlayerName;
            MapId = s_MapId;
            PlayerPos = new Vector3(s_PlayerPos[0], s_PlayerPos[1], s_PlayerPos[2]);
            PlayerPrefab = s_PlayerPrefab;
            KVToDic(s_Bag, ref Bag);
            KVToDic(s_RunningQuest, ref RunningQuest);
            FinishedQuest = s_FinishedQuest;
            ListToSet(s_PickUps, ref PickUps);
            KVToDic(s_Entitys, ref Entitys);
        }

        /// <summary>
        /// 写入前，序列化
        /// </summary>
        public void BeforeWrite()
        {
            s_PlayerName = PlayerName;
            s_MapId = MapId;
            s_PlayerPos = new float[] { PlayerPos.x, PlayerPos.y, PlayerPos.z };
            s_PlayerPrefab = PlayerPrefab;
            DicToKV(Bag, ref s_Bag);
            DicToKV(RunningQuest, ref s_RunningQuest);
            s_FinishedQuest = FinishedQuest;
            SetToList(PickUps, ref s_PickUps);
            DicToKV(Entitys, ref s_Entitys);
        }
        #endregion

        #region 序列化转换
        private void DicToKV(Dictionary<int, int> from, ref List<KVInt32Int32> to)
        {
            if (to == null)
            {
                to = new List<KVInt32Int32>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to.Add(new KVInt32Int32
                {
                    Key = f.Key,
                    Value = f.Value
                });
            }
        }

        private void DicToKV(Dictionary<int, EntityExt> from, ref List<KVInt32EntityExt> to)
        {
            if (to == null)
            {
                to = new List<KVInt32EntityExt>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to.Add(new KVInt32EntityExt
                {
                    Key = f.Key,
                    Value = f.Value
                });
            }
        }

        private void KVToDic(List<KVInt32Int32> from, ref Dictionary<int, int> to)
        {
            if (from == null)
            {
                from = new List<KVInt32Int32>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to[f.Key] = f.Value;
            }
        }

        private void KVToDic(List<KVInt32EntityExt> from, ref Dictionary<int, EntityExt> to)
        {
            if (from == null)
            {
                from = new List<KVInt32EntityExt>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to[f.Key] = f.Value;
            }
        }

        private void SetToList(HashSet<int> from, ref List<int> to)
        {
            if (to == null)
            {
                to = new List<int>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to.Add(f);
            }
        }

        private void ListToSet(List<int> from, ref HashSet<int> to)
        {
            if (from == null)
            {
                from = new List<int>();
            }

            to.Clear();
            foreach (var f in from)
            {
                to.Add(f);
            }
        }
        #endregion
    }
}