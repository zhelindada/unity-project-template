---
name: "gdd-level-design"
description: "从基础 GDD 扩展出详细的关卡设计与内容规格文档——关卡设计支柱、单关卡单页设计、世界地图与节奏、任务/Quest 结构模板。Use when 需要将'我们有森林和地下城'这样的概括描述变成具体到每个关卡的地图布局和敌人配置。"
---

# GDD Level Design & Content Specifications

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, level design, world map, pacing, quest design, mission structure
**Position in pipeline:** 3 of 8

## Overview

这是 GDD 扩展流水线的第三步。此时你应该已经有了世界观（区域设定）和玩法系统（玩家能力、敌人类型），现在是时候将这些元素组合成具体的可玩内容。你将设计关卡支柱、每个关卡的单页设计、世界地图与难度曲线、以及任务结构模板。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/01-narrative/01-world-bible.md` — 区域名称、地貌、文化用于关卡主题
- `GDD/01-narrative/03-story-structure.md` — 故事节拍与关卡对应关系
- `GDD/02-gameplay/01-player-mechanics.md` — 玩家能力决定关卡可设计的挑战类型
- `GDD/02-gameplay/02-combat-design.md` — 敌人类型用于关卡配置

## Output Documents

### Document 1: Level Design Pillars (`GDD/03-level-design/01-ld-pillars.md`)

这是一份给整个关卡设计团队的约束和规范文档，确保所有关卡风格一致。

**Metrics & Standards**
```
走廊宽度: 最小 Xm, 标准 Ym, 最大 Zm
门洞高度: Xm
标准房间面积: Xm² (小) / Ym² (中) / Zm² (大)
天花板高度: Xm (室内) / Ym (室外)
跳跃间隙: 最小 Xm (基础跳跃距离的 80%)
攀爬高度: Xm
掩体高度: 低掩体 Xm / 高掩体 Ym
视线距离: 室内 Xm / 室外 Ym
```

**Modular Kit Definition**
- 列出所有需要的模块化拼装件类型（墙壁、地板、门、楼梯、柱子、平台等）
- 每种拼装件的标准尺寸
- 材质主题变体（如"石质地牢套件"vs"木质小屋套件"）

**Level Design Rules**
- 引导规则（灯光引导/颜色引导/地标引导/敌人引导）
- 不能做的事（如"不要让玩家看到未加载区域""不要在战斗区域设置死胡同"）
- 可读性规则（远景地标、路径标识、危险信号）

**Combat Space Design**
- 战斗场地分类（竞技场型/走廊型/垂直型/掩体型）
- 每种类型的敌人配置原则
- 掩体密度标准

### Document 2: Individual Level One-Pagers (`GDD/03-level-design/02-level-one-pagers/`)

为每个关卡/区域生成一份独立文件。每个文件按如下模板：

```markdown
# [关卡名称]

## 基本信息
- 序号: Level X
- 主题/地貌: [描述]
- 前置关卡: [名称]
- 后续关卡: [名称]
- 预计通关时间: X 分钟
- 所属章节: [名称]

## 故事背景
[1段：这个关卡在故事中的位置和目的]

## 地图布局
[ASCII map 或 mermaid flowchart 描绘关卡结构]

## 敌人配置
| 区域 | 敌人类型 | 数量 | 刷新方式 |
|------|----------|------|----------|
| ... | ... | ... | ... |

## 关键元素
- 收集品: [列出]
- 谜题: [描述]
- 脚本事件: [描述]
- Boss 战: [是/否，如果是则描述]

## 视觉/氛围
- 光照主色调: [颜色]
- 天气: [描述]
- 音乐/音效: [风格]
- 目标玩家情绪: [情感词]

## 特殊机制
[这个关卡独有的玩法机制]
```

### Document 3: World Map & Pacing (`GDD/03-level-design/03-world-map-pacing.md`)

**World Map**
- 使用 ASCII 或 mermaid 绘制完整世界地图
- 标注所有关卡位置、连接路线
- 标注交通方式（步行/传送/载具）
- 标注各区域的建议等级范围

**Difficulty Curve**
- 使用 mermaid 或 ASCII 绘制难度曲线图
- X 轴：游戏进度（关卡 1→N）
- Y 轴：难度（1-10）
- 标注 Boss 关卡和难度峰值

**Player Experience Heat Map**
- 基于时间线的玩家体验曲线：
```
关卡1: 教学/低强度 ▏ 关卡2: 新敌人引入 ▏ 关卡3: 探索为主 ▏ 关卡4: Boss/高强度 ▏ 关卡5: 休息/叙事 ▏ ...
```
- 标注"紧张区"和"喘息区"的交替节奏
- 标注新机制/新敌人首次出现的位置

**Travel Methods**
- 关卡间如何移动（选关菜单/世界地图行走/无缝衔接）
- 快速旅行/传送解锁条件

### Document 4: Mission / Quest Structure (`GDD/03-level-design/04-quest-structure.md`)

**Quest Taxonomy**
- 主线任务（Main Quest）的模板结构
- 支线任务（Side Quest）的模板结构
- 动态事件（Dynamic Event）的模板结构
- 每日/重复任务（Daily/Repeatable）的模板结构

**Quest Template**
每个任务类型使用以下模板：
```markdown
## 任务名称
- 任务类型: [主线/支线/动态/每日]
- 接取条件: [等级/前置任务/触发区域]
- 任务给予者: [NPC 名称]
- 任务目标: [清晰列出]
- 奖励: [经验/金币/物品]
- 失败条件: [描述]
- 分支逻辑: [如果...则...]
- 对话脚本: [简要摘录]
```

**Branching Logic Patterns**
- 定义常见的分支模式模板：
  - "选择 A 或 B"（互斥奖励）
  - "帮助 X 或 Y"（声望分支）
  - "按顺序完成 N 个目标"（无分支线性）
  - "目标达成度影响奖励"（百分比分支）
- 每个模式标注 fail state 处理

**Quest Chain Example**
- 提供 1 个完整的 5-8 步任务链示例
- 展示任务之间的触发关系（mermaid flowchart）

## Workflow

1. **Read all upstream inputs** — 基础 GDD + `GDD/01-narrative/` + `GDD/02-gameplay/` 中已有文档
2. **Define pillars first** — 关卡支柱约束后续所有设计
3. **Design world map** — 确定关卡数量和连接关系
4. **Generate level one-pagers** — 每个关卡一页
5. **Plot pacing curve** — 确保紧张/喘息交替
6. **Design quest templates** — 建立可复用的任务模式
7. **Report** — 汇总关卡数量、总预估时长、关键节奏决策

## Design Principles

- **支柱先行**：关卡支柱是整个团队的"关卡语言"，必须先于具体设计确定
- **节奏是王道**：高强度遭遇后必须跟喘息区（安全屋/叙事/探索区）
- **教学即关卡**：每个新机制首次出现的关卡本质上是该机制的教学关
- **每个关卡应该有一个"记忆点"**：一个让玩家在通关后会记住的独特时刻或场景

## Output Rules

- 保存到 `GDD/03-level-design/` 目录
- 每个关卡单页为独立文件，放在 `02-level-one-pagers/` 子目录中
- mermaid 图表用于流程和结构，ASCII 用于地图布局
- 中文输出 + 英文术语
