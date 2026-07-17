---
name: "gdd-art-audio"
description: "从基础 GDD 扩展出完整的美术与音频方向指南——美术圣经、技术美术规格、音频设计文档。Use when 需要让美术和音频团队可以在没有持续沟通的情况下独立产出风格一致的美术和音频资源。"
---

# GDD Art & Audio Direction Guides

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, art direction, audio design, art bible, technical art, VFX, music
**Position in pipeline:** 5 of 8

## Overview

这是 GDD 扩展流水线的第五步。此时你应该已经有了世界观、玩法系统和关卡设计——这些都会直接约束美术和音频方向。你的任务是为美术团队和音频团队提供足够清晰的边界和方向，让他们可以独立工作而不会偏离整体愿景。

注意：此技能不要求你生成最终美术资源——只生成方向性文档和规格约束。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/01-narrative/01-world-bible.md` — 区域地貌、文化、技术水平决定视觉主题
- `GDD/01-narrative/04-tone-guide.md` — 整体基调直接指导美术风格和音乐风格
- `GDD/02-gameplay/01-player-mechanics.md` — 玩家动作决定动画状态机复杂度
- `GDD/03-level-design/01-ld-pillars.md` — 关卡支柱中的模块化拼装件直接对应 3D 资产需求

## Output Documents

### Document 1: Art Bible (`GDD/05-art-audio/01-art-bible.md`)

**Visual Pillars**
- 定义 3 个核心视觉支柱（如"破败中的生机""巨构与渺小""光与影的对比"）
- 每个支柱用 1-2 段描述其含义和在游戏中的具体体现
- 列出 3-5 个视觉参考作品（游戏/电影/绘画），标注从每个参考中借鉴的具体元素

**Style & Aesthetic**
- 整体美术风格定义（如 stylized PBR / hand-painted / pixel art / realistic）
- 风格的"光谱"位置（卡通←→写实, 明亮←→阴暗, 简约←→繁杂）
- 参考作品的画面截图描述

**Color Palette System**
- 环境色板：按区域/生物群系定义主色调、辅色、强调色
```
沙漠区域: 主 #E8C382 / 辅 #A67C52 / 强调 #4A90D9
森林区域: 主 #2D5A27 / 辅 #8B9A46 / 强调 #D4A843
...
```
- UI 色板：主色调、辅色、警告色、成功色、信息色
- 阵营色板：每个阵营的标志色及使用规则
- 颜色使用禁忌（如"永远不要用纯黑 #000000""UI 中避免红色以外的暖色系统提示"）

**Lighting Rules**
- 每个区域的光照模板（方向光颜色/强度、环境光颜色、雾色）
- 室内 vs 室外的光照差异原则
- 剧情时刻的特殊光照规则
- 光照优先级系统（如：玩家安全区暖光 / 战斗区冷光 / Boss 区戏剧光）

**Character Visual Language**
- 角色剪影（Silhouette）设计原则
- 形状语言（Shape Language）：圆形=友好/三角形=威胁/方形=稳固 的具体应用
- 角色比例规范（头身比、肩宽比）
- 主角 vs NPC vs 敌人的视觉区分策略
- 装备/服装随进度变化的视觉语言

**Environment Art Principles**
- 区域可读性：玩家如何通过视觉判断自己身处哪个区域
- 地标设计原则：每个区域需要一个独特的大型地标（远距离可见）
- 路径引导的视觉策略

**VFX Style Guide**
- VFX 整体风格（风格化/写实/低多边形粒子）
- 颜色编码规则（治疗=绿色/毒=紫色/火=橙红/冰=蓝白）
- 粒子密度规范（手机端低/PC 端高）
- VFX 在屏幕上的最大覆盖面积限制

### Document 2: Technical Art Specs (`GDD/05-art-audio/02-technical-art-specs.md`)

**Polygon Budgets**
| 资产类型 | 主视角模型 | LOD1 | LOD2 | LOD3 |
|----------|-----------|------|------|------|
| 主角 | X,000 | X,000 | X,000 | - |
| Boss | X,000 | X,000 | X,000 | X,000 |
| 普通敌人 | X,000 | X,000 | X,000 | - |
| 场景物件(大型) | X,000 | X,000 | X,000 | - |
| 场景物件(小型) | X00 | X00 | - | - |
| 武器 | X,000 | X00 | - | - |

**Texture Specifications**
- 纹理大小规范（按资产类型和平台分层）
- 纹理格式（移动端 ASTC / PC 端 BC7）
- 纹理图集策略
- PBR 贴图规范（哪些贴图必须、哪些可选）
- 特殊材质需求列表

**LOD Rules**
- LOD 切换距离（按屏幕占比或世界距离定义）
- 每个 LOD 级别的面数缩减比例
- LOD 切换的过渡方式
- 哪些资产类型不需要 LOD

**Shader Requirements**
- 列出所有需要的 Shader 类型及用途：
  - Standard/PBR（标准表面）
  - Toon/Cel（卡通渲染）
  - Foliage（植被风动）
  - Water（水面）
  - Glass/Transparent（透明）
  - Dissolve（溶解/消失）
  - Outline（轮廓高亮）
  - Terrain（地形混合）
  - UI（UI 特殊材质）
- 每个 Shader 的可调参数列表
- 性能约束（指令数上限、采样器数量上限）

**Animation Specifications**
- 列出所有需要的动画状态机，每个状态机包含：
  - 所属对象（主角/敌人类型/UI）
  - 状态列表（Idle, Walk, Run, Jump, Attack_01, Attack_02, Hit, Death 等）
  - 过渡条件
- 指定每个动画所需的 Blend Tree：
  - 移动 Blend Tree（Idle→Walk→Run 的混合参数）
  - 瞄准偏移（Aim Offset）
- 动画压缩设置
- 需要面部动画 / 口型同步的角色列表

**Platform-Specific Constraints**
- 如果多平台：按平台列出每个视觉参数的上限
- 最低配置 vs 推荐配置的画面差异

### Document 3: Audio Design Document (`GDD/05-art-audio/03-audio-design.md`)

**Music Direction**
- 整体音乐风格描述（管弦/电子/民族/混合）
- 参考作品列表（电影配乐/游戏原声/音乐家）
- 主旋律（Leitmotif）系统：为主题/角色/区域定义的旋律主题
- 动态音乐系统设计：
  - 哪些参数控制音乐变化（战斗状态/区域/剧情节点/血量）
  - 层级叠加逻辑（Layer-based）还是片段切换（Horizontal re-sequencing）
  - 转场逻辑（Stinger 的使用时机）

**Ambient Soundscapes**
- 每个生物群系/区域的环境音景描述
- 环境音的元素清单（风声/水声/动物/人群等）
- 环境音的空间化策略

**SFX Priority System**
- 建立 SFX 优先级表（1-10，10 为最高优先）
```
10: UI 确认音、受到致命伤害警告
9: 敌人攻击预警、Boss 关键技能提示
8: 玩家攻击命中反馈、格挡/闪避反馈
7: 脚步声、开关门
6: 环境交互音
5: 环境氛围细节
...
```
- 同优先级同时播放上限
- 语音频道的专用优先级

**Real-Time Mixing Rules**
- 不同游戏状态下的混音策略：
  - 探索：环境音为主，音乐为辅
  - 战斗：音乐和 SFX 提升，环境音降低
  - 剧情对话：所有非语音 -6dB
  - 暂停/菜单：音乐保持，SFX 和语音静音
- Sidechain 压缩规则（什么声音应该压缩什么）

**Voiceover Direction**
- 配音风格指南（表演风格、语速、情绪范围）
- 主要角色的配音方向（年龄感、音色、说话节奏）
- 配音录制规格（采样率/位深/文件格式/响度标准 LUFS）
- 语音后期处理要求
- 需要本地化的语言列表

## Workflow

1. **Read all upstream inputs** — 基础 GDD + `GDD/01-narrative/` + `GDD/02-gameplay/` + `GDD/03-level-design/` 中已有文档
2. **Define visual pillars** — 从世界观和基调中提炼核心视觉原则
3. **Build color system** — 基于区域和阵营分配色板
4. **Specify technical constraints** — 根据目标平台确定面数/纹理/Shader 预算
5. **Design audio system** — 从音乐方向到 SFX 优先级到混音规则
6. **Cross-reference gameplay** — 确保动画状态机覆盖所有玩家动词
7. **Report** — 汇总关键美术决策、资产数量预估、音频优先级框架

## Design Principles

- **约束即自由**：清晰的美术边界让美术师在框内任意发挥，而不是在无限空间中迷失
- **视觉层级优先**：先定义大形状和颜色，再考虑细节纹理
- **音频是游戏感的 50%**：一个优秀的 SFX 优先级系统是"游戏手感"的基础
- **每种颜色都需要理由**：不能凭喜好定色板，必须根植于世界观和情感目标

## Output Rules

- 保存到 `GDD/05-art-audio/` 目录
- 颜色使用 hex 代码 + 文字描述
- 表格用于面数预算、纹理规格、SFX 优先级
- mermaid 用于动画状态机和音频动态系统
- 中文输出 + 英文术语
