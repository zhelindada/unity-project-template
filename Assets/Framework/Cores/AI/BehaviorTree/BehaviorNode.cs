using System;

namespace Dada.Cores.AI
{
    public abstract class BehaviorNode
    {
        protected BehaviorNode(string name = null)
        {
            Name = string.IsNullOrWhiteSpace(name) ? GetType().Name : name;
        }

        public string Name { get; }
        public BehaviorNode Parent { get; internal set; }
        public bool IsRunning { get; private set; }
        public BehaviorStatus? LastStatus { get; private set; }

        public BehaviorStatus Tick(BehaviorTreeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!IsRunning)
            {
                IsRunning = true;
                LastStatus = null;
                OnEnter(context);
            }

            BehaviorStatus status = OnTick(context);
            LastStatus = status;
            if (status != BehaviorStatus.Running)
            {
                IsRunning = false;
                OnExit(context, status);
            }

            return status;
        }

        public void Abort(BehaviorTreeContext context)
        {
            if (!IsRunning)
                return;
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            OnAbort(context);
            IsRunning = false;
            LastStatus = null;
        }

        public void Reset()
        {
            if (IsRunning)
                throw new InvalidOperationException($"节点 '{Name}' 正在运行，重置前必须先中止它。");

            LastStatus = null;
            OnReset();
        }

        protected virtual void OnEnter(BehaviorTreeContext context) { }
        protected abstract BehaviorStatus OnTick(BehaviorTreeContext context);
        protected virtual void OnExit(BehaviorTreeContext context, BehaviorStatus status) { }
        protected virtual void OnAbort(BehaviorTreeContext context) { }
        protected virtual void OnReset() { }
    }
}
