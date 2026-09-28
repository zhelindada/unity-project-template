using System;
using System.Collections.Generic;
using System.Linq;

namespace Dada.Cores.AI
{
    public abstract class CompositeNode : BehaviorNode
    {
        private readonly List<BehaviorNode> _children = new();

        protected CompositeNode(IEnumerable<BehaviorNode> children, string name = null)
            : base(name)
        {
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            foreach (BehaviorNode child in children)
                AddChild(child);
        }

        public IReadOnlyList<BehaviorNode> Children => _children;

        public void AddChild(BehaviorNode child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (IsRunning)
                throw new InvalidOperationException("行为树运行时不能修改子节点。");
            if (child.Parent != null)
                throw new InvalidOperationException($"节点 '{child.Name}' 已属于另一个父节点。");

            for (BehaviorNode ancestor = this; ancestor != null; ancestor = ancestor.Parent)
            {
                if (ReferenceEquals(ancestor, child))
                    throw new InvalidOperationException("行为树节点不能形成循环引用。");
            }

            child.Parent = this;
            _children.Add(child);
        }

        protected void ResetChildren()
        {
            foreach (BehaviorNode child in _children)
            {
                if (child.IsRunning)
                    throw new InvalidOperationException($"子节点 '{child.Name}' 尚未中止。");
                child.Reset();
            }
        }

        protected void AbortRunningChildren(BehaviorTreeContext context)
        {
            foreach (BehaviorNode child in _children)
                child.Abort(context);
        }

        protected override void OnAbort(BehaviorTreeContext context)
        {
            AbortRunningChildren(context);
        }

        protected override void OnReset()
        {
            ResetChildren();
        }
    }

    public sealed class SequenceNode : CompositeNode
    {
        private int _currentIndex;

        public SequenceNode(params BehaviorNode[] children)
            : this((IEnumerable<BehaviorNode>)children)
        {
        }

        public SequenceNode(IEnumerable<BehaviorNode> children, string name = null)
            : base(children, name)
        {
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            _currentIndex = 0;
            ResetChildren();
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            while (_currentIndex < Children.Count)
            {
                BehaviorStatus status = Children[_currentIndex].Tick(context);
                if (status == BehaviorStatus.Running)
                    return BehaviorStatus.Running;
                if (status == BehaviorStatus.Failure)
                    return BehaviorStatus.Failure;

                _currentIndex++;
            }

            return BehaviorStatus.Success;
        }
    }

    public sealed class SelectorNode : CompositeNode
    {
        private int _currentIndex;

        public SelectorNode(params BehaviorNode[] children)
            : this((IEnumerable<BehaviorNode>)children)
        {
        }

        public SelectorNode(IEnumerable<BehaviorNode> children, string name = null)
            : base(children, name)
        {
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            _currentIndex = 0;
            ResetChildren();
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            while (_currentIndex < Children.Count)
            {
                BehaviorStatus status = Children[_currentIndex].Tick(context);
                if (status == BehaviorStatus.Running)
                    return BehaviorStatus.Running;
                if (status == BehaviorStatus.Success)
                    return BehaviorStatus.Success;

                _currentIndex++;
            }

            return BehaviorStatus.Failure;
        }
    }

    public enum ParallelPolicy
    {
        RequireAll,
        RequireOne,
    }

    public sealed class ParallelNode : CompositeNode
    {
        private readonly ParallelPolicy _policy;
        private BehaviorStatus?[] _childStatuses;

        public ParallelNode(ParallelPolicy policy, params BehaviorNode[] children)
            : this(children.AsEnumerable(), policy)
        {
        }

        public ParallelNode(IEnumerable<BehaviorNode> children, ParallelPolicy policy, string name = null)
            : base(children, name)
        {
            _policy = policy;
        }

        protected override void OnEnter(BehaviorTreeContext context)
        {
            ResetChildren();
            _childStatuses = new BehaviorStatus?[Children.Count];
        }

        protected override BehaviorStatus OnTick(BehaviorTreeContext context)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (_childStatuses[i] == null || _childStatuses[i] == BehaviorStatus.Running)
                    _childStatuses[i] = Children[i].Tick(context);
            }

            int successes = _childStatuses.Count(status => status == BehaviorStatus.Success);
            int failures = _childStatuses.Count(status => status == BehaviorStatus.Failure);

            if (_policy == ParallelPolicy.RequireAll)
            {
                if (failures > 0)
                    return BehaviorStatus.Failure;
                return successes == Children.Count ? BehaviorStatus.Success : BehaviorStatus.Running;
            }

            if (successes > 0)
                return BehaviorStatus.Success;
            return failures == Children.Count ? BehaviorStatus.Failure : BehaviorStatus.Running;
        }

        protected override void OnExit(BehaviorTreeContext context, BehaviorStatus status)
        {
            AbortRunningChildren(context);
        }
    }
}
