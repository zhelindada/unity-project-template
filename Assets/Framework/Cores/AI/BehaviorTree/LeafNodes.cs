using System;

namespace Dada.Cores.AI
{
    public sealed class ActionNode : BehaviorNode
    {
        private readonly Func<BehaviorTreeContext, BehaviorStatus> _action;

        public ActionNode(Func<BehaviorTreeContext, BehaviorStatus> action, string name = null)
            : base(name)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            return _action(context);
        }
    }

    public sealed class ConditionNode : BehaviorNode
    {
        private readonly Func<BehaviorTreeContext, bool> _condition;

        public ConditionNode(Func<BehaviorTreeContext, bool> condition, string name = null)
            : base(name)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            return _condition(context) ? BehaviorStatus.Success : BehaviorStatus.Failure;
        }
    }

    public sealed class WaitNode : BehaviorNode
    {
        private readonly float _duration;
        private float _elapsed;

        public WaitNode(float duration, string name = null)
            : base(name)
        {
            if (duration < 0f)
                throw new ArgumentOutOfRangeException(nameof(duration));
            _duration = duration;
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            _elapsed = 0f;
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            _elapsed += context.DeltaTime;
            return _elapsed >= _duration ? BehaviorStatus.Success : BehaviorStatus.Running;
        }
    }
}
