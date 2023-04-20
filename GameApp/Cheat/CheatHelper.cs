using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Base.Logs;

namespace GameApp.Cheat
{
    public static class CheatHelper
    {
        /// <summary>
        /// 初始化所有作弊类型
        /// </summary>
        public static void GetCheatFuncList(List<CheatData> result)
        {
            MethodInfo[] methods = typeof(CheatManager).GetMethods(BindingFlags.Instance | BindingFlags.Public);
            if (methods == null)
            {
                return;
            }

            foreach (MethodInfo method in methods)
            {
                var attr = method.GetCustomAttribute(typeof(CheatAttribute)) as CheatAttribute;
                if (attr != null)
                {
                    result.Add(CheatData.ToCheatData(method, attr));
                }
            }
        }
    }

    /// <summary>
    /// 作弊数据
    /// </summary>
    public class CheatData
    {
        public CheatAttribute Attribute;

        public MethodInfo Method;

        /// <summary>
        /// 传参类型
        /// </summary>
        public ParameterInfo[] Parameters;

        public string[] ParametersInputs;

        public static CheatData ToCheatData(MethodInfo method, CheatAttribute attr)
        {
            var cheatData = new CheatData()
            {
                Attribute = attr,
                Method = method,
                Parameters = method.GetParameters(),
                ParametersInputs = new string[method.GetParameters().Length]
            };

            for (int index = 0; index < cheatData.Parameters.Length; index ++)
            {
                var par = cheatData.Parameters[index];
                if (par.ParameterType == typeof(int)) // Int
                {
                    cheatData.ParametersInputs[index] = "0";
                }
                else if (par.ParameterType == typeof(string)) // String
                {
                    cheatData.ParametersInputs[index] = cheatData.Parameters[index].Name;
                }
                else if (par.ParameterType.IsEnum)
                {
                    cheatData.ParametersInputs[index] = "0";
                }
            }

            return cheatData;
        }

        /// <summary>
        /// 执行作弊
        /// </summary>
        /// <param name="parameters"></param>
        public void DoCheat(object[] parameters)
        {
            if (Method != null)
            {
                Method.Invoke(CheatManager.Instance, parameters);
            }
        }
    }

    public enum CheatFuncType
    {
        全部 = 0,
        其他 = 1,
        Count = 2,
    }

    /// <summary>
    /// 作弊函数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CheatAttribute : Attribute
    {
        public string Name { get; set; }

        public string[] Args { get; set; }

        public CheatFuncType Type { get; set; }


        public CheatAttribute(string name, CheatFuncType type, params string[] args)
        {
            this.Name = name;
            this.Type = type;
            this.Args = args;
        }
    }
}