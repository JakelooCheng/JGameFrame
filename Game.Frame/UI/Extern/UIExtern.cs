using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Base.Data;
using System.Runtime.CompilerServices;

namespace Game.Frame.UI
{
    public static class UIExtern
    {
        private static readonly Vector3 hidenScale = new Vector3(0, 0.001f, 0);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUIActive(this GameObject gameObject, bool isActive)
        {
            SetUIActive(gameObject.transform, isActive);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUIActive(this Component component, bool isActive)
        {
            SetUIActive(component.transform, isActive);
        }

        /// <summary>
        /// ͨ�����ſ��� UI ����ʾ���أ�����ֱ������ Active ���µ���������
        /// </summary>
        public static void SetUIActive(Transform transform, bool isActive)
        {
            var scale = transform.localScale;

            //TODO ��ʵ��
            transform.gameObject.SetActive(isActive);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUIActive(this GameObject gameObject)
        {
            return IsUIActive(gameObject.transform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUIActive(this Component component)
        {
            return component.transform.localScale.x != hidenScale.x && component.transform.gameObject.activeSelf;
        }

        /// <summary>
        /// ���ϲ��Ҹ��ڵ��Ƿ� Active
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUIActiveInHierarchy(this Transform transform)
        {
            while (transform != null)
            {
                if (!transform.IsUIActive())
                {
                    return false;
                }
                transform = transform.parent;
            }
            return true;
        }
    }
}