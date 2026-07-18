---
name: gdd-to-dev-plan
description: 生成完整的游戏开发计划——基于详细 GDD 文档（叙事/玩法/关卡/UI/美术/技术/商业化/打磨），输出阶段划分、任务拆解、工时估算、依赖关系、里程碑和质量门禁。Use when 需要将 GDD 文档集转化为可执行的开发路线图，或进行项目排期、资源规划和里程碑定义。
---

# GDD to Development Plan

**Tier:** POWERFUL
**Category:** Game Design / Production
**Tags:** GDD, dev plan, project management, scheduling, milestones, resource planning

## Overview

将 GDD 扩展流水线（gdd-orchestrator）产出的全套设计文档转化为详细的开发计划。输出可直接用于项目管理的开发路线图，包含阶段划分、任务拆解、工时估算、依赖关系和里程碑。

## Input

扫描 `GDD/` 目录，读取所有存在文档：
- `01-narrative/` → 叙事实现任务
- `02-gameplay/` → 核心系统开发
- `03-level-design/` → 内容制作
- `04-ui-ux/` → UI 开发
- `05-art-audio/` → 资产制作
- `06-tech-design/` → 基础设施
- `07-monetization/` → 运营支撑
- `08-polish/` → 后期优化

## Quick Start

用户说"生成开发计划"、"根据 GDD 排期"、"制作开发路线图"时执行。

## Workflow

### Step 1: Inventory
扫描 `GDD/` 目录，列出所有已有文档，标注缺失部分。

### Step 2: Extract
遍历每份 GDD 文档，提取四类需求：
- **Features**: 每个可玩的机制/系统
- **Content**: 关卡、角色、物品、UI 面板等
- **Tech**: 架构、工具、管线、性能目标
- **Assets**: 2D/3D、音频、动画、VFX 清单

### Step 3: Phase Assignment
按标准游戏开发阶段归类任务（详见 `references/phase-definitions.md`）：
1. Pre-Production — 原型、技术选型、管线
2. Production P1 — 核心玩法、灰盒关卡、占位资产
3. Production P2 — 完整内容、全部系统、正式资产
4. Alpha — 内容完整、可全通
5. Beta — 全面打磨、性能达标、Bug 修复
6. Gold — 最终 QA、本地化、平台认证、发布

### Step 4: Breakdown & Estimate
对每个需求：
- 拆为 1-5 天的可执行任务
- 标注职能（程序/美术/策划/音频/QA）
- 估算工时（人天），参考 `references/estimation-guide.md`
- 标优先级：P0 必须 / P1 重要 / P2 锦上添花

### Step 5: Dependency Mapping
- 硬依赖：A 完成 B 才能开始
- 软依赖：建议顺序
- 标注关键路径（critical path）

### Step 6: Quality Gates
为每个阶段定义完成标准（详见 `references/phase-definitions.md`）。

### Step 7: Output
按 `references/plan-template.md` 模板输出 `DevPlan/` 目录。

## Output Structure

```
DevPlan/
├── 00-overview.md          # 总览与总结
├── 01-pre-production.md    # 预生产阶段
├── 02-production-p1.md     # 生产阶段 1
├── 03-production-p2.md     # 生产阶段 2
├── 04-alpha.md             # Alpha 阶段
├── 05-beta.md              # Beta 阶段
├── 06-gold.md              # Gold 阶段
└── 99-master-schedule.md   # 总时间线与关键路径
```

## Rules

- 仅基于 GDD 文档内容，不虚构功能
- 标注所有推演假设：`> 💡 假设: ...`
- 合理推演优先于向用户提问（但关键决策必须确认）
- 中文输出 + 英文术语

## References

- [Plan Template](references/plan-template.md) — 开发计划输出模板
- [Estimation Guide](references/estimation-guide.md) — 工时估算参考
- [Phase Definitions](references/phase-definitions.md) — 阶段定义与质量门禁
