using Codice.CM.Common;
using Game.Frame;
using GameApp.Cache;
using GameApp.Entitys;
using GameApp.Utils;
using System.Collections;
using System.Collections.Generic;
using TableData;
using UnityEngine;

namespace GameApp.Quest
{
    public class DropBehavior : BehaviorBase
    {
        public override void Excute()
        {
            var dropId = BehaviorConfig.IntArg1;

            var dropConfig = DropConfigManager.Instance.GetItem(dropId);
            if (dropConfig != null)
            {
                /// 大弹窗表现
                if (BehaviorConfig.IntArg2 == 0)
                {
                    /// 添加进背包
                    GameCache.Bag.AddItem(dropConfig.Item, dropConfig.Count);
                    /// 弹出弹窗
                    CommonUtility.Reward(dropConfig.Item, dropConfig.Count, Finish);
                }
                /// 一般提示表现
                else
                {
                    var itemConfig = ItemConfigManager.Instance.GetItem(dropConfig.Item);
                    CommonUtility.Toast("获得物品", $"{itemConfig.Name} × {dropConfig.Count}");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}
