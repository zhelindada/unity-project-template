using System;
using UnityEngine;

namespace Dada.Cores.AI
{
    public sealed class BehaviorTreeContext
    {
        public GameObject Agent { get; }
        public Transform Transform => Agent != null ? Agent.transform : null;
        public BehaviorBlackboard Blackboard { get; }
        public float DeltaTime { get; private set; }
        public float ElapsedTime { get; private set; }

        public BehaviorTreeContext(GameObject agent, BehaviorBlackboard blackboard = null)
        {
            Agent = agent;
            Blackboard = blackboard ?? new BehaviorBlackboard();
        }

        internal void Advance(float deltaTime)
        {
            if (deltaTime < 0f)
                throw new ArgumentOutOfRangeException(nameof(deltaTime), "行为树时间增量不能为负数。");

            DeltaTime = deltaTime;
            ElapsedTime += deltaTime;
        }

        internal void ResetTime()
        {
            DeltaTime = 0f;
            ElapsedTime = 0f;
        }
    }
}
