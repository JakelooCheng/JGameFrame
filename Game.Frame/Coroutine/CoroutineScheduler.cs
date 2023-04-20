using Game.Base.Logs;
using System;
using System.Collections;


namespace Game.Frame.Coroutine
{
    public class CoroutineScheduler
    {
        public event Action<CoroutineNode> OnPreUpdateCoroutine;
        public event Action<CoroutineNode> OnPostUpdateCoroutine;

        //根结点
        private CoroutineNode first = null;

        public CoroutineNode StartCoroutine(IEnumerator fiber)
        {
            // if function does not have a yield, fiber will be null and we no-op
            if (fiber == null)
            {
                return null;
            }
            // create coroutine node and run until we reach first yield
            CoroutineNode coroutine = new CoroutineNode(fiber);
            AddCoroutine(coroutine);
            return coroutine;
        }

        public void StopAllCoroutines()
        {
            first = null;
        }

        public bool HasCoroutines()
        {
            return first != null;
        }

        public void Update()
        {
            CoroutineNode coroutine = first;
            while (coroutine != null)
            {
                // store listNext before coroutine finishes and is removed from the list
                CoroutineNode listNext = coroutine.ListNext;

                if (coroutine.Awaiter == null || coroutine.Awaiter.IsDone)
                {
                    if (coroutine.Pausing == false)
                    {
                        UpdateCoroutine(coroutine);
                    }
                }

                coroutine = listNext;
            }
        }

        private void UpdateCoroutine(CoroutineNode coroutine)
        {
            IEnumerator fiber = coroutine.Fiber;
            bool bMoveNext = false;
            try
            {
                if (OnPreUpdateCoroutine != null)
                {
                    OnPreUpdateCoroutine(coroutine);
                }

                bMoveNext = coroutine.Fiber.MoveNext();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
            finally
            {
                if (OnPostUpdateCoroutine != null)
                {
                    OnPostUpdateCoroutine(coroutine);
                }
            }

            if (bMoveNext && fiber?.Current != null)
            {
                if (fiber.Current is ICoroutineAwaiter curAwaiter)
                {
                    coroutine.Awaiter = curAwaiter;
                }
                else
                {
                    // throw new Exception("awaiter can not be null");
                    Log.Warning($"current awaiter is null: {fiber.Current}");
                }
            }
            else
            {
                // coroutine finished
                coroutine.Finished = true;
                RemoveCoroutine(coroutine);
            }

        }

        //添加协程结点
        private void AddCoroutine(CoroutineNode coroutine)
        {
            if (first != null)
            {
                coroutine.ListNext = first;
                first.ListPrevious = coroutine;
            }
            first = coroutine;
        }

        //移除协程结点
        public void RemoveCoroutine(CoroutineNode coroutine)
        {
            if (first == coroutine)
            {
                // remove first
                first = coroutine.ListNext;
            }
            else
            {
                // not head of list
                if (coroutine.ListNext != null)
                {
                    if (coroutine.ListPrevious == null)
                        throw new Exception("coroutine broken, failed to remove top coroutine");
                    // remove between
                    coroutine.ListPrevious.ListNext = coroutine.ListNext;
                    coroutine.ListNext.ListPrevious = coroutine.ListPrevious;
                }
                else if (coroutine.ListPrevious != null)
                {
                    // and listNext is null
                    coroutine.ListPrevious.ListNext = null;
                    // remove last
                }
            }
            coroutine.ListPrevious = null;
            coroutine.ListNext = null;
        }

    }//class
}