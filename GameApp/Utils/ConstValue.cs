using UnityEngine;

namespace GameApp.Utils
{
    public static class ConstValue
    {
        /// <summary>
        /// NPC 测距距离
        /// </summary>
        public static readonly float MapNpcMeasureRange = 0.75f;

        /// <summary>
        /// 传送点测距距离
        /// </summary>
        public static readonly float MapTransMeasureRange = 0.8f;

        /// <summary>
        /// 采集物测距距离
        /// </summary>
        public static readonly float MapPickUpMeasureRange = 0.8f;

        /// <summary>
        /// 椅子测距距离
        /// </summary>
        public static readonly float MapChairMeasureRange = 0.5f;

        /// <summary>
        /// Player 移动速度
        /// </summary>
        public static readonly float PlayerMoveSpeed = 0.5f;

        /// <summary>
        /// 交互气泡偏移
        /// </summary>
        public static readonly Vector2 InteractionOffset = new Vector2(100, 100);
        
         /// <summary>
        /// 对话气泡偏移
        /// </summary>
        public static readonly float DialogOffset = 1;
    }
}