using System;

namespace Game.Frame.UI
{
    public enum LayerType
    {
        HUD = 0,    // HUD信息
        Form = 1,   // 常驻窗口
        Dialog = 2, // 次级窗口
        Toast = 3,  // 系统提示
        Top = 4,    // 最高级
        Count = 5   // 统计计数
    }

    public enum ClearMode
    { 
        /// <summary>
        /// 默认（一段时间后被回收）
        /// </summary>
        Default,

        /// <summary>
        /// 常驻（不回收）
        /// </summary>
        DoNotDestroy,

        /// <summary>
        /// 不可见时直接销毁
        /// </summary>
        DestroyOnHide
    }

    public enum WindowMode
    {
        /// <summary>
        /// 互斥窗口
        /// </summary>
        Exclusive,

        /// <summary>
        /// 共存窗口
        /// </summary>
        Inclusive,

        /// <summary>
        /// 常驻窗口
        /// </summary>
        Permanent
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UIElementAttribute : Attribute
    { 
        public string AssetPath { get; private set; }

        public UIElementAttribute(string assetPath)
        {
            this.AssetPath = assetPath;
        }
    }

    /// <summary>
    /// UIWindow 数据
    /// </summary>
    public sealed class UIWindowAttribute : UIElementAttribute
    {
        /// <summary>
        /// 所在层级
        /// </summary>
        public LayerType LayerType { get; }

        /// <summary>
        /// 驻留类型
        /// </summary>
        public WindowMode WindowMode { get; }

        /// <summary>
        /// 回收方式
        /// </summary>
        public ClearMode ClearMode { get; }


        public UIWindowAttribute(string assetPath, LayerType layerType, WindowMode windowMode, ClearMode clearMode) : base (assetPath)
        {
            LayerType = layerType;
            WindowMode = windowMode;
            ClearMode = clearMode;
        }
    }
}