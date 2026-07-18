---
name: unity-interaction
description: 通用射线交互系统（Interaction）——基于接口 + Raycast 的交互框架。适用于 RPG、平台跳跃、模拟等任意类型。Use when 需要玩家注视/靠近物体时触发交互（E 键捡起/对话/开门）、需要统一的交互接口支持多种交互类型、或需要通用检测逻辑。
---

# Unity Interaction System

**Tier:** POWERFUL
**Category:** Unity / Gameplay Systems
**Tags:** Unity, interaction, raycast, interface, RPG, gameplay

## Overview

最小化、通用的交互系统。基于 `IInteractable` 接口 + Raycast 检测，支持主/副两种交互动作。零外部依赖，适用于任意游戏类型。

## Use this skill when

- 需要玩家注视/靠近物体时触发交互（按 E 捡起/对话/开门/操作机关）
- 需要统一的交互接口支持多种交互物类型
- 需要区分主交互（E）和副交互（F）
- 需要一个即插即用的交互框架

## Do not use this skill when

- 交互物只有 1-2 种且逻辑完全固定 → 直接写判断即可
- 需要物理碰撞触发而非射线检测 → 用 Trigger
- 需要 UI Toolkit 的点击交互 → 用 UI 事件系统

## Core Features

1. **Interface-Based**: `IInteractable` 适用于任意对象类型
2. **Dual Actions**: 主交互（E）和副交互（F）
3. **Raycast Detection**: 基于玩家朝向自动选择目标
4. **Zero Dependencies**: 完全自包含，不依赖其他 skill

## Core Files (2 files)

- `IInteractable.cs.txt`: 交互接口定义
- `InteractionController.cs.txt`: Raycast 检测 + 输入处理

## Usage

详见 [guide.md](references/guide.md)。
