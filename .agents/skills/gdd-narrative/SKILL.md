---
name: "gdd-narrative"
description: "从基础 GDD 扩展出完整的叙事与世界设定文档集——World Bible、角色背景、故事结构、对话风格指南。Use when 需要将基础 GDD 中的简要设定扩展为详细叙事文档，或当你需要为游戏项目补全世界观与故事线。"
---

# GDD Narrative & World Expansion

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, narrative design, world building, character design, story structure
**Position in pipeline:** 1 of 8 (first skill, no upstream dependencies)

## Overview

这是 GDD 扩展流水线的第一步。你将从一个基础 GDD（可能只有寥寥几段关于故事和设定的描述）中，提取并大幅扩展叙事相关内容，生成 4 份详细的设计文档。因为这是流水线的第一步，你不需要读取任何前置输出。

后续技能会依赖你产出的世界观和角色信息——尤其是 `gdd-level-design`（需要世界观来设计关卡主题）和 `gdd-art-audio`（需要视觉风格锚点）。

## Input Requirements

让用户提供基础 GDD 文件路径。如果用户未指定，主动询问。基础 GDD 至少应包含：
- 游戏名称和类型
- 1-2 段核心故事/设定描述
- 主要角色名称（哪怕只是占位符）

如果基础 GDD 中某个方面完全没有信息，基于游戏类型和已有碎片合理推演，并在生成文档顶部用 `> 💡 以下内容基于 [已有信息] 推演，请验证。` 标注。

## Output Documents

为以下每份文档生成完整的 markdown 文件。不要跳过任何部分——如果基础 GDD 信息不足，进行合理创意推演。

### Document 1: World Bible (`GDD/01-narrative/01-world-bible.md`)

这是整个游戏世界的权威参考文档。必须覆盖以下每个维度：

**Geography & Regions**
- 世界地图的文字描述（列出所有区域/国家/城市及其相对位置）
- 每个区域的地貌特征（气候、地形、代表性景观）
- 各区域之间的交通方式和旅行时间
- 使用 ASCII map 或 mermaid flowchart 描绘区域关系

**History & Timeline**
- 按时间顺序列出 8-15 个关键历史事件
- 每个事件包含：发生时间（可以是虚构纪元）、事件名称、1 段简述、对当代的影响
- 区分"玩家会直接经历的事件"和"背景历史"

**Factions & Power Structures**
- 列出 3-8 个主要势力/组织
- 每个势力包含：名称、核心理念/目标、领袖、主要据点、与其他势力的关系（用 +/- 标记敌对/同盟）
- 画出 faction relationship diagram（mermaid）

**Cultures & Societies**
- 每个主要种族/文明的日常生活、节日、禁忌、艺术风格
- 社会阶层结构
- 宗教信仰体系（神明、教义、仪式）

**Technology / Magic Rules**
- 技术或魔法系统的铁律（能做什么、不能做什么、代价是什么）
- 如果有硬魔法系统：列出具体规则（如 Sanderson 三定律式的约束）
- 技术/魔法在战力和社会中的角色

**Calendar & Time**
- 历法系统（一天几小时、一年几天、季节划分）
- 重要日期和节日

### Document 2: Character Lore (`GDD/01-narrative/02-character-lore.md`)

**Main Characters (Protagonist & Core Cast)**
- 每个角色一页：全名、年龄、外貌描述、性格特质（用大五人格或 MBTI 标注）、背景故事、核心动机、角色弧线（从 A 状态到 B 状态的转变）
- 战斗/能力风格（如果适用）
- 与其他主要角色的关系

**Antagonists**
- 每个反派：动机（不能只是"很坏"）、手段、与主角的镜像关系
- 反派的"合理性"——为什么从他们的角度他们是对的

**NPC Archetypes & Templates**
- 定义 5-8 个 NPC 模板：商人、铁匠、任务给予者、护卫、平民等
- 每个模板包含：对话风格标记、常见出现在哪些区域、典型台词模板

**Faction Reputation System** (RPG 必须，其他类型可选)
- 定义所有可追踪声望的势力
- 声望等级命名（如 敌对→冷淡→中立→友善→崇敬）
- 每个等级解锁的行为和后果
- 声望增减的典型事件

### Document 3: Story Structure (`GDD/01-narrative/03-story-structure.md`)

**Main Plot Beat-by-Beat**
- 使用三幕结构或 Hero's Journey 框架
- 每幕包含 5-10 个关键节拍（beat），每个节拍描述 1-2 段
- 标注每个节拍对应的玩法类型（战斗/探索/解谜/Boss/剧情）

**Branching Decision Trees**
- 列出 3-5 个重大玩家选择点
- 每个选择点画出决策树（mermaid flowchart）：选项 → 即时后果 → 长远影响
- 标注"不可逆"的选择

**Multiple Endings**
- 定义所有结局（通常 2-5 个）
- 每个结局：触发条件、最终场景简述、结局后的世界状态
- 区分"好结局 / 真结局 / 坏结局 / 隐藏结局"

**Side Quest Philosophy**
- 支线任务设计原则（例如：支线必须揭示世界观 OR 深化角色 OR 提供独特奖励）
- 支线类型分类（角色任务、世界事件、收集任务、挑战任务）
- 支线与主线的时机关系

### Document 4: Tone & Writing Guide (`GDD/01-narrative/04-tone-guide.md`)

**Tone Definition**
- 一句话概括整体基调（如"暗黑但带有一丝黑色幽默"）
- 列出 3-5 个参考作品（游戏/电影/小说）及从每个作品借鉴的具体元素

**Dialogue Style Guide**
- 对话规则：每句话长度上限、禁用词汇、标志性口头禅
- 不同角色类型的对话标记（如：学者用长句、佣兵只用短句）
- 提供 3-5 段示例对话（手游/Dating Sim 风格 vs 史诗 RPG 风格区别对待）

**Do's and Don'ts**
- Do: 具体可操作的建议（≥10 条）
- Don't: 需要避免的常见陷阱（≥10 条）

**Exposition Rules**
- 信息透露策略：show-don't-tell 的具体实现方式
- 关键信息的揭露节奏（哪些信息在什么时候让玩家知道）

## Workflow

1. **Read the basic GDD** — 用户指定路径，用 Read 工具读取
2. **Extract narrative seeds** — 识别出已有的所有叙事相关信息
3. **Gap-fill with inference** — 基于游戏类型、已有碎片、行业惯例进行创意推演
4. **Generate each document** — 按 Document 1-4 的顺序依次生成，每个保存为独立 markdown 文件
5. **Cross-reference check** — 确保 4 份文档之间信息一致（角色名、地名、事件时间线）
6. **Report summary** — 用 3-5 句中文总结生成内容，列出关键创意决策供用户审查

## Output Rules

- 所有文件保存到 `GDD/01-narrative/` 目录（相对于项目根目录）
- 用 `> 💡 推演内容` 标注 AI 填补的部分，方便用户识别和修改
- 所有 mermaid 图表用 ```mermaid 代码块包裹
- 每个文档控制在 2000-4000 字之间（足够详细但不过度膨胀）
- 中文输出，但保留英文游戏设计专业术语（如 faction, NPC, side quest 等）

## Cross-References

完成此技能后，以下后续技能会读取你的输出：
- **gdd-level-design** — 读取 World Bible 中的区域设定用于关卡主题设计
- **gdd-art-audio** — 读取 Tone Guide 和 World Bible 中的视觉风格锚点
- **gdd-gameplay** — 读取 Character Lore 中的角色能力风格
