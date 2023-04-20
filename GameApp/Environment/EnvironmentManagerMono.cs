using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace GameApp.Environment
{
    public class EnvironmentManagerMono : MonoBehaviour
    {
        [Title("全局参数")]
        [Range(0, 1)]
        public float CurRealTime = 0;

        [Title("组件绑定")]
        public Light GlobalLight;

        [Title("变换范围")]
        public Vector2 LightTempRange;

        public Vector2 LightIntensityRange;

        public void Update()
        {
            OnUpdate();
        }

        public void OnUpdate()
        {
            /// 昼夜变换参数
            float curTime2X = (CurRealTime < 0.5f ? CurRealTime : 1 - CurRealTime) * 2;
            float curTime4X = (curTime2X < 0.5f ? curTime2X : 1 - curTime2X) * 2;

            GlobalLight.colorTemperature = Mathf.Lerp(LightTempRange.x, LightTempRange.y, curTime2X);
            GlobalLight.intensity = Mathf.Lerp(LightIntensityRange.x, LightIntensityRange.y, curTime4X);
        }
    }

    [CustomEditor(typeof(EnvironmentManagerMono))]
    public class EnvironmentManagerEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            (target as EnvironmentManagerMono).OnUpdate();
        }
    }
}
