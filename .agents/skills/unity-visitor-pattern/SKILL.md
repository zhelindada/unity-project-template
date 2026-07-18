---
name: unity-visitor-pattern
description: 访问者模式（Visitor）——对对象结构执行解耦操作。完美适配 PowerUp 系统和复杂数值修正。Use when 需要对不同类型执行统一操作（PowerUp 影响玩家/敌人/道具各不同）、需要在不修改已有类的情况下扩展操作、或需要双向分发（Double Dispatch）。
---

# Unity Visitor Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, visitor, double-dispatch, powerup, stat modifier

## Overview

访问者模式通过双向分发（Double Dispatch）安全地对多种类型执行统一操作。在 Unity 中通过 ScriptableObject 访客 + `IVisitable` 接口实现，专为 PowerUp 系统优化。

## Use this skill when

- 需要对不同类型对象执行统一操作（PowerUp 触碰玩家回血、触碰敌人扣血、触碰道具销毁）
- 需要在不修改已有组件的情况下添加新操作（新 PowerUp 类型）
- 需要双向分发（操作行为取决于访客类型 + 被访对象类型）
- 多个系统需要对同一组类型执行不同逻辑

## Do not use this skill when

- 只有 1-2 种类型和 1-2 种操作 → if/switch 即可
- 操作逻辑不关心对象类型 → 普通接口即可
- 类型很少变化但操作频繁变化 → 考虑 Strategy

## Core Features

1. **Double Dispatch**: 安全且结构化的多类型操作处理
2. **Scriptable Visitors**: ScriptableObject 驱动物逻辑，Inspector 可配置
3. **Decoupled Logic**: 核心组件无"逻辑泄漏"，外部系统可独立扩展

## Core Files

- `VisitorCore.cs.txt`: 基础接口和可访问组件基类
- `ScriptableVisitor.cs.txt`: Pro Path ScriptableObject 实现
- `VisitorExample.cs.txt`: 数值修正实战示例

## Usage

详见 [guide.md](references/guide.md) —— 包含 Pro/Light 双路径说明。

## Reference Material

- [Implementation Guide](references/guide.md)
