---
name: unity-memento-pattern
description: 备忘录模式（Memento）——捕获并恢复对象状态快照。完美适配装备方案管理、关卡检查点和 Undo/Redo 系统。Use when 需要保存/恢复对象状态（RPG 装备方案切换）、需要实现 Undo/Redo、或需要关卡检查点存档功能。
---

# Unity Memento Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, memento, snapshot, undo, checkpoint, loadout

## Overview

备忘录模式提供结构化的状态快照方案。严格分离状态数据（Memento）、状态生产者（Originator）和状态管理者（Caretaker），确保已保存快照不可变，不会被后续修改破坏。

## Use this skill when

- 需要多套装备方案（Loadout）之间切换并保留各自状态
- 需要实现 Undo/Redo（关卡编辑器、操作历史）
- 需要关卡检查点（Checkpoint）——保存玩家位置、血量、道具等
- 需要保存/恢复复杂对象的状态（如技能冷却、Buff 剩余时间）

## Do not use this skill when

- 只需要简单存档 → 用 `unity-data-persistence`
- 状态非常简单（单个数值）→ 直接赋值即可
- 不需要回退到历史状态 → 不需要快照

## Core Features

1. **Immutable Snapshots**: 已保存的数据不可被后续修改破坏
2. **Loadout Management**: 内置多槽位管理（Caretaker）
3. **Encapsulation**: 状态结构对保存系统隐藏，仅暴露必要接口

## Core Files

- `IMemento.cs.txt`: 基础接口和泛型 Memento 包装
- `LoadoutManager.cs.txt`: Caretaker 实现，管理列表型快照
- `HotbarMementoExample.cs.txt`: UI/物品状态持久化实战示例

## Usage

参考 `HotbarMementoExample.cs.txt` —— 使用拷贝构造函数保存 ScriptableObject 列表。

## Reference Material

- [Implementation Guide](references/guide.md)
