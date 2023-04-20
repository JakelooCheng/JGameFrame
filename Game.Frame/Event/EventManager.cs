using Game.Base.Module;
using Game.Base.LifeCycle;
using Game.Base.ObjectPool;
using System;
using System.Collections.Generic;
using Game.Base.Logs;

namespace Game.Frame.Event
{
    public class EventManager : GameModuleBase, ILateUpdate, IShutdown
    {
        private Dictionary<string, PoolList<EventHandler<EventArgsBase>>> receiverDic 
            = new Dictionary<string, PoolList<EventHandler<EventArgsBase>>>();

        private Dictionary<string, PoolList<EventArgsBase>> delaySendDic
            = new Dictionary<string, PoolList<EventArgsBase>>();


        public void OnLateUpdate()
        {
            foreach (var sender in delaySendDic)
            {
                if (!receiverDic.TryGetValue(sender.Key, out var receiverList))
                {
                    foreach (var arg in sender.Value)
                    {
                        CallReceivers(receiverList, null, arg);
                    }
                    sender.Value.Dispose();
                }
            }
            delaySendDic.Clear();
        }
        
        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe(string id, EventHandler<EventArgsBase> handler)
        {
            if (!receiverDic.TryGetValue(id, out var receiverList))
            {
                receiverList = PoolList<EventHandler<EventArgsBase>>.Get();
                receiverDic.Add(id, receiverList);
            }
            receiverList.Add(handler);
        }

        /// <summary>
        /// 订阅（同时移除其他订阅者）
        /// </summary>
        public void SubscribeSingle(string id, EventHandler<EventArgsBase> handler)
        {
            if (!receiverDic.TryGetValue(id, out var receiverList))
            {
                receiverList = PoolList<EventHandler<EventArgsBase>>.Get();
                receiverDic.Add(id, receiverList);
            }
            else
            {
                receiverList.Clear();
            }
            receiverList.Add(handler);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public void Unsubscribe(string id, EventHandler<EventArgsBase> handler)
        {
            if (receiverDic.TryGetValue(id, out var receiverList))
            {
                receiverList.Remove(handler);
                if (receiverList.Count == 0)
                {
                    receiverDic.Remove(id);
                    receiverList.Dispose();
                }
            }
        }

        /// <summary>
        /// 立刻发送/触发
        /// </summary>
        public void Send(object sender, EventArgsBase args)
        {
            if (receiverDic.TryGetValue(args.Id, out var receiverList))
            {
                CallReceivers(receiverList, sender, args);
            }
        }

        /// <summary>
        /// 帧后触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void SendDelay(EventArgsBase args)
        {
            if (!delaySendDic.TryGetValue(args.Id, out var receiverList))
            {
                receiverList = PoolList<EventArgsBase>.Get();
                delaySendDic.Add(args.Id, receiverList);
            }
            receiverList.Add(args);
        }

        /// <summary>
        /// 触发
        /// </summary>
        private void CallReceivers(List<EventHandler<EventArgsBase>> receiverList, object sender, EventArgsBase args)
        {
            try
            {
                foreach (var receiver in receiverList)
                {
                    receiver.Invoke(sender, args);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception.ToString());
            }
        }

        public void Shutdown()
        {
            foreach (var receiverList in receiverDic.Values)
            {
                receiverList.Dispose();
            }
            foreach (var delayList in delaySendDic.Values)
            {
                delayList.Dispose();
            }
            receiverDic.Clear();
            delaySendDic.Clear();
        }
    }
}