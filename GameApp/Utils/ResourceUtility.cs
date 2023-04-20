namespace GameApp.Utils
{
    /// <summary>
    /// 资源相关
    /// </summary>
    public static class ResourceUtility
    {
        /// <summary>
        /// 获取表格路径
        /// </summary>
        public static string GetTablePath(string subPath)
        {
            return $"TableData/{subPath}";
        }

        /// <summary>
        /// 获取模型路径
        /// </summary>
        /// <returns></returns>
        public static string GetModelPath(string subPath)
        {
            return $"Art/Prefabs/{subPath}";
        }
    }
}