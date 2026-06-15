---
name: unity-builder-pattern
description: 建造者模式（Builder）——使用流式接口（Fluent API）分步构建复杂对象。简化 GameObject 的组件组合和初始化。Use when 需要创建包含多个组件的复杂 GameObject、需要链式调用简化对象配置、或需要将构建过程与表示分离。
---

# Unity Builder Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, builder, fluent API, GameObject, component

## Overview

建造者模式通过链式调用分步构建复杂对象。在 Unity 中，通过 `IBuilder<T>` 泛型接口 + 流式 API 简化 GameObject 的组件组合和初始化配置。

## Use this skill when

- 需要创建包含 Rigidbody + Collider + 脚本等复杂组件的 GameObject
- 需要链式调用简化对象配置，代码可读性优先
- 需要将构建过程与最终表示分离（同一构建流程产生不同类型对象）

## Do not use this skill when

- 对象很简单（只有 Transform）→ 直接用 `new GameObject()`
- 使用 Prefab 即可满足需求 → Prefab 优先

## Core Features

1. **Fluent API**: 链式方法调用，代码即文档
2. **Generic Interface**: `IBuilder<T>` 适用于任意类型
3. **Component Management**: 一行添加组件

## Core Files (1 file)

- `Builder.cs.txt`: 完整 Builder 模式（接口 + 流式实现）

## Usage

```csharp
var player = new GameObjectBuilder("Player")
    .WithPosition(Vector3.zero)
    .AddComponent<Rigidbody>()
    .Build();
```

## Reference Material

- [Implementation Guide](references/guide.md)
