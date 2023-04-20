using Game.Base.Module;
using Game.Base.LifeCycle;
using System.Collections;

namespace Game.Frame.Coroutine
{
    public class CoroutineManager : GameModuleBase, IUpdate, IShutdown
    {
        /// <summary>
        /// 当前状态下的协程
        /// </summary>
        private CoroutineScheduler coroutine = new CoroutineScheduler();

        public void OnUpdate()
        {
            coroutine.Update();
        }

        /// <summary>
        /// 开启协程-状态切换自动移除
        /// </summary>
        public CoroutineNode StartCoroutine(IEnumerator fiber)
        {
            return coroutine.StartCoroutine(fiber);
        }

        /// <summary>
        /// 关闭协程
        /// </summary>
        public void RemoveCoroutine(CoroutineNode coroutineNode)
        {
            coroutine.RemoveCoroutine(coroutineNode);
        }

        public void Shutdown()
        {
            coroutine.StopAllCoroutines();
        }
    }
}
