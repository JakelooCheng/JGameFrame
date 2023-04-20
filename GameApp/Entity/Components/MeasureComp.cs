using Game.Base.EC;
using UnityEngine;
using Game.Base.LifeCycle;
using System.Collections.Generic;
using GameApp.Cache;
using Game.Frame.Event;
using System;

namespace GameApp.Entitys
{
    public class MeasureComp : ComponentBase, IInitAfter, IUpdate, IShutdown
    {
        private Vector3 checkPos;

        private MeasureData measureData;

        public InteractionType InteractionType => measureData.InteractionType;

        public bool IsInRange => measureData == null ? false : measureData.IsInRange;

        private bool isActive = true;
        /// <summary>
        /// 设置是否校验
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                /// 关闭测距时强制发送一次消息
                if (value == false)
                {
                    EntityMeasureEvent.FireNow(false, Entity, measureData.InteractionType);
                }
            }
        }

        /// <summary>
        /// 测距状态变化时通知
        /// </summary>
        private Action<bool> onMeasureStateChange;

        public void InitAfter()
        {
            var gameObjectComp = Entity.GetComp<GameObjectComp>();
            if (gameObjectComp != null)
            {
                checkPos = gameObjectComp.Transform.position;
            }
        }

        /// <summary>
        /// 设置原点
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector3 pos)
        {
            checkPos = pos;
        }

        /// <summary>
        /// 添加点对点测距
        /// </summary>
        public void AddMeasureP2P(InteractionType type, float range)
        {
            measureData = new MeasureData(type, range);
        }

        /// <summary>
        /// 添加点对矩形测距
        /// </summary>
        public void AddMeasureP2R(InteractionType type, Rect rangeRect)
        {
            measureData = new MeasureData(type, rangeRect);
        }

        public void OnUpdate()
        {
            if (IsActive)
            {
                CheckRange();
            }
        }

        /// <summary>
        /// 订阅进入回调
        /// </summary>
        public void Subscribe(Action<bool> callback)
        {
            onMeasureStateChange += callback;
            callback?.Invoke(IsInRange);
        }

        /// <summary>
        /// 取消订阅回调
        /// </summary>
        public void UnSubscribe(Action<bool> callback)
        {
            onMeasureStateChange -= callback;
        }

        public void CheckRange()
        {
            var player = GameCache.Player.PlayerEntity;
            if (player == null || measureData == null)
            {
                return;
            }

            var playerPos = player.GetComp<GameObjectComp>().Transform.position;

            // 测距
            bool curIsInRange = false;
            bool lastIsInRange = measureData.IsInRange;
            if (measureData.RangeType == MeasureRangeType.Point2Point)
            {
                var magnitude = (playerPos - checkPos).sqrMagnitude;

                curIsInRange = magnitude < measureData.SqrtRange;
            }
            else if (measureData.RangeType == MeasureRangeType.Point2Rect)
            {
                curIsInRange = measureData.RangeRect.Contains(playerPos);
            }
            measureData.IsInRange = curIsInRange;

            /// 测距结果不一致时进行同步
            if (curIsInRange != lastIsInRange)
            {
                EntityMeasureEvent.FireNow(curIsInRange, Entity, measureData.InteractionType);
                onMeasureStateChange?.Invoke(curIsInRange);
            }
        }

        public void Shutdown()
        {
            // 结束前发离开时间
            EntityMeasureEvent.FireNow(false, Entity, measureData.InteractionType);
            onMeasureStateChange?.Invoke(false);
        }
    }

    public class MeasureData
    {
        public MeasureRangeType RangeType { get; private set; }

        public InteractionType InteractionType { get; private set; }

        private float range;
        /// <summary>
        /// 点到点的检测范围
        /// </summary>
        public float Range
        {
            get => range;
            set
            {
                range = value;
                SqrtRange = range * range;
            }
        }

        /// <summary>
        /// 平方测距距离
        /// </summary>
        public float SqrtRange { get; private set; }

        /// <summary>
        /// 点到矩形的检测范围
        /// </summary>
        public Rect RangeRect { get; private set; }

        /// <summary>
        /// 是否在范围内
        /// </summary>
        public bool IsInRange = false;

        public MeasureData(InteractionType type, float range)
        {
            InteractionType = type;
            RangeType = MeasureRangeType.Point2Point;
            Range = range;
        }

        public MeasureData(InteractionType type, Rect rect)
        {
            InteractionType = type;
            RangeType = MeasureRangeType.Point2Rect;
            RangeRect = rect;
        }
    }

    /// <summary>
    /// 测距方法
    /// </summary>
    public enum MeasureRangeType
    {
        /// <summary>
        /// 点到点
        /// </summary>
        Point2Point = 1,

        /// <summary>
        /// 点到矩形
        /// </summary>
        Point2Rect = 2,
    }
}