using System;
using UnityEngine;

namespace Dada.Cores.AI
{
    public sealed class BehaviorTree
    {
        public BehaviorTree(
            BehaviorNode root,
            GameObject agent = null,
            BehaviorBlackboard blackboard = null)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Context = new BehaviorTreeContext(agent, blackboard);
        }

        public BehaviorNode Root { get; }
        public BehaviorTreeContext Context { get; }
        public BehaviorBlackboard Blackboard => Context.Blackboard;
        public BehaviorStatus? LastStatus { get; private set; }

        public BehaviorStatus Tick(float deltaTime)
        {
            Context.Advance(deltaTime);
            LastStatus = Root.Tick(Context);
            return LastStatus.Value;
        }

        public void Restart(bool clearBlackboard = false)
        {
            Root.Abort(Context);
            Root.Reset();
            Context.ResetTime();
            LastStatus = null;

            if (clearBlackboard)
                Blackboard.Clear();
        }

        public void Stop()
        {
            Root.Abort(Context);
            Root.Reset();
            LastStatus = null;
        }
    }
}
