using System.Collections;

namespace Game.Frame.Coroutine
{
    public class CoroutineNode
    {
        public CoroutineNode ListPrevious = null;
        public CoroutineNode ListNext = null;
        public readonly IEnumerator Fiber;
        public bool Finished = false;

        public ICoroutineAwaiter Awaiter;
        public bool Pausing = false;

        public CoroutineNode(IEnumerator fiber)
        {
            this.Fiber = fiber;
        }
    }
}