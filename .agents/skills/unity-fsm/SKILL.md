---
name: unity-fsm
description: 有限状态机（FSM）——基于 State + Strategy 模式的可扩展状态机。包含计时器系统、检测策略和 AI 模板。Use when 需要复杂 AI（巡逻/追击/逃跑）、玩家控制器状态管理（站立/跳跃/冲刺）、或需要结构化状态转换的系统。
---

# Unity Finite State Machine (FSM)

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, FSM, state machine, AI, player controller, strategy

This skill provides a modular, object-oriented State Machine architecture for Unity, specifically refactored to match the implementation in Adam Myhre's 3D Platformer.

## Core Features
- **Object-Oriented**: Each state is a dedicated class.
- **Hybrid Support**: Support for both "Heavy" (classes) and "Light" (`ActionState`) states.
- **Strategy-Based Transitions**: Uses Predicates and Detection Strategies.
- **Timer System**: Includes Adam Myhre's `CountdownTimer` and `StopwatchTimer`.
- **Namespace Support**: Uses `Platformer` and `Utilities` namespaces.

## Core Files (assets/code/)
- `IState.cs.txt`, `StateMachine.cs.txt`, `IPredicate.cs.txt`: Base FSM logic.
- `ActionState.cs.txt`: **New!** Lambda-based state to reduce boilerplate.
- `Timer.cs.txt`: AI cooldown management.
- `IDetectionStrategy.cs.txt`, `ConeDetectionStrategy.cs.txt`: Sensing logic.
- `PlayerDetector.cs.txt`, `Enemy.cs.txt`, `EnemyBaseState.cs.txt`: AI templates.

## Usage Guides
- [GUIDE.md](references/guide.md): General FSM setup and wiring.
- [ENEMY-AI.md](references/enemy-ai.md): Specific guide for implementing AI with Detection Strategies.

## Implementation Pattern
1. **Define** your states by inheriting from `BaseState`.
2. **Wire** them in your `PlayerController` or `EnemyAI` using `stateMachine.AddTransition`.
3. **Execute** via `Update()` and `FixedUpdate()` calls to the machine.
