namespace Game.Base.LifeCycle
{
    /// <summary>
    /// 考虑回收到池时调用
    /// </summary>
    public interface IClear
    {
        void Clear();
    }
}