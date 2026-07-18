---
name: "gdd-gameplay"
description: "从基础 GDD 扩展出详细的玩法深度与系统设计文档——玩家机制深度分析、战斗/敌人设计圣经、成长与经济数值表、技能树/能力目录。Use when 需要将'玩家可以跳跃'这样的一句话需求变成精确的物理参数表和实现规格。"
---

# GDD Gameplay Depth & System Design

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, gameplay design, combat design, progression, economy, skill trees
**Position in pipeline:** 2 of 8

## Overview

这是 GDD 扩展流水线的第二步。基础 GDD 对玩法的描述通常过于模糊（"玩家可以战斗""有多种敌人""角色会成长"），无法直接用于实现。你的任务是将这些模糊描述转化为精确的数值、状态机、数据表和实现规格。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/01-narrative/01-world-bible.md` — 魔法/技术规则影响战斗系统设计
- `GDD/01-narrative/02-character-lore.md` — 角色能力风格指导技能设计

## Output Documents

### Document 1: Player Mechanics Deep Dive (`GDD/02-gameplay/01-player-mechanics.md`)

将每个玩家动词（verb）扩展为完整的实现规格。对于每种动作类型，按以下模板展开：

**For Movement Verbs (jump, dash, slide, wall-run, swim, fly, etc.):**
```
动作名称: Jump
├── 基础参数
│   ├── 跳跃高度: X 米 (或 Unity 单位)
│   ├── 跳跃距离: X 米
│   ├── 滞空时间: X 秒
│   ├── 上升/下落速度曲线: [描述或数值]
│   └── 重力倍率: X
├── 手感调节
│   ├── Coyote Time (离地后仍可跳跃的宽恕帧): X 秒
│   ├── Jump Buffer (提前输入的缓存窗口): X 秒
│   ├── 可变跳跃高度 (按住更高): 是/否, 最小/最大比
│   └── 空中转向灵敏度: [数值]
├── 动量系统
│   ├── 是否保留地面动量: 是/否
│   ├── 空中加速度: [数值]
│   └── 速度上限: [数值]
├── 状态转换
│   ├── 可以从哪些状态进入: [列表]
│   ├── 可以转换到哪些状态: [列表]
│   └── 优先级: [数值]
└── 特殊情况
    ├── 着地硬直: X 秒
    └── 与其他动作的连携: [描述]
```

**For All Player Verbs:**
- 列出每个动词，按上述模板规格化
- 标注哪些参数需要策划可调（暴露为配置）、哪些是代码常量
- 提供"手感参数"一节，汇总所有影响手感的宽恕值

**Input System Spec**
- 输入映射表（键盘/手柄/触屏）
- 是否支持键位自定义
- 连击输入窗口（如适用）
- 长按/短按区分逻辑

### Document 2: Combat / Enemy Design Bible (`GDD/02-gameplay/02-combat-design.md`)

**Damage Formula**
- 完整伤害计算公式（包含所有变量）
- 伤害类型表（物理/魔法/元素/真实伤害等）
- 伤害减免计算逻辑
- 暴击机制（触发方式、倍率、是否伪随机分布）

**Full Enemy Roster**
- 使用表格列出所有敌人类型：

| ID | 名称 | 类型 | HP | ATK | DEF | SPD | 出现区域 | 行为树简述 |
|----|------|------|-----|------|------|------|-----------|-------------|
| E001 | ... | 普通 | 100 | 15 | 5 | 3 | 森林 | 巡逻→发现→追击→攻击 |

- 每个敌人类型的详细行为树（mermaid flowchart）
- 攻击模式表（每个攻击的预警时间、伤害范围、冷却时间）

**Boss Fights**
- 每个 Boss 的分阶段设计图
- 阶段转换触发条件
- 每个阶段的攻击模式、弱点、推荐策略
- Boss 竞技场布局要点

**Combat Feel Parameters**
- 受击反馈：屏幕震动参数、顿帧（hitstop）时长、击退距离
- 攻击预警：信号类型（地面标记/特效/音效）、预警时长
- 硬直/踉跄系统：阈值、恢复时间、视觉反馈

**Faction Affiliations**
- 怪物之间的阵营关系（谁会攻击谁）
- 内斗机制（如适用）

### Document 3: Progression & Economy Spreadsheets (`GDD/02-gameplay/03-progression-economy.md`)

**XP & Leveling**
- 完整 XP 曲线表（1 级到最大等级）— 使用表格列出每级所需 XP 和累计 XP
- 公式：`XP_required(level) = ...`
- 升级奖励清单（每级获得什么）
- 属性成长曲线（每级各属性增长值）

**Power Scaling**
- 玩家强度随等级增长曲线
- 敌人强度匹配曲线
- "碾压"和"被碾压"的边界定义

**In-Game Economy Model**
- 所有货币类型定义（金币/钻石/声望币等）
- Faucet（产出）清单：每个来源的产出量、频率
- Sink（消耗）清单：每个消耗点的消耗量、频率
- 经济平衡目标：每日净收益、通胀控制策略

**Loot & Rarity System**
- 稀有度等级定义（如 Common/Uncommon/Rare/Epic/Legendary）
- 每个稀有度的颜色代码、掉落概率
- 装备属性池和数值范围（按稀有度分层）
- 掉落表设计（每个敌人/宝箱的掉落内容）
- 防脸黑机制（pity system）

### Document 4: Skill Trees / Abilities Catalog (`GDD/02-gameplay/04-abilities-catalog.md`)

**Skill Tree Architecture**
- 技能树整体结构（单棵大树 vs 多棵专业树）
- 解锁规则（消耗资源、前置技能、等级限制）
- 技能点数获取方式

**Full Ability Catalog**
- 使用表格列出所有技能，每行一个：

| ID | 名称 | 树/分支 | 层级 | 前置 | 资源消耗 | 冷却 | 效果描述 | 数值 | 最大等级 |
|----|------|---------|------|------|----------|------|----------|------|----------|

**Ability Interactions**
- 技能之间的协同效应（哪些技能组合产生额外效果）
- 冲突规则（哪些技能互斥）

**Upgrade Paths**
- 每个可升级技能的升级表（等级 1→2→3 的数值变化）
- 分支选择（如"火球术→大火球 OR 连珠火球"）

## Workflow

1. **Read inputs** — 基础 GDD + `GDD/01-narrative/`（如果存在）
2. **Inventory player verbs** — 从基础 GDD 提取所有玩家动作
3. **Design systems bottom-up** — 先确定核心公式，再填参数
4. **Reference standards** — 对标同类型游戏的常见数值范围
5. **Generate documents** — 按 1-4 顺序生成
6. **Consistency check** — 确保伤害公式 <-> 敌人数据 <-> 成长曲线三者自洽
7. **Report** — 总结关键数值决策和平衡假设

## Design Principles

- **每个数值都要有理由**：不能随便定一个数。理由可以是"对标 Dark Souls 的轻攻击伤害比例"或"确保 3 级的玩家能在 3 次攻击内杀死同等级小怪"
- **先定边界再定数值**：先确定"最快/最慢""最强/最弱"的边界，中间值做插值
- **标注可调参数**：用 `[TUNING]` 标记需要实际测试调整的参数

## Output Rules

- 保存到 `GDD/02-gameplay/` 目录
- 数值表格用 markdown table
- 状态机/行为树用 mermaid flowchart
- 公式用代码块包裹，标注每个变量的含义
- 中文输出 + 英文术语
