# Behavior Tree

代码驱动的运行时行为树，命名空间为 `Dada.Cores.AI`。

## 基本用法

```csharp
public sealed class EnemyAI : BehaviorTreeRunner
{
    protected override BehaviorNode CreateRoot()
    {
        return new SelectorNode(
            new SequenceNode(
                new ConditionNode(ctx => ctx.Blackboard.Get("hasTarget", false)),
                new ActionNode(ChaseTarget)),
            new ActionNode(Patrol));
    }

    private BehaviorStatus ChaseTarget(BehaviorTreeContext context)
    {
        // 持续追击时返回 Running，抵达后返回 Success，无法抵达返回 Failure。
        return BehaviorStatus.Running;
    }

    private BehaviorStatus Patrol(BehaviorTreeContext context)
    {
        return BehaviorStatus.Running;
    }
}
```

`BehaviorTreeRunner` 支持 `Update`、`FixedUpdate` 和 `Manual` 三种更新模式。需要接入项目的 `TickEngine` 时，将模式设为 `Manual`，在 `ITickListener.OnTick` 中调用 `TickTree(deltaTime)`。

节点实例只能拥有一个父节点。正在运行的树应通过 `Stop()` 或 `Restart()` 中止，不要直接重置运行中的节点。
