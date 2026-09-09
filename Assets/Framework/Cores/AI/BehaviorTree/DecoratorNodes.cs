using System;

namespace Dada.Cores.AI
{
    public abstract class DecoratorNode : BehaviorNode
    {
        protected DecoratorNode(BehaviorNode child, string name = null)
            : base(name)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
            if (child.Parent != null)
                throw new InvalidOperationException($"节点 '{child.Name}' 已属于另一个父节点。");
            child.Parent = this;
        }

        public BehaviorNode Child { get; }

        protected override void OnAbort(BehaviorTreeContext context)
        {
            Child.Abort(context);
        }

        protected override void OnReset()
        {
            Child.Reset();
        }
    }

    public sealed class InverterNode : DecoratorNode
    {
        public InverterNode(BehaviorNode child, string name = null)
            : base(child, name)
        {
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            Child.Reset();
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            return Child.Tick(context) switch
            {
                BehaviorStatus.Success => BehaviorStatus.Failure,
                BehaviorStatus.Failure => BehaviorStatus.Success,
                _ => BehaviorStatus.Running,
            };
        }
    }

    public sealed class RepeatNode : DecoratorNode
    {
        private readonly int _repeatCount;
        private int _completedCount;

        public RepeatNode(BehaviorNode child, int repeatCount = -1, string name = null)
            : base(child, name)
        {
            if (repeatCount < -1)
                throw new ArgumentOutOfRangeException(nameof(repeatCount));
            _repeatCount = repeatCount;
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            _completedCount = 0;
            Child.Reset();
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            if (_repeatCount == 0)
                return BehaviorStatus.Success;

            BehaviorStatus status = Child.Tick(context);
            if (status == BehaviorStatus.Running)
                return BehaviorStatus.Running;
            if (status == BehaviorStatus.Failure)
                return BehaviorStatus.Failure;

            _completedCount++;
            if (_repeatCount >= 0 && _completedCount >= _repeatCount)
                return BehaviorStatus.Success;

            Child.Reset();
            return BehaviorStatus.Running;
        }
    }

    public sealed class UntilFailureNode : DecoratorNode
    {
        public UntilFailureNode(BehaviorNode child, string name = null)
            : base(child, name)
        {
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            Child.Reset();
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            BehaviorStatus status = Child.Tick(context);
            if (status == BehaviorStatus.Running)
                return BehaviorStatus.Running;
            if (status == BehaviorStatus.Failure)
                return BehaviorStatus.Success;

            Child.Reset();
            return BehaviorStatus.Running;
        }
    }
}
