using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
using static Game.Extern.QuickUI.UIPrefabInfo;

namespace Game.Extern.QuickUI.Editor
{
    public static partial class QuickUIHelper
    {
        /// <summary>
        /// UI Script 模板目录
        /// </summary>
        public static readonly string BaseTempPath = "Assets/Scripts/Game.Extern/QuickUI/Editor/TemplateScript/";

        private static readonly Dictionary<UIPrefabType, (string, string)> templates = new Dictionary<UIPrefabType, (string, string)>()
        {
            { UIPrefabType.Window, ("WindowTemplate.txt", "WindowComponentTemplate.txt") },
            { UIPrefabType.Fragment, ("FragmentTemplate.txt", "FragmentComponentTemplate.txt") },
            { UIPrefabType.Item, ("ItemTemplate.txt", "ItemComponentTemplate.txt") },
        };


        #region 同步变量到脚本
        public static void AddToFile(UIPrefabInfo prefabInfo, QuickUIMono component)
        {
            try
            {
                string file = String.Empty;
                using (StreamReader reader = new StreamReader(prefabInfo.ComponentScriptPath))
                {
                    file = reader.ReadToEnd();

                    // 替换变量
                    int startIndex = file.IndexOf("(禁止编辑)自动生成变量");
                    int endIndex = file.IndexOf("#endregion (禁止编辑)自动生成变量");
                    file = file.Remove(startIndex, endIndex - startIndex);
                    file = file.Insert(startIndex, GetVariables(component));

                    // 替换赋值
                    startIndex = file.IndexOf("(禁止编辑)自动生成绑定");
                    endIndex = file.IndexOf("#endregion (禁止编辑)自动生成绑定");
                    file = file.Remove(startIndex, endIndex - startIndex);
                    file = file.Insert(startIndex, GetAssignment(component));
                }

                using (StreamWriter writer = new StreamWriter(prefabInfo.ComponentScriptPath))
                {
                    writer.Write(file);
                    component.PrefabInfo.ScriptHashCode = component.GetHashCode();
                }

                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"QuickUI Error 读取文件出错 {ex}");
            }
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        /// <returns></returns>
        public static string GetVariables(QuickUIMono component, bool needHead = true)
        {
            stringCache.Clear();
            if (needHead)
            {
                // 组装 Hash
                stringCache.Append($"(禁止编辑)自动生成变量\n\t\t// HashCode:{component.GetHashCode()}\n\t\t");
            }
            // 组装变量
            for (int index = 0; index < component.UIObjectsArray.Count; index++)
            {
                stringCache.Append($"private {component.UIObjectsArray[index].GetType()} {component.UIObjectsName[index]};\n\t\t");
            }
            return stringCache.ToString();
        }

        /// <summary>
        /// 获取赋值
        /// </summary>
        public static string GetAssignment(QuickUIMono component, bool needHead = true)
        {
            // 组装赋值
            stringCache.Clear();
            if (needHead)
            {
                stringCache.Append("(禁止编辑)自动生成绑定\n\t\t\t");
            }
            else
            {
                stringCache.Append("QuickUIMono quickUI = RectTransform.GetComponent<QuickUIMono>();\n\t\t\t");
            }
            for (int index = 0; index < component.UIObjectsArray.Count; index++)
            {
                stringCache.Append($"{component.UIObjectsName[index]} = quickUI.UIObjectsArray[{index}] as {component.UIObjectsArray[index].GetType()};\n\t\t\t");
            }
            return stringCache.ToString();
        }
        #endregion

        #region 创建脚本
        /// <summary>
        /// 生成 UI 脚本
        /// </summary>
        public static void CreateUIScript(UIPrefabInfo prefabInfo, int defaultHashCode)
        {
            // 生成 Normal 脚本
            if (GetFile(BaseTempPath + templates[prefabInfo.PrefabType].Item1, out string temp))
            {
                SetFile(prefabInfo.NormalScriptPath, 
                    string.Format(temp,
                    prefabInfo.ModuleName, 
                    prefabInfo.PrefabName));
            }
            // 生成 Components 脚本
            if (GetFile(BaseTempPath + templates[prefabInfo.PrefabType].Item2, out string compTemp))
            {
                SetFile(prefabInfo.ComponentScriptPath, 
                    string.Format(compTemp,
                    prefabInfo.ModuleName, 
                    prefabInfo.AssetPath, 
                    prefabInfo.PrefabName));
            }

            Debug.Log($"QuickUI Log 等待或手动刷新 AssetDatabase");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 写回文件
        /// </summary>
        private static void SetFile(string fullPath, string content)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    Debug.LogWarning($"QuickUI Warning 存在同名脚本 {fullPath}！");
                }

                if (!Directory.Exists(fullPath.Substring(0, fullPath.LastIndexOf('/')))) // 如果路径不存在
                {
                    Directory.CreateDirectory(fullPath.Substring(0, fullPath.LastIndexOf('/')));
                }

                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.Write(content);
                }
                Debug.Log($"QuickUI Log 已创建 {fullPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"QuickUI Error 写入文件出错 {ex}");
            }
        }

        /// <summary>
        /// 读取模板
        /// </summary>
        private static bool GetFile(string fullPath, out string result)
        {
            result = "";
            try
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"QuickUI Error 获取文件出错 {ex}");
            }
            return !string.IsNullOrEmpty(result);
        }

        /// <summary>
        /// 校验 HashCode
        /// </summary>
        public static bool CheckHashCode(string path, out int hashCode)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line.IndexOf("// HashCode:") >= 0)
                        {
                            hashCode = Int32.Parse(line.Replace("// HashCode:", "").Replace("\n", ""));
                            return true;
                        }
                        line = reader.ReadLine();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"QuickUI Error 校验文件出错 {ex}");
            }
            hashCode = -1;
            return false;
        }
        #endregion
    }
}