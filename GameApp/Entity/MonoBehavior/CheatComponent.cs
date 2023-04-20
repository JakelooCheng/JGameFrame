using GameApp.Cheat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameApp.Entitys
{
    [ExecuteAlways]
    public class CheatComponent : MonoBehaviour
    {
        /// <summary>
        /// 作弊指令数据
        /// </summary>
        public List<CheatData> CheatDatas = new List<CheatData>();

        /// <summary>
        /// 用作弊类型统计的作弊指令
        /// </summary>
        public Dictionary<CheatFuncType, List<CheatData>> CheatDatasByType = new Dictionary<CheatFuncType, List<CheatData>>();


        private void OnEnable()
        {
            CheatDatas.Clear();
            CheatDatasByType.Clear();

            CheatHelper.GetCheatFuncList(CheatDatas);
            CheatDatasByType.Add(CheatFuncType.全部, CheatDatas);
            foreach (var cheatData in CheatDatas)
            {
                if (!CheatDatasByType.TryGetValue(cheatData.Attribute.Type, out var cheatList))
                {
                    cheatList = new List<CheatData>();
                    CheatDatasByType.Add(cheatData.Attribute.Type, cheatList);
                }
                cheatList.Add(cheatData);
            }
        }
    }
}
