#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Extern.QuickUI
{
    public class UIPrefabInfo
    {
        #region 项目配置
        /// <summary>
        /// 预制体类型
        /// </summary>
        public enum UIPrefabType
        {
            Window = 0, // 窗口
            Fragment = 1, // Fragment
            Item = 2 // 小组件
        }

        /// <summary>
        /// 项目配置
        /// </summary>
        private static class Setting
        {
            // UI Prefab 所在目录
            public static readonly string PrefabPath = "Assets/Resources/UI/Prefabs";
            // UI Script 所在目录
            public static readonly string BaseScriptPath = "Assets/Scripts/GameApp/UI/Windows";
            // Sprites 目录
            public static readonly string BaseSpritePath = "Assets/Resources/UI/Atlas";
        }
        #endregion

        public string ScriptName; // 脚本基础名
        public string PrefabName; // 预制体名
        public string ClipPath; // 脚本所在目录
        public string NormalScriptPath; // 正常脚本位置
        public string ComponentScriptPath; // .Components 脚本位置
        public string ModuleName; // 模块名，用于 namespace
        public string AssetPath; // 资源所在目录
        public UIPrefabType PrefabType; // UI 预制体类型

        public UnityEngine.Object NormalScriptObject; // 脚本 Object
        public UnityEngine.Object ComponentScriptObject; // .Components 脚本 Object

        /// <summary>
        /// 是否是快速绑定生成的
        /// </summary>
        public bool IsCreateByQuickUI = true;
        public int ScriptHashCode = 0;


        public static UIPrefabInfo Make(GameObject from)
        {
            var prefabPath = GetPrefabPath(from);
            if (prefabPath == null)
            {
                Debug.LogError("QuickUI Error 生成 PrefabInfo 失败，必须对预制体操作！");
                return null;
            }

            string[] pathArray = prefabPath.Split(new string[] { Setting.PrefabPath }, System.StringSplitOptions.None);
            if (pathArray.Length > 1)
            {
                try
                {
                    UIPrefabInfo result = new UIPrefabInfo();
                    var realPath = pathArray[1];
                    pathArray = realPath.Split('/');
                    // Window 还是 Fragment 还是 Item
                    result.PrefabType = realPath.IndexOf("Frag") > 0 ? UIPrefabType.Fragment : (realPath.IndexOf("Item") > 0 ? UIPrefabType.Item : UIPrefabType.Window);
                    // 前置路径
                    string realName = pathArray[pathArray.Length - 1];
                    result.ClipPath = realPath.Split(new string[] { realName }, System.StringSplitOptions.None)[0];
                    // 模块名字
                    result.ModuleName = pathArray[1].Split('.')[0];
                    // 资源路径路径
                    result.AssetPath = "UI/Prefabs" + realPath.Split('.')[0];
                    // 重构后的名字
                    string scriptName = realName.Split('.')[0];
                    scriptName = scriptName.Replace("Window", "");
                    scriptName = scriptName.Replace("Fragment", "");
                    scriptName = scriptName.Replace("Frag", "");
                    scriptName = scriptName.Replace("Item", "");

                    // 构建名字
                    result.ScriptName = scriptName + (result.PrefabType == UIPrefabType.Window ? "Window" : (result.PrefabType == UIPrefabType.Item ? "Item" : "Fragment"));
                    result.PrefabName = scriptName + (result.PrefabType == UIPrefabType.Window ? "Window" : (result.PrefabType == UIPrefabType.Item ? "Item" : "Frag"));

                    // 绑定脚本路径
                    result.NormalScriptPath = Setting.BaseScriptPath + result.ClipPath + result.ScriptName + ".cs";
                    result.ComponentScriptPath = Setting.BaseScriptPath + result.ClipPath + result.ScriptName + ".Components.cs";

                    result.NormalScriptObject = AssetDatabase.LoadAssetAtPath(result.NormalScriptPath, typeof(UnityEngine.Object));
                    result.ComponentScriptObject = AssetDatabase.LoadAssetAtPath(result.ComponentScriptPath, typeof(UnityEngine.Object));

                    return result;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"QuickUI Error 生成 PrefabInfo 失败，路径或名称出错 {ex}");
                }
            }
            else
            {
                Debug.LogError("QuickUI Error 生成 PrefabInfo 失败，选择的 Prefab 不在 UI 路径内！");
            }
            return null;
        }

        /// <summary>
        /// 获取预制体的真实路径
        /// </summary>
        /// <param name="from">任意位置的 GameObject</param>
        /// <returns>真实路径</returns>
        public static string GetPrefabPath(GameObject from)
        {
            // Project 中的预制体
            if (PrefabUtility.IsPartOfPrefabAsset(from))
            {
                return AssetDatabase.GetAssetPath(from);
            }

            // Scene 中的预制体，禁用变体
            if (PrefabUtility.IsPartOfPrefabInstance(from))
            {
                // 判断自身是否是实例
                if (PrefabUtility.IsAnyPrefabInstanceRoot(from))
                {
                    var prefabAsset = PrefabUtility.GetCorrespondingObjectFromOriginalSource(from);
                    return AssetDatabase.GetAssetPath(prefabAsset);
                }
            }

            // PrefabMode 中的预制体
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(from);
            if (prefabStage != null)
            {
                return prefabStage.assetPath;
            }

            return null;
        }

        /// <summary>
        /// 判断预制体是否需要保存
        /// </summary>
        /// <returns></returns>
        public static bool IsDirtyPrefab(GameObject from)
        {
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(from);
            if (prefabStage != null)
            {
                return prefabStage.scene.isDirty;
            }
            else
            {
                var propertyModifications = PrefabUtility.GetObjectOverrides(from);
                if (propertyModifications.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif