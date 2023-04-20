using Game.Frame;
using Game.Frame.Timer;
using Game.Frame.UI;
using GameApp;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    public enum TUITransition
    {
        DipToBlack = 0, // 转到黑场
        DipToImage = 1 // 转到图片（一般是任务用）
    }

    public partial class CommonUITransitionWindow
    {
        public static bool IsOnTransition = false; // 是否有未关闭的转场

        private Dictionary<TUITransition, Transition> transitionDic = new Dictionary<TUITransition, Transition>();


        protected override void OnCreate()
        {
            InitComponents();

            transitionDic.Add(TUITransition.DipToBlack, new Transition(new RawImageTransition(), trDipToBlackTransition));
            transitionDic.Add(TUITransition.DipToImage, new Transition(new RawImageTransition(), trDipToImageTransition));
        }

        protected override void OnDestroy()
        {
            foreach (var trans in transitionDic.Values)
            {
                trans.Shutdown();
            }
            transitionDic.Clear();
        }

        protected override void OnShow()
        {

        }

        #region 转场调用接口
        /// <summary>
        /// 转场入
        /// </summary>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        /// <param name="afterIn"></param>
        public void TransIn(TUITransition type, float duration = 1000, Action afterIn = null, object args = null)
        {
            if (transitionDic.TryGetValue(type, out var tran))
            {
                tran.Entry(duration, afterIn, args);
                IsOnTransition = true;
            }
        }

        /// <summary>
        /// 转场出
        /// </summary>
        /// <param name="type"></param>
        /// <param name="duration">持续时间</param>
        /// <param name="afterOut">完成回调</param>
        public void TransOut(TUITransition type, float duration = 1000, Action afterOut = null)
        {
            if (transitionDic.TryGetValue(type, out var tran))
            {
                tran.Exit(duration, afterOut);
                IsOnTransition = false;
            }
        }

        /// <summary>
        /// 转场持续一段时间后自动退出
        /// </summary>
        public void TransWait(TUITransition type, float durationIn = 1000, float durationWait = 1000, float durationOut = 1000, Action afterCallback = null)
        {
            TransIn(type, durationIn, () =>
            {
                TransOutWait(type, durationWait, durationOut, afterCallback);
            });
        }

        /// <summary>
        /// 一段时间后自动退出转场
        /// </summary>
        public void TransOutWait(TUITransition type, float durationWait = 1000, float durationOut = 1000, Action afterCallback = null)
        {
            AppFacade.Timer.Wait((int)durationWait, isScaled: false, () =>
            {
                TransOut(type, durationOut, afterCallback);
            });
        }
        #endregion

        #region 转场管理类
        private class Transition
        {
            private ITimer updateTimer;
            private ITimer UpdateTimer
            {
                set
                {
                    if (updateTimer != null)
                    {
                        AppFacade.Timer.Cancel(ref updateTimer);
                        updateTimer = value;
                        AfterEntry?.Invoke();
                        AfterExit?.Invoke();
                        AfterEntry = null;
                        AfterExit = null;
                    }
                    else
                    {
                        updateTimer = value;
                    }
                }
            }

            private Transform Root { get; set; }

            private ITransitionEffect Effect { get; set; }

            private Action AfterEntry { get; set; }
            private Action AfterExit { get; set; }

            public void Shutdown()
            {
                AfterEntry = null;
                AfterExit = null;
                UpdateTimer = null;
            }

            public Transition(ITransitionEffect effect, Transform root)
            {
                Root = root;
                Root.SetUIActive(false);
                effect.Init(Root);
                Effect = effect;
            }

            public void Entry(float duration, Action afterEntry, object args)
            {
                Effect.BeforeEntry(args);
                UpdateTimer = AppFacade.Timer.Frame(timer =>
                {
                    float progress = timer.RunDurationMS / duration;
                    if (progress > 1)
                    {
                        progress = 1;
                        UpdateTimer = null;
                    }
                    Effect.OnEntry(progress);
                }, isScaled: false);
                Root.SetUIActive(true);
                AfterEntry = afterEntry;
            }

            public void Exit(float duration, Action afterExit)
            {
                UpdateTimer = AppFacade.Timer.Frame(timer =>
                {
                    float progress = 1 - timer.RunDurationMS / duration;
                    if (progress < 0)
                    {
                        progress = 0;
                        Root.SetUIActive(false);
                        UpdateTimer = null;
                    }
                    Effect.OnExit(progress);
                }, isScaled: false);
                AfterExit = afterExit;
            }
        }
        #endregion

        #region 转场效果实现
        /// <summary>
        /// 转场接口
        /// </summary>
        private interface ITransitionEffect
        {
            public abstract void Init(Transform root);

            public abstract void BeforeEntry(object args);

            public abstract void OnEntry(float progress); // 进入动画每帧调用

            public abstract void OnExit(float progress); // 推出动画每帧调用
        }

        /// <summary>
        /// 使用 RawImage 做的转场
        /// </summary>
        private class RawImageTransition : ITransitionEffect
        {
            protected RawImage blockedImage;

            public virtual void Init(Transform root)
            {
                blockedImage = root.GetComponent<RawImage>();
            }

            public virtual void BeforeEntry(object args)
            {
                string path = args as string;
                SetRawImage(path);
            }

            public void SetRawImage(string path)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    // CommonUtil.SetRawImageSync(blockedImage, ResourceUtility.GetCommonBackgroundPath(path));
                    blockedImage.color = Color.white;
                }
                else
                {
                    blockedImage.texture = null;
                    blockedImage.color = Color.clear;
                }
            }

            public virtual void OnEntry(float progress)
            {
                Color color = blockedImage.color;
                color.a = progress;
                blockedImage.color = color;
            }

            public virtual void OnExit(float progress)
            {
                Color color = blockedImage.color;
                color.a = progress;
                blockedImage.color = color;
            }
        }
        #endregion
    }
}