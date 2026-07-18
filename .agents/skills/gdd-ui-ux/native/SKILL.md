---
name: "gdd-ui-ux-native"
description: "[Native/UGUI] 从基础 GDD 扩展出完整的 UI/UX 与玩家引导文档——HUD 线框图、完整菜单地图与流程图、新手引导教程计划、无障碍辅助规格。Use when 需要把'我们会有一个菜单'变成每一个屏幕、每一个按钮的精确规格。"
---

# GDD UI/UX & Player Onboarding — Native (UGUI)

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, UI design, UX, HUD, menu flow, onboarding, accessibility, UGUI, native
**Position in pipeline:** 4 of 8
**UI Framework:** Unity UGUI (Canvas-based)

## Overview

这是 GDD 扩展流水线的第四步。基础 GDD 对 UI 和上手体验的描述通常极其简略甚至完全缺失。你将设计每一个屏幕上每一个 UI 元素的状态和行为，规划完整的菜单导航系统，定义新手引导策略，并制定无障碍辅助规格。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/02-gameplay/01-player-mechanics.md` — 玩家动作决定 HUD 需要显示的信息
- `GDD/02-gameplay/03-progression-economy.md` — 货币/经验系统决定 HUD 和菜单布局
- `GDD/02-gameplay/04-abilities-catalog.md` — 技能系统决定技能 UI
- `GDD/03-level-design/04-quest-structure.md` — 任务系统决定任务 UI

## Output Documents

### Document 1: HUD Wireframes (`GDD/04-ui-ux/01-hud-wireframes.md`)

为 HUD 上每个元素提供精确规格。使用 ASCII 线框图 + 详细描述。

**Full HUD Layout**
```
┌─────────────────────────────────────────────────┐
│ [HP BAR] [MP BAR]          [MINIMAP]  [CURRENCY] │
│                                                    │
│                    (GAME VIEW)                     │
│                                                    │
│ [SKILL SLOTS]              [AMMO]  [QUEST TRACKER] │
│ [⚔ ⚔ ⚔ ⚔]                                     │
└─────────────────────────────────────────────────┘
```

**Per-Element Specs**
对每个 HUD 元素定义：
```markdown
### 生命值条 (Health Bar)
- 位置: 左上角
- 尺寸: 宽 240px, 高 20px
- 样式: 红色填充 + 深色背景 + 边框
- 状态变体:
  - 正常: 标准红色
  - 警告 (<30%): 红色闪烁 2Hz
  - 危险 (<15%): 红色快速闪烁 4Hz + 屏幕边缘红色渐变
  - 回复中: 白色高亮动画覆盖
- 动效:
  - 受伤: 先闪白 0.1s → 红色从右向左减少，缓动 ease-out
  - 回复: 绿色从右向左增加，缓动 ease-out
  - 最大值变化: 条本身伸缩，0.3s ease
