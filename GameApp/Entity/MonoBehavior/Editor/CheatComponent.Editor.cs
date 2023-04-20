using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameApp.Entitys;
using GameApp.Cheat;
using System;
using GameApp.Quest;

namespace GameApp.Entity.Editor
{
    public enum DebugType
    { 
        作弊 = 0,
        任务 = 1,
        Count = 2
    }

    [CustomEditor(typeof(CheatComponent))]
    public class CheatComponentEditor : UnityEditor.Editor
    {
        private List<CheatData> curShowCheatList = new List<CheatData>();

        private string[] tabNameList;
        private string[] debugNameList;

        private int tabSelectIndex = 0;
        private int debugSelectIndex = 0;

        public override void OnInspectorGUI()
        {
            InitTabs();
            DrawDebug(); // 绘制调试信息
        }

        private void InitTabs()
        {
            if (tabNameList != null && debugNameList != null)
            {
                return;
            }

            var tabList = new List<string>();
            for (int index = 0; index < (int)DebugType.Count; index++)
            {
                tabList.Add(((DebugType)index).ToString());
            }
            debugNameList = tabList.ToArray();
            tabList.Clear();
            for (int index = 0; index < (int)CheatFuncType.Count; index ++)
            {
                tabList.Add(((CheatFuncType)index).ToString());
            }
            tabNameList = tabList.ToArray();
        }

        /// <summary>
        /// 绘制页签
        /// </summary>
        private void DrawDebug()
        {
            tabSelectIndex = GUILayout.Toolbar(tabSelectIndex, debugNameList);
            switch ((DebugType)tabSelectIndex)
            {
                case DebugType.作弊:
                    EditorGUILayout.BeginVertical("frameBox");
                    DrawCheatTabs(); // 绘制页签
                    DrawCheatDatas(); // 作弊指令
                    EditorGUILayout.EndVertical();
                    break;
                case DebugType.任务:
                    DrawDebugQuest();
                    break;
            }
        }

        /// <summary>
        /// 绘制页签
        /// </summary>
        private void DrawCheatTabs()
        {
            var comp = target as CheatComponent;
            tabSelectIndex = GUILayout.Toolbar(tabSelectIndex, tabNameList);

            /// 构建页签列表
            if (!comp.CheatDatasByType.TryGetValue((CheatFuncType)tabSelectIndex, out var list))
            {
                list = new List<CheatData>();
            }
            curShowCheatList = list;
        }

        /// <summary>
        /// 绘制作弊指令
        /// </summary>
        private void DrawCheatDatas()
        {
            if (curShowCheatList == null)
            {
                return;
            }

            foreach (var data in curShowCheatList)
            {
                EditorGUILayout.BeginHorizontal();
                var result = DrawParam(data);
                if (GUILayout.Button(data.Attribute.Name, GUILayout.Width(100)))
                {
                    data.DoCheat(result);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// 绘制参数列表
        /// </summary>
        private object[] DrawParam(CheatData cheatData)
        {
            GUILayout.Space(10);
            object[] result = new object[cheatData.Parameters.Length];
            for (int index = 0; index < cheatData.Parameters.Length; index ++)
            {
                var par = cheatData.Parameters[index];
                object args = null;
                if (par.ParameterType == typeof(int)) // Int
                {
                    cheatData.ParametersInputs[index] = EditorGUILayout.TextArea(cheatData.ParametersInputs[index]);
                    int arg = int.Parse(cheatData.ParametersInputs[index]);
                    args = arg;
                }
                else if (par.ParameterType == typeof(string)) // String
                {
                    cheatData.ParametersInputs[index] = EditorGUILayout.TextArea(cheatData.ParametersInputs[index]);
                    args = cheatData.ParametersInputs[index];
                }
                else if (par.ParameterType.IsEnum) // Enum
                {
                    var enumArg = Enum.Parse(par.ParameterType, cheatData.ParametersInputs[index]);
                    args = EditorGUILayout.EnumPopup(enumArg as Enum, GUILayout.Width(100));
                    cheatData.ParametersInputs[index] = args.ToString();
                }

                result[index] = args;
            }

            return result;
        }

        /// <summary>
        /// 绘制任务调试信息
        /// </summary>
        private void DrawDebugQuest()
        {
            if (QuestManager.Instance == null)
            {
                EditorGUILayout.HelpBox("运行中使用", MessageType.Info);
                return;
            }
        }
    }
}
