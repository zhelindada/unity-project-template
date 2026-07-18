---
name: "gdd-polish"
description: "从基础 GDD 扩展出打磨、反馈与'游戏手感'文档——摄像机系统规格、反馈循环索引、后处理与 VFX 触发条件。Use when 需要为每个玩家操作都规划好对应的视听触反馈，让游戏'手感'达到可量产的规格。原型阶段可跳过。"
---

# GDD Polish, Feedback & "Game Feel"

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, game feel, camera system, feedback loops, VFX, screen shake, juice
**Position in pipeline:** 8 of 8 (final skill)
**Status:** 可选（原型阶段可跳过）

## Overview

这是 GDD 扩展流水线的最后一步。"Game Feel"（游戏手感）往往存在于一个独立的"果汁文档"（Juice Document）中。你的任务是为游戏中的每一个玩家行为定义其对应的视觉、音频和触觉反馈。同时，你将详细设计摄像机系统——这是游戏手感中影响最大但也最容易被忽视的组件。

> **注意：** 基础 GDD 中标注为 "skipped for prototype" 的部分——此技能在原型阶段可以跳过。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/02-gameplay/01-player-mechanics.md` — 每个玩家动作都需要在此定义反馈
- `GDD/02-gameplay/02-combat-design.md` — 战斗手感参数
- `GDD/04-ui-ux/01-hud-wireframes.md` — HUD 动画与反馈配合
- `GDD/05-art-audio/03-audio-design.md` — SFX 优先级系统的延续
- 所有 `GDD/03-level-design/02-level-one-pagers/` — 脚本事件的反馈设计

## Output Documents

### Document 1: Camera System Specs (`GDD/08-polish/01-camera-system.md`)

**Camera Mode Definitions**
- 摄像机类型选择及理由（第三人称跟随/第一人称/固定视角/混合）
- 如果有多种模式，列出切换条件和逻辑

**Framing Rules**
- 默认构图规则（如 Rule of Thirds 中角色占画面位置）
- 不同游戏状态的构图变化：
  - 探索：宽松跟随，角色偏画面下方
  - 战斗：拉远，角色居中，锁定敌人保持在画面内
  - 瞄准：拉近，角色偏向一侧
  - Boss 战：特殊构图规则

**Damping & Smoothing**
- 位置跟随的阻尼参数（X/Y/Z 轴分离）
- 旋转跟随的阻尼参数
- 模拟惯性（摄像机是否需要滞后于玩家移动）

**Collision Detection**
- 摄像机碰撞检测方式（Raycast/SphereCast）
- 遮挡物透明化策略（半透明/消失/轮廓）
- 摄像机推近（Push In）的行为参数

**Field of View**
- 默认 FOV 值
- FOV 变化的触发条件：
  - 冲刺/加速时微增 FOV（速度感）
  - 瞄准时微减 FOV（聚焦感）
  - 低血量时微增 FOV（紧张感）
- FOV 变化的缓动曲线

**Screen Shake Parameters**
- 建立屏幕震动事件表：

| 事件 ID | 触发条件 | 震动强度 | 持续时间 | 衰减曲线 | 频率 |
|---------|----------|----------|----------|----------|------|
| 玩家受伤 | OnHit | 0.3 | 0.1s | ease-out | 高 |
| 普通攻击命中 | OnAttackHit | 0.1 | 0.05s | ease-out | 低 |
| 暴击命中 | OnCritHit | 0.4 | 0.15s | ease-out | 中 |
| Boss 重击 | BossHeavy | 0.7 | 0.3s | ease-out | 低 |
| 爆炸 | Explosion | 0.5 | 0.2s | ease-in-out | 高 |
| 落地 | Land | 0.2~0.5 | 0.08s | ease-out | 低 |

- 震动方向（水平/垂直/旋转/随机）

**Special Camera Behaviors**
- 对话/剧情中的摄像机行为
- 死亡时的摄像机行为
- 胜利/通关时的摄像机行为
- 传送/切换场景时的摄像机行为

### Document 2: Feedback Loop Index (`GDD/08-polish/02-feedback-loop-index.md`)

这是整个 Game Feel 文档的核心——为每一个玩家可执行的动作，定义其完整的反馈链条。

**Feedback Loop Template**
对每个玩家动作填写：
```markdown
## [动作名称]

### 触发条件
[何时触发该动作]

### Visual Feedback
- 角色动画: [动画名称和时序]
- VFX: [特效描述，如"击中火花 + 伤害数字"]
- UI 响应: [如"准星扩散→收缩"]
- 环境响应: [如"地面留下痕迹"]

