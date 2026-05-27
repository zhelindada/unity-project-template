---
name: unity-ui-guide
description: Unity UI 组件参数详解与创建规范——覆盖 Native UGUI 和 Evo UI 两种框架。Use when 需要创建/修改 Unity UI、排查 UI 不显示/不可交互问题、或需要参考 UI 组件参数。
---

# Unity UI 组件参数详解与创建规范

## Overview

根据项目使用的 UI 框架，选择对应的子技能：

## UI Framework Selection

| 框架 | 子技能 | 说明 |
|------|--------|------|
| **Unity UGUI (Canvas)** | [native](native/SKILL.md) | 传统 Canvas-based UI，GameObject + RectTransform，内置组件（Image/Button/Slider 等） |
| **Evo UI** | [evo](evo/SKILL.md) | 基于 uGUI + TextMesh Pro 的第三方 UI 框架，提供 Styler 全局样式系统、预配置 UI 元素、图标系统等 |

> 未来可能扩展：UI Toolkit (UITK) 等。

## Workflow

1. **判断项目使用的 UI 框架**
   - 如果使用传统 UGUI（GameObject/Canvas 体系）→ 执行 [native](native/SKILL.md) 子技能
   - 如果项目引入了 Evo UI 包 → 执行 [evo](evo/SKILL.md) 子技能
2. **读取对应子技能的 SKILL.md** 并按其规范执行

## Quick Reference (跨框架通用)

### Canvas 基准

| 字段 | 值 |
|------|-----|
| renderMode | ScreenSpaceOverlay |
| referenceResolution | 1920×1080 |
| matchWidthOrHeight | 竖屏=0，横屏=1，通用=0.5 |

### 字号层级

| 角色 | 默认 |
|------|------|
| title | 36 |
| heading | 28 |
| body | 24 |
| caption | 18 |
| button | 22 |

### 通用设计约束

| 约束 | 阈值 |
|------|------|
| 文字-背景对比度 | ≥ 3.0 (WCAG AA) |
| 可点击元素最小尺寸 | ≥ 24px |
| 字号下限 | ≥ 12px |
| 弹窗最大尺寸 | ≤ 父级 80% |
