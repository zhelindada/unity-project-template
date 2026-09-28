using System;
using UnityEngine;

namespace Dada.Cores.AI
{
    public enum BehaviorTreeUpdateMode
    {
        Update,
        FixedUpdate,
        Manual,
    }

    public abstract class BehaviorTreeRunner : MonoBehaviour
    {
        [Header("Behavior Tree")]
        [SerializeField] private BehaviorTreeUpdateMode _updateMode = BehaviorTreeUpdateMode.Update;
        [SerializeField, Min(0f)] private float _tickInterval = 0.1f;

        private float _accumulator;

        public BehaviorTree Tree { get; private set; }
        public BehaviorBlackboard Blackboard => Tree?.Blackboard;
        public BehaviorStatus? LastStatus => Tree?.LastStatus;

        protected virtual void Awake()
        {
            InitializeTree();
        }

        protected virtual void OnDisable()
        {
            Tree?.Stop();
            _accumulator = 0f;
        }

        private void Update()
        {
            if (_updateMode == BehaviorTreeUpdateMode.Update)
                Advance(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_updateMode == BehaviorTreeUpdateMode.FixedUpdate)
                Advance(Time.fixedDeltaTime);
        }

        public void InitializeTree()
        {
            Tree?.Stop();
            BehaviorNode root = CreateRoot();
            if (root == null)
                throw new InvalidOperationException($"{GetType().Name}.CreateRoot() 不能返回 null。");

            Tree = new BehaviorTree(root, gameObject, CreateBlackboard());
            _accumulator = 0f;
            OnTreeInitialized(Tree);
        }

        public BehaviorStatus TickTree(float deltaTime)
        {
            if (Tree == null)
                InitializeTree();
            return Tree.Tick(deltaTime);
        }

        public void RestartTree(bool clearBlackboard = false)
        {
            Tree?.Restart(clearBlackboard);
            _accumulator = 0f;
        }

        protected abstract BehaviorNode CreateRoot();
        protected virtual BehaviorBlackboard CreateBlackboard() => new();
        protected virtual void OnTreeInitialized(BehaviorTree tree) { }

        private void Advance(float deltaTime)
        {
            if (_tickInterval <= 0f)
            {
                TickTree(deltaTime);
                return;
            }

            _accumulator += deltaTime;
            if (_accumulator < _tickInterval)
                return;

            float elapsed = _accumulator;
            _accumulator = 0f;
            TickTree(elapsed);
        }
    }
}