- 数字显示: 是/否，字体，位置
```

**覆盖所有可能的 HUD 元素：**
- 生命/魔力/体力/护盾条
- 小地图/雷达
- 货币显示
- 技能/道具快捷栏
- 任务追踪器
- 弹药/耐久度
- 准星
- 交互提示
- 伤害数字
- Buff/Debuff 图标
- 对话字幕区域
- 连击/评分显示
- 目标标记/导航箭头

### Document 2: Full Menu Map & Flowcharts (`GDD/04-ui-ux/02-menu-map.md`)

**Complete Screen Inventory**
列出游戏中每一个屏幕/面板：
- 主菜单
- 暂停菜单
- 设置（画面/声音/控制/辅助功能）
- 物品栏
- 角色状态/属性
- 技能树
- 地图
- 任务日志
- 商店
- 对话界面
- 制作/合成
- 图鉴/成就
- 排行榜/社交
- 死亡/失败画面
- 通关/胜利画面
- 加载画面
- 开场动画/Logo

**Navigation Flowchart**
使用 mermaid flowchart 画出完整的屏幕间导航：
- 从哪个屏幕可以进入哪个屏幕
- 返回逻辑（返回键回到哪里）
- 模态弹窗的层级关系

**Per-Screen Specs**
对每个关键屏幕：
```markdown
### 物品栏 (Inventory)
- 触发方式: 按 Tab / 菜单按钮
- 布局草图: [ASCII wireframe]
- 排序/过滤选项: [列表]
- 物品操作: 使用/装备/丢弃/拆分
- 快捷操作: 右键菜单 / 双击使用
- 过渡动画: 从暂停菜单滑入，0.2s ease-out
- 背景处理: 游戏画面模糊 + 暗化
```

**Settings Screen Detail**
- 画面设置清单（分辨率/全屏/画质预设/垂直同步/FOV/亮度）
- 声音设置（主音量/音乐/音效/语音/动态范围）
- 控制设置（键位绑定/灵敏度/反转轴/死区/震动强度）
- 辅助功能设置（见 Document 4）

### Document 3: Onboarding & Tutorial Plan (`GDD/04-ui-ux/03-onboarding.md`)

**Tutorial Philosophy**
- 教学原则（如"每次只教一个机制""让玩家自己做而非被动看""失败应该是安全的"）
- 教学形式的选用标准：
  - 强制弹窗教程：何时使用、何时避免
  - 上下文提示：用于提醒而非初次教学
  - 关卡设计教学法（Diegetic Tutorial）：安全环境 + 逐步引入
  - 视频教程：用于复杂系统概览

**First-Time User Experience (FTUE) Script**
- 从玩家点击"新游戏"到获得完全控制权的逐秒脚本
- 分钟 0-1: 开场动画/世界观设定
- 分钟 1-3: 基础移动教学（无需弹窗，关卡设计引导）
- 分钟 3-5: 首次战斗教学
- 分钟 5-10: 首次探索和收集
- 分钟 10-15: 首个 UI 交互（打开菜单/装备物品）
- 分钟 15-20: 首次 Boss 或挑战遭遇

**Player Ramps Diagram**
```
复杂度
  ↑
  │         ╭──────── 高级系统
  │      ╭──╯
  │   ╭──╯  中级系统
  │╭──╯
  ╰╯  基础操作
  └──────────────────→ 游戏时间
```

标注每个新机制在第几分钟/第几关引入。

**Contextual Tutorial Triggers**
- 定义触发条件（如"第一次遇到需要二段跳的间隙时弹出提示"）
- 提示的显示时长和消失逻辑
- 提示数据库结构建议

### Document 4: Accessibility Specifications (`GDD/04-ui-ux/04-accessibility.md`)

**Visual Accessibility**
- 色盲模式：至少支持红色盲/绿色盲/蓝色盲三种
  - 具体方案：用形状/图案/文字标注补充颜色信息
- 字幕选项：大小（小/中/大/超大）、背景透明度、说话者标注
- 高对比度模式
- UI 缩放选项

**Motor Accessibility**
- 按键保持/连发设置
- 输入死区自定义
- QTE 替代方案（按住替代连打）
- 单手模式布局

**Audio Accessibility**
- 重要音效的可视化提示（伤害方向指示、敌人脚步声字幕）
- 单声道输出选项
- 独立音量控制粒度的要求

**Cognitive Accessibility**
- 难度选项设计（不仅是数值缩放，而是改变机制复杂度）
- 导航辅助（明确的任务标记、防迷路指引）
- 信息密度选择（简化 HUD 模式）

## Workflow

1. **Read inputs** — 基础 GDD + 所有可用上游文档
2. **Audit required HUD elements** — 根据游戏机制列出所有必要信息显示
3. **Design HUD layout** — 先整体布局再逐个元素细化
4. **Map all screens** — 确保没有遗漏任何界面
5. **Script the FTUE** — 从玩家视角编写逐分钟体验
6. **Define accessibility** — 覆盖视觉/运动/听觉/认知四个维度
7. **Report** — 汇总 UI 界面总数、关键导航路径、辅助功能覆盖情况

## Design Principles

- **信息层次**：一级信息（HP/弹药）常驻 HUD，二级信息（Buff）按需显示，三级信息（详细属性）在菜单中
- **最少点击原则**：任何功能应该在 3 次点击以内到达
- **死亡线原则**：HP 低时信息传递不能仅依赖颜色（色盲玩家无法区分），必须有形状/位置/动效的冗余
- **教学融入关卡**：最好的教学是玩家不知道自己被教了

## Output Rules

- 保存到 `GDD/04-ui-ux/` 目录
- ASCII 线框图用于所有 HUD 和屏幕布局
- mermaid flowchart 用于导航和流程图
- 中文输出 + 英文术语
