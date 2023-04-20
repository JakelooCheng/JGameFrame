namespace Game.Frame.Timer
{
    public interface ITimer
    {
        /// <summary>
        /// 是否无效
        /// </summary>
        bool IsInvalid { get; }

        /// <summary>
        /// 记时器id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        bool Pause { get; set; }

        /// <summary>
        /// 循环次数，当 小于等于 0 时，表示无限循环
        /// </summary>
        long LoopTime { get; }

        /// <summary>
        /// 已循环次数
        /// </summary>
        long LoopedTime { get; }

        /// <summary>
        /// 循环间隔
        /// </summary>
        int IntervalMS { get; }

        /// <summary>
        /// 运行时长
        /// </summary>
        float RunDurationMS { get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        float StartMS { get; }

        /// <summary>
        /// 是否是被时间缩放影响的计时器
        /// </summary>
        bool IsScaled { get; }
    }
}