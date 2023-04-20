using Game.Base.Logs;

namespace GameApp.Cheat
{
    /// <summary>
    /// 其他
    /// </summary>
    public partial class CheatManager
    {
        [Cheat("打印日志", CheatFuncType.其他)]
        public void CheatShowLog(string 日志, int 数字, CheatFuncType 枚举)
        {
            Log.Error(日志);
            Log.Error(数字.ToString());
            Log.Error(枚举.ToString());
        }
    }
}