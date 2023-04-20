using Game.Frame;
using Game.Frame.Timer;
using Game.Frame.UI;
using GameApp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    public partial class ClickWindow
    {
        public class ClickPoint
        {
            public Transform Root { get; set; }
            public ParticleSystem[] pointEffect;

            public static ClickPoint Clone(Transform root)
            {
                ParticleSystem[] pointEffect = new ParticleSystem[1];
                for (int index = 0; index < root.childCount; index++)
                {
                    pointEffect[index] = root.GetChild(index).GetComponent<ParticleSystem>();
                    var mainParticalMoudle = pointEffect[index].main;
                    mainParticalMoudle.useUnscaledTime = true; // 设置不受 TimeScale 影响
                }
                return new ClickPoint
                {
                    Root = root,
                    pointEffect = pointEffect
                };
            }
        }

        private ITimer updateTimer;

        private static readonly int maxCount = 5;
        private int curPoint = 0;
        private bool canShowClick = true;
        private List<ClickPoint> clickPointList = new List<ClickPoint>(maxCount);
        private RectTransform screenRootTr;

        protected override void OnCreate()
        {
            InitComponents();

            for (int i = 0; i < maxCount; i++)
            {
                GameObject point = GameObject.Instantiate(goClickTemp, RectTransform);
                point.SetUIActive(true);
                clickPointList.Add(ClickPoint.Clone(point.transform));
            }

            updateTimer = AppFacade.Timer.Run(0, 0, Update);
        }

        protected override void OnShow()
        {
            screenRootTr = Canvas.rootCanvas.transform.GetComponent<RectTransform>();
        }

        protected override void OnDestroy()
        {
            foreach (ClickPoint clickPoint in clickPointList)
            {
                GameObject.Destroy(clickPoint.Root.gameObject);
            }
            clickPointList.Clear();

            AppFacade.Timer.Cancel(ref updateTimer);
            updateTimer = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Click(Input.mousePosition);
            }

            // 防止按住的情况
            if (Input.touchCount > 0)
            {
                if (canShowClick)
                    Click(Input.GetTouch(0).position);
                canShowClick = false;
            }
            else
            {
                canShowClick = true;
            }
        }

        private void Click(Vector2 clickPos)
        {
            ClickPoint point = clickPointList[curPoint++ % maxCount];
            if (curPoint > maxCount)
            {
                curPoint = 0;
            }

            float X = screenRootTr.sizeDelta.x * (clickPos.x / Screen.width - 0.5f);
            float Y = screenRootTr.sizeDelta.y * (clickPos.y / Screen.height - 0.5f);

            foreach (ParticleSystem effect in point.pointEffect)
            {
                effect.Stop();
                effect.Play();
            }
            point.Root.localPosition = new Vector2(X, Y);
        }
    }
}