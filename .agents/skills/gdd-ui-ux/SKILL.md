---
name: "gdd-ui-ux"
description: "从基础 GDD 扩展出完整的 UI/UX 与玩家引导文档——HUD 线框图、完整菜单地图与流程图、新手引导教程计划、无障碍辅助规格。Use when 需要把'我们会有一个菜单'变成每一个屏幕、每一个按钮的精确规格。"
---

# GDD UI/UX & Player Onboarding

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, UI design, UX, HUD, menu flow, onboarding, accessibility
**Position in pipeline:** 4 of 8

## Overview

这是 GDD 扩展流水线的第四步。根据项目的 UI 框架，选择对应的子技能执行：

## UI Framework Selection

根据项目的 UI 框架选择对应的子技能：

| 框架 | 子技能 | 说明 |
|------|--------|------|
| **Unity UGUI (Canvas)** | [native](native/SKILL.md) | 传统 Canvas-based UI，GameObject + RectTransform |

> 未来可能扩展：UI Toolkit (UITK)、UI Toolkit + UGUI 混合 等。

## Workflow

1. **判断项目使用的 UI 框架** — 检查项目中的 Canvas/Panel 使用情况
   - 如果使用传统 UGUI（GameObject/Canvas 体系）→ 执行 native 子技能
2. **读取对应子技能的 SKILL.md** 并按其规范执行
3. **输出到 `GDD/04-ui-ux/` 目录**

## Design Principles (通用)

这些原则适用于所有 UI 框架：

- **信息层次**：一级信息（HP/弹药）常驻 HUD，二级信息（Buff）按需显示，三级信息（详细属性）在菜单中
- **最少点击原则**：任何功能应该在 3 次点击以内到达
- **死亡线原则**：HP 低时信息传递不能仅依赖颜色（色盲玩家无法区分），必须有形状/位置/动效的冗余
- **教学融入关卡**：最好的教学是玩家不知道自己被教了

## Output Rules

- 保存到 `GDD/04-ui-ux/` 目录
- ASCII 线框图用于所有 HUD 和屏幕布局
- mermaid flowchart 用于导航和流程图
- 中文输出 + 英文术语
