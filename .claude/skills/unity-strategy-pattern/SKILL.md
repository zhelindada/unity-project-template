---
name: unity-strategy-pattern
description: 策略模式（Strategy）——用 ScriptableObject 封装可互换算法（法术、攻击、AI 行为）。支持运行时热替换，无需修改客户端代码。Use when 需要可替换的行为系统（法术/技能/攻击方式）、需要在 Inspector 中切换算法、或需要运行时动态改变 AI 行为。
---

# Unity Strategy Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, strategy, ScriptableObject, spell, ability, AI, hot-swap

## Overview

策略模式用可互换的算法对象替代条件分支（switch/if）。在 Unity 中通过 ScriptableObject 实现，每个行为（火球、护盾、环绕弹幕）是独立资产，可在 Inspector 中拖拽替换。

## Use this skill when

- 需要可替换的技能/法术系统（火球 ↔ 冰箭 ↔ 护盾）
- 需要 AI 行为可切换（巡逻 ↔ 追击 ↔ 逃跑）
- 添加新行为时不想修改 PlayerController
- 需要在 Inspector 中拖拽切换行为而无需写代码

## Do not use this skill when

- 行为只有 1-2 种且固定不变 → 直接写判断即可
- 行为逻辑差异极小（如只是参数不同）→ 用同一个类 + 配置参数

## Core Features

1. **Encapsulated Logic**: 每个行为独立在各自的 ScriptableObject 中
2. **Hot-Swapping**: 运行时替换策略资产即可改变行为
3. **Open/Closed Compliance**: 添加新法术无需修改 PlayerController

## Core Files

- `SpellStrategy.cs.txt`: 抽象 ScriptableObject 基类（策略接口）
- `ShieldSpellStrategy.cs.txt`: 简单实例化策略
- `OrbitalSpellStrategy.cs.txt`: 复杂逻辑策略（Pro Path）

## Usage

详见 [GUIDE.md](references/guide.md) —— 创建灵活的法术/技能系统。
