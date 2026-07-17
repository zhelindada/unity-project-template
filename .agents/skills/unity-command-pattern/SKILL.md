---
name: unity-command-pattern
description: 命令模式（Command）——将操作封装为对象，支持队列、撤销/重做（Undo/Redo）和异步执行。Use when 需要实现操作回退（关卡编辑器）、命令队列（RTS 单位指令）、或异步任务管理。
---

# Unity Command Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, command, undo, redo, async, queue

## Overview

命令模式将每个操作封装为独立对象，支持排队、延迟执行、撤销/重做。基于 `ICommand` 异步接口 + ScriptableObject 轻量命令。

## Use this skill when

- 需要实现 Undo/Redo 功能（关卡编辑器、技能序列）
- 需要命令队列系统（RTS 单位指令、回合制战斗行动列表）
- 需要异步执行命令并追踪完成状态
- 需要在 Inspector 中配置可执行命令

## Do not use this skill when

- 简单的函数调用即能满足需求
- 没有撤销/排队需求 → 直接用方法调用

## Core Features

1. **Async Interface**: `ICommand` 支持 Task-based 异步执行
2. **SO Commands**: ScriptableObject 驱动的轻量命令，Inspector 配置
3. **Command Queue**: 顺序执行 + 完成回调

## Core Files (1 file)

- `Command.cs.txt`: 完整命令模式（接口 + SO + 队列）

## Usage

复杂逻辑实现 `ICommand`，简单 Inspector 配置用 `CommandSO`。

## Reference Material

- [Implementation Guide](references/guide.md)