### Audio Feedback
- 主要音效: [描述]
- 辅助音效: [描述]
- 音效变化: [随状态如何变化]
- 优先级: [1-10]

### Haptic Feedback (触觉)
- 振动描述: [如"短促轻振 50ms"]
- 强度: [0.0-1.0]

### 其他系统响应
- 摄像机: [是否有震动/FOV变化]
- HUD: [任何 HUD 动画]
- 时间: [是否有顿帧/慢动作]
```

**Complete Action List**
至少覆盖以下所有动作类型（根据游戏类型扩展）：
- 移动类：行走、奔跑、冲刺、跳跃、二段跳、滑铲、攀爬、游泳
- 战斗类：轻攻击、重攻击、特殊攻击、格挡、闪避、弹反、处决
- 交互类：拾取物品、开启宝箱、打开门、拉开关、对话
- 状态变化：升级、死亡、复活、进入新区域、任务完成、成就解锁
- UI 交互：打开菜单、切换标签、装备物品、确认购买、获得新物品
- 环境交互：进入水中、受到伤害、回血、获得 Buff、受到 Debuff

### Document 3: Post-Processing & VFX Triggers (`GDD/08-polish/03-post-processing-vfx.md`)

**Post-Processing Volume Rules**
- 默认后处理配置（Bloom/Ambient Occlusion/Color Grading/Vignette/Chromatic Aberration/...）
- 不同区域的后处理变体（如地下城的暗角更强、森林的色调偏暖）
- 后处理效果的渐变参数

**Conditional Post-Processing Effects**
定义在特定状态下触发的后处理变化：

| 状态 | 效果 | 强度 | 淡入时间 | 淡出时间 |
|------|------|------|----------|----------|
| 低血量 (<30%) | 屏幕边缘红色 Vignette | 0.6 | 0.3s | 0.5s |
| 中毒 | 屏幕色调偏绿 + 边缘模糊 | 0.4 | 0.5s | 0.3s |
| 速度加成 | 速度线 + FOV 微增 | 0.3 | 0.1s | 0.2s |
| 进入水下 | 蓝色色调 + 模糊 + 气泡 | 0.8 | 1.0s | 1.0s |
| 致命一击命中 | 短暂黑白 + 高对比度 | 1.0 | 0.0s (instant) | 0.15s |

**VFX Trigger System**
- 基于事件的 VFX 触发表（链接到 Document 2 中的 Feedback Loop Index）
- 特效池化策略（哪些 VFX 需要预热/池化）
- VFX 的 LOD 规则（远距离特效简化）

**Screen Effects**
- 全屏特效的触发条件和参数：
  - 受击闪屏（Hit Flash）：颜色/透明度/时长
  - 低血量指示（Low HP vignette）：颜色/形状/强度曲线
  - 速度线（Speed Lines）：触发速度阈值/透明度/密度
  - 晕眩效果（Stun）：模糊/重影/色偏
  - 传送过渡（Teleport）：效果类型/时长

**Transition Effects**
- 场景切换过渡效果
- 时间流逝过渡效果
- 梦境/幻觉等特殊状态的过渡

## Workflow

1. **Read upstream inputs** — 重点读 player-mechanics, combat-design, audio-design, HUD 文档
2. **Design camera first** — 摄像机是所有反馈的"画框"，必须先确定
3. **Audit every player action** — 从 player-mechanics 中提取完整的动作清单
4. **Build feedback matrix** — 为每个动作填写完整的视听触反馈
5. **Design post-processing triggers** — 条件触发的后处理效果
6. **Consistency check** — 确保相似动作的反馈语言一致
7. **Report** — 汇总反馈事件总数、摄像机模式、后处理状态数

## Design Principles

- **每个动作都值得反馈**：没有反馈的动作等于不存在
- **反馈分层**：同一动作可以有多个反馈层（动画 + VFX + SFX + 震动 + 后处理），层层叠加
- **反馈的"信号清晰度"**：危险信号的反馈（如低血量）必须比舒适信号（如升级）更强烈
- **摄像机的"隐身原则"**：好的摄像机系统应该让玩家忘记它的存在——只在做错时才会被注意到
- **后处理是调料不是主菜**：大量后处理叠加会让画面模糊，克制使用

## Output Rules

- 保存到 `GDD/08-polish/` 目录
- 反馈清单用 markdown table
- 参数用精确数值，标注单位
- 中文输出 + 英文术语
