---
name: gdd-pge-prototype
description: 基于 PGE（Plan-Generate-Evaluate）循环从 GDD 文档生成可玩游戏原型。Plan 分析 GDD 并规划架构，Generate 创建代码/场景/资产，Evaluate 运行并检测问题，自动迭代修复直到原型可玩。Use when 需要从 GDD 生成经过验证的可玩原型、需要自动迭代修复原型问题、或需要 PGE 驱动的质量保证流程。
---

# GDD PGE Prototype

**Tier:** POWERFUL
**Category:** Game Design / Prototyping
**Tags:** GDD, PGE, prototype, iterative, evaluate, quality

## Overview

PGE（Plan-Generate-Evaluate）循环驱动的原型生成器。与一次性生成的 `gdd-to-prototype` 不同，本技能在生成后自动进入 Evaluate 阶段——运行原型、收集错误、对照检查清单逐项验证，发现问题后自动迭代修复，直到原型通过全部验证标准或达到最大迭代轮数（5 轮）。

触发短语："PGE 生成原型"、"用 PGE 做原型"、"生成并验证原型"。

## PGE Cycle

`Plan → Generate → Evaluate → (未通过? 分析→修复→重新Evaluate) → 全部通过 ✓`

最多 5 轮，每轮开头报告：`> 🔄 PGE 第 N/5 轮：[本轮聚焦的问题]`

## Phase 1: Plan

### 1.1 读取 GDD
读取 `GDD/` 目录（或 gdd-orchestrator 产出），重点提取 `02-gameplay/`（核心玩法动词）、`03-level-design/`（关卡结构）、`04-ui-ux/`（HUD 需求）、`06-tech-design/`（技术约束）。详见 `references/pge-cycle-detail.md`。

### 1.2 确定原型范围
输出一份 **Plan Card**，plan card 的核心和必须实现的机制部分使用gdd已经定义的内容，不添加任何前置文档没有涉及的内容：

```
📋 Plan Card
├── 核心验证假设
├── 必须实现的机制
├── 不做的系统
├── 成功标准
└── 风险点
```

完整模板见 `references/pge-cycle-detail.md`。

### 1.3 框架检测
检测引擎（Unity/Unreal/Godot/纯代码）。Unity 项目确认 Input System 和渲染管线，使用 gladekit-unity MCP。

## Phase 2: Generate

样例：

按 Plan Card 搭建所有所需场景

**编译门禁**：`compile_scripts` → 等待 `status='idle'`。`errorCount > 0` → 修复 → 重新编译 → 直到 `errorCount=0`。编译不通过不进入 Phase 3。

## Phase 3: Evaluate

### 3.1 运行时验证
`start_runtime_observation` → 进入 Play Mode（≥ 5 秒）→ `get_runtime_events` 收集所有 Error/Exception。

### 3.2 检查清单（7 项必检）
- [ ] 场景可运行，0 runtime errors
- [ ] 玩家可移动/跳跃/核心交互
- [ ] 核心 Gameplay 循环可完整体验（30 秒内）
- [ ] Camera 跟随正常，不穿墙不抖动
- [ ] UI 显示正确（血条/分数实时更新）
- [ ] 无任何和游戏系统相关的报错

完整测试方法见 `references/eval-checklist.md`。

### 3.3 判定
- 全部通过 → 退出循环，输出原型 ✓
- 未通过 → 生成 Fix Card → 修复 → 重新 Evaluate。修复模式见 `references/iteration-patterns.md`

## Iteration Control

| 终止条件 | 行为 |
|---------|------|
| 全部检查通过 | 成功退出，输出原型摘要 |
| 达到 5 轮仍未通过 | 输出原型 + 已知问题清单 |
| 连续 2 轮无进展 | 暂停，请求用户指导 |

## Output

`Assets/_Prototype/` 下包含 Scene、Scripts、Prefabs、UI。完成后输出 PGE 最终报告：迭代轮数、通过项、已知问题、验证结论（✅ 可玩 / ⚠️ 有条件可玩 / ❌ 需人工介入）。

## Rules

- 全部 Primitive 占位资产（Cube/Sphere/Capsule/Plane）
- 单脚本 ≤ 100 行；中文注释 + 英文标识符
- 不做：存档、菜单、设置、本地化、音效（核心验证依赖除外）
- 推演假设标注：`> 💡 假设: ...`
- 每轮迭代向用户简短报告（1-2 句）
- 可与 `gdd-to-prototype` 组合：本技能在其基础上叠加 PGE 循环层

## References

- [PGE Cycle Detail](references/pge-cycle-detail.md) — 三阶段详细步骤、Plan Card 模板、场景搭建序列、脚本模板
- [Evaluation Checklist](references/eval-checklist.md) — 完整验证检查清单与测试方法
- [Iteration Patterns](references/iteration-patterns.md) — 常见问题分类与修复模式速查表
