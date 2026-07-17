---
name: "gdd-orchestrator"
description: "一键运行完整的 GDD 扩展流水线——从基础 GDD 出发，依次运行 8 个专业 skill，生成从叙事、玩法、关卡、UI、美术音频、技术设计到商业化和打磨的完整游戏设计文档集。Use when 需要将一份简短的基础 GDD 扩展为全套可交付的设计文档。"
---

# GDD Orchestrator — 完整 GDD 扩展流水线

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, game design document, pipeline, orchestration, full expansion

## Overview

这是 GDD 扩展系统的总控入口。它按正确顺序依次调用 8 个专业 skill，将一份基础 GDD（可能只有 15 页）扩展为一套完整的设计文档集（最终产出约 25-30 份 markdown 文件）。

## Pipeline Architecture

```
基础 GDD
  │
  ├─→ [1] gdd-narrative ──→ GDD/01-narrative/
  │      (World Bible, Character Lore, Story Structure, Tone Guide)
  │
  ├─→ [2] gdd-gameplay ──→ GDD/02-gameplay/
  │      (Player Mechanics, Combat Design, Progression, Abilities)
  │
  ├─→ [3] gdd-level-design ──→ GDD/03-level-design/
  │      (LD Pillars, Level One-Pagers, World Map, Quest Structure)
  │
  ├─→ [4] gdd-ui-ux ──→ GDD/04-ui-ux/
  │      (HUD Wireframes, Menu Map, Onboarding, Accessibility)
  │
  ├─→ [5] gdd-art-audio ──→ GDD/05-art-audio/
  │      (Art Bible, Technical Art Specs, Audio Design)
  │
  ├─→ [6] gdd-tech-design ──→ GDD/06-tech-design/
  │      (Architecture, Save Systems, Networking, Tools, Performance)
  │
  ├─→ [7] gdd-monetization (可选) ──→ GDD/07-monetization/
  │      (Monetization, Live Service, Analytics, Marketing)
  │
  └─→ [8] gdd-polish (可选) ──→ GDD/08-polish/
         (Camera System, Feedback Loop Index, VFX Triggers)
```

## Usage

### Quick Start (Full Pipeline)

用户说 "展开我的 GDD" 或 "完整扩展 GDD" 时，按以下流程：

1. **确认 GDD 路径** — 询问用户基础 GDD 文件位置
2. **确认模式** — 询问 "完整模式"（全部 8 步）还是 "原型模式"（跳过步骤 7 和 8）
3. **按顺序执行** — 依次调用每个 skill
4. **每步完成后报告** — 汇总已生成的文档和关键决策

### Individual Step

用户说 "展开 GDD 的 [部分名称]" 时，直接调用对应的 skill。

## Execution Protocol

### Step 1: gdd-narrative
```
调用 Skill: gdd-narrative
输入: 基础 GDD
输出: GDD/01-narrative/ (4 文件)
```
- 提醒用户：这是唯一不需要前置输出的步骤
- 完成后确认：World Bible、角色设定、故事结构、基调指南已生成

### Step 2: gdd-gameplay
```
调用 Skill: gdd-gameplay
输入: 基础 GDD + GDD/01-narrative/
输出: GDD/02-gameplay/ (4 文件)
```
- 提醒用户：会读取世界观中的技术/魔法规则
- 完成后确认：玩家机制、战斗设计、经济数值、技能目录已生成

### Step 3: gdd-level-design
```
调用 Skill: gdd-level-design
输入: 基础 GDD + GDD/01-narrative/ + GDD/02-gameplay/
输出: GDD/03-level-design/ (4+ 文件)
```
- 提醒用户：会读取世界观区域设定和敌人类型
- 完成后确认：关卡支柱、关卡单页、世界地图、任务结构已生成

### Step 4: gdd-ui-ux
```
调用 Skill: gdd-ui-ux
输入: 基础 GDD + GDD/02-gameplay/ + GDD/03-level-design/
输出: GDD/04-ui-ux/ (4 文件)
```
- 提醒用户：会根据玩法系统确定 HUD 信息需求
- 完成后确认：HUD 线框图、菜单地图、引导计划、无障碍规格已生成

### Step 5: gdd-art-audio
```
调用 Skill: gdd-art-audio
输入: 基础 GDD + GDD/01-narrative/ + GDD/02-gameplay/ + GDD/03-level-design/
输出: GDD/05-art-audio/ (3 文件)
```
- 提醒用户：会读取世界观的视觉基调和玩法动画需求
- 完成后确认：美术圣经、TA 规格、音频设计已生成

### Step 6: gdd-tech-design
```
调用 Skill: gdd-tech-design
输入: 基础 GDD + GDD/02-gameplay/ + GDD/03-level-design/ + GDD/04-ui-ux/ + GDD/05-art-audio/
输出: GDD/06-tech-design/ (5 文件)
```
- 提醒用户：会汇总前面所有文档的技术需求
- 完成后确认：架构、存档、网络、工具、性能目标已生成

### Step 7: gdd-monetization (可选)
```
调用 Skill: gdd-monetization
输入: 基础 GDD + GDD/02-gameplay/ + GDD/04-ui-ux/
输出: GDD/07-monetization/ (4 文件)
```
- 如果是原型模式，自动跳过此步骤
- 完成后确认：盈利设计、Live Ops 路线图、KPI、市场策略已生成

### Step 8: gdd-polish (可选)
```
调用 Skill: gdd-polish
输入: 基础 GDD + GDD/02-gameplay/ + GDD/04-ui-ux/ + GDD/05-art-audio/ + GDD/03-level-design/
输出: GDD/08-polish/ (3 文件)
```
- 如果是原型模式，自动跳过此步骤
- 完成后确认：摄像机系统、反馈循环索引、后处理触发条件已生成

## Final Report

全部完成后，生成一份总结报告：

```markdown
# GDD 扩展完成

## 基础信息
- 游戏名称: [名称]
- 基础 GDD: [路径]
- 模式: [完整/原型]

## 生成文件总览
| 步骤 | 技能 | 文件数 | 关键决策 |
|------|------|--------|----------|
| 1 | 叙事与世界 | 4 | [列出关键决策] |
| 2 | 玩法系统 | 4 | [...] |
| 3 | 关卡设计 | N | [...] |
| 4 | UI/UX | 4 | [...] |
| 5 | 美术音频 | 3 | [...] |
| 6 | 技术设计 | 5 | [...] |
| 7 | 商业化 | 4 | [...] (或"已跳过") |
| 8 | 打磨 | 3 | [...] (或"已跳过") |

## 总文件数: N
## 输出目录: GDD/

## 下一步建议
- [建议 1: 如"请美术团队审查 Art Bible"]
- [建议 2: 如"运行 gdd-gameplay 时需要实际测试数值"]
- [建议 3: ...]
```

## Rules

- **每个步骤必须等待上一步完成** —— skill 之间的依赖关系是严格顺序的
- **如果某一步生成的文件被后续步骤读取，确认文件已存在后再前进**
- **遇到缺乏信息导致无法推进时，给出合理推演并用 `> 💡` 标注**
- **每一步完成后向用户简短报告**（1-2 句），不要沉默执行
- **如果用户在中间步骤给出修改意见，记录并传递给后续步骤**
