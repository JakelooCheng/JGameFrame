using System;

namespace Game.Frame.Coroutine
{
    public struct AwaitUntil : ICoroutineAwaiter
    {
        public bool IsDone => func == null || func();

        private readonly Func<bool> func;

        public AwaitUntil(Func<bool> func)
        {
            this.func = func;
        }
    }
}