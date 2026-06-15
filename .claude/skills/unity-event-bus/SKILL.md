---
name: unity-event-bus
description: 高级事件总线（Event Bus）——基于反射自举的代码驱动全局消息系统。零配置、O(1) 派发、零 GC 分配。Use when 需要解耦系统间通信（玩家死亡→UI更新+音效+存档）、需要全局事件广播而无需手动连线、或需要高性能零分配事件系统。
---

# Unity Event Bus

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, event bus, decoupling, messaging, reflection, performance

## Overview

高性能类型安全的全局消息中枢。通过反射自动发现所有事件类型并初始化总线，实现系统间零耦合通信。优化为 struct 消息以消除 GC，使用静态泛型索引实现 O(1) 派发。

## Use this skill when

- 需要解耦多个系统之间的通信（如玩家死亡→UI 更新 + 播放音效 + 存档）
- 需要全局事件广播而无需手动在 Inspector 中连线
- 需要高性能零分配的事件系统
- 项目有多个独立模块需要松耦合通信

## Do not use this skill when

- 只有 1-2 个对象间通信 → 直接引用或 UnityEvent 即可
- 需要 ScriptableObject 可视化事件通道 → 使用 SO Event Channel
- 只是简单的 UI 数据更新 → 用 Data Binding

## Core Features

1. **Reflection Bootstrapping**: 自动发现所有事件类型，场景加载时自动初始化
2. **Generic Performance**: 静态泛型索引实现 O(1) 事件派发
3. **Zero Allocation**: struct 消息优化，无运行时 GC
4. **Editor Safety**: 退出 Play Mode 自动清理所有静态绑定，防止 Editor 内存泄漏

## Core Files

- `EventBusCore.cs.txt`: 基础接口和静态泛型总线中枢
- `EventBusBootstrapper.cs.txt`: Unity 自动初始化和程序集扫描
- `EventBusExample.cs.txt`: 玩家事件和系统事件实战示例

## Usage

定义 `struct : IEvent` → 创建 `EventBinding<T>` → `OnEnable` 中注册 → 任意位置 `EventBus<T>.Raise()` 触发。

## Reference Material

- [Implementation Guide](references/guide.md)
