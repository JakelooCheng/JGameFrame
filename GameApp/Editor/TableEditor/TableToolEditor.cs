using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameApp.Entitys;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;

namespace GameApp.Editor
{
    public static class TableToolEditor
    {
        private static readonly string excelToByteCmd = System.Environment.CurrentDirectory + "\\..\\Design\\TableTool\\TableTool.exe";
        private static readonly string excelToByteWorkPath = System.Environment.CurrentDirectory + "\\..\\Design/Excel";
        private static readonly string excelToByteBytesPath = System.Environment.CurrentDirectory + "\\Assets\\Resources\\TableData";

        /// <summary>
        /// 导出所有表格
        /// </summary>
        [MenuItem("TableTool/导出所有表格")]
        public static void ExcuteAllExcel()
        {
            ExcuteExcelToByte();
        }

        private static void ExcuteExcelToByte(params string[] excelNames)
        {
            try
            {
                string args = "-log_encoding UTF-8 -code_type C# ";
                if (excelNames.Length > 0)
                {
                    args += $"-export_tables "; // 添加导出表
                    foreach (var excelName in excelNames)
                    {
                        args += excelName + "|";
                    }
                }
                args += $" -data_path {excelToByteBytesPath} "; // Bytes 路径
                args += $"-work_path {excelToByteWorkPath} "; // Excel 路径
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = excelToByteCmd;
                process.StartInfo.Arguments = args;
                process.StartInfo.WorkingDirectory = excelToByteWorkPath;
                process.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                process.StartInfo.RedirectStandardInput = true; // 接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardOutput = true; // 由调用程序获取输出信息
                process.StartInfo.RedirectStandardError = true; // 重定向标准错误输出
                process.StartInfo.CreateNoWindow = true; // 不显示程序窗口
                EditorUtility.DisplayProgressBar("提示", "重新导出表格...", 0);
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    Debug.Log($"TableToolExcule: {output}");
                }
                process.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
