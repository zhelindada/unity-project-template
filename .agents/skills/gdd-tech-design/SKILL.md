---
name: "gdd-tech-design"
description: "从基础 GDD 扩展出完整的技术设计与架构文档——架构概览、数据结构与存档系统、网络模型、工具需求、性能目标。Use when 需要把开发者模糊的玩法需求翻译为具体的技术选型和实现架构。"
---

# GDD Technical Design & Architecture

**Tier:** POWERFUL
**Category:** Game Design
**Tags:** GDD, technical design, game architecture, save systems, networking, performance, tools
**Position in pipeline:** 6 of 8

## Overview

这是 GDD 扩展流水线的第六步。前面所有文档定义了"游戏是什么样的"——现在你需要定义"游戏怎么造出来"。你将做出引擎选择、设计数据架构、定义网络模型（如适用）、规划工具链、设定性能目标。

这份文档的核心读者是工程师和 TA（技术美术）。

## Input Requirements

**Required:**
- 基础 GDD（用户指定路径）

**Recommended (如果存在则必须读取):**
- `GDD/02-gameplay/01-player-mechanics.md` — 玩家的每个动作都是需要实现的系统
- `GDD/02-gameplay/03-progression-economy.md` — 经济数值需要数据结构承载
- `GDD/02-gameplay/04-abilities-catalog.md` — 技能系统影响架构复杂度
- `GDD/03-level-design/02-level-one-pagers/` — 关卡数量和复杂度影响流式加载需求
- `GDD/04-ui-ux/02-menu-map.md` — 菜单系统影响 UI 架构
- `GDD/05-art-audio/02-technical-art-specs.md` — 技术美术规格已定义了渲染约束

## Output Documents

### Document 1: Architecture Overview (`GDD/06-tech-design/01-architecture.md`)

**Engine & Tech Stack**
- 引擎选择（Unity/Unreal/Godot/自研）及理由
- 编程语言选择
- 渲染管线选择（URP/HDRP/Built-in/Deferred/Forward）
- 物理引擎选择

**Key Plugins & Middleware**
- 列出所有必要的第三方插件/库：
  - 音频中间件（FMOD/Wwise/Master Audio）
  - UI 框架
  - 对话系统
  - 寻路方案
  - 本地化方案
  - 性能分析工具
  - 热更新方案（如适用）

**Code Architecture**
- 整体代码架构模式（ECS/MVP/MVC/组件化）
- 核心命名空间/模块划分
- 依赖注入方案（如适用）
- 事件系统的设计（Event Bus / ScriptableObject Event / C# event）

**Project Folder Structure**
```
Assets/
├── _Project/
│   ├── Scenes/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Gameplay/
│   │   ├── UI/
│   │   └── Systems/
│   ├── Prefabs/
│   ├── Art/
│   ├── Audio/
│   └── Data/
```

**Data Pipeline**
- 资产导入流程（美术→引擎的完整链路）
- 数据配置方式（ScriptableObject / JSON / Excel→Asset / 自研工具）
- 构建管线（Build Pipeline）

### Document 2: Data Structures & Save Systems (`GDD/06-tech-design/02-data-save-systems.md`)

**Data Schema**
- 定义核心数据结构（使用 JSON 或 C# 伪代码）：
  - PlayerData（玩家数据）
  - InventoryData（背包数据）
  - QuestData（任务状态）
  - WorldState（世界状态）
  - SettingsData（玩家设置）

```json
{
  "PlayerData": {
    "level": "int",
    "xp": "int",
    "stats": { "hp": "float", "atk": "float", "..." },
    "position": { "x": "float", "y": "float", "z": "float" },
    "unlockedAbilities": ["string_id"]
  }
}
```

**Save System Design**
- 存档方式（本地/云端/两者）
- 自动存档触发点清单
- 存档槽位数量
- 存档文件大小估算
- 存档加密策略

**Save File Versioning**
- 版本号管理策略
- 存档迁移策略（旧版本存档如何升级到新版本）
- 存档损坏检测

**Inventory Serialization**
- 物品唯一 ID 生成策略
- 物品序列化格式
- 物品耐久度/附魔等动态属性的存储方案

### Document 3: Networking Model (`GDD/06-tech-design/03-networking.md`)

> 如果游戏是纯单机，此文档仅需一行："本游戏为单机游戏，不需要网络系统。"然后跳过本节。

**Network Topology**
- 客户端-服务器 / P2P / Listen Server 的选择及理由
- 权威方（Server Authority / Client Authority）的设计：
  - 哪些逻辑在服务器运算
  - 哪些逻辑在客户端预测

**Hit Registration**
- 命中判定方式（客户端预测 + 服务器校验 / 纯服务器判定）
- 延迟补偿方案

**State Synchronization**
- 同步哪些状态（位置/动画/生命值/...）
- 同步频率
- 插值/外推策略

**Matchmaking & Lobby**
- 匹配逻辑（基于技能等级/延迟/地区）
- 房间/大厅的创建和加入流程（mermaid sequence diagram）
- 断线重连策略
- 旁观者模式设计

### Document 4: Tools Requirements (`GDD/06-tech-design/04-tools-requirements.md`)

**Essential Tools**
列出开发过程中必须要的工具：

**Level Editor Features**
- 关卡编辑器需求（如果使用 Unity 场景即关卡则说明此策略）
- 地形编辑
- 物件放置和对齐
- 敌人/触发器/收集品配置
- 关卡验证工具（检查是否有无法到达的区域等）

**Scripting / Data Configuration**
- 脚本语言选择（Lua/C# 可视化脚本）
- 数值配置表的管理方式
- 对话编辑器需求

**Procedural Generation** (如适用)
- 哪些内容会程序化生成
- 生成算法概述
- 种子系统
- 手动覆写/微调机制

**CI/CD & Build Pipeline**
- 持续集成策略
- 自动化测试需求
- 构建目标平台列表

### Document 5: Performance Targets (`GDD/06-tech-design/05-performance-targets.md`)

**Per-Platform Targets**
| 指标 | PC (推荐) | PC (最低) | Console | Mobile (高端) | Mobile (低端) |
|------|-----------|-----------|---------|---------------|---------------|
| 目标帧率 | 60 | 30 | 60 | 60 | 30 |
| 分辨率 | 1440p | 1080p | 动态 | 1080p | 720p |
| 内存预算 | 8GB | 4GB | 共享 | 2GB | 1GB |
| 加载时间 | <5s | <15s | <10s | <10s | <20s |
| 安装包大小 | <50GB | <50GB | <50GB | <2GB | <2GB |

**Rendering Budgets**
- Draw Call 上限
- 三角面数上限（屏幕内）
- 纹理内存预算
- Shader 复杂度限制

**Optimization Strategies**
- LOD 策略
- 遮挡剔除方案
- 纹理流式加载
- 对象池策略（哪些对象需要池化）
- 资源异步加载策略

## Workflow

1. **Read upstream inputs** — 基础 GDD + 所有可用上游文档（特别是 gameplay 和 art）
2. **Inventory technical needs** — 从玩法需求反推技术需求
3. **Choose tech stack** — 确定引擎、渲染管线、关键中间件
4. **Design data architecture** — 核心数据结构 + 存档方案
5. **Define network model** — 如果需要多人，详细定义网络架构
6. **Plan tooling** — 确保开发效率的工具需求
7. **Set performance budgets** — 按平台设定明确数值目标
8. **Report** — 总结关键技术决策和风险点

## Design Principles

- **选择有理由**：每个技术选型都要附带决策理由
- **数据结构先行**：存档格式和网络协议是技术设计中最难修改的部分，需要一开始就谨慎设计
- **性能预算明确到数字**："游戏应该流畅"不是性能目标。"室内场景 Draw Call ≤ 500, 室外 ≤ 1000"才是
- **考虑开发效率**：工具链设计应优先保障策划和美术的自足能力，减少对程序的依赖

## Output Rules

- 保存到 `GDD/06-tech-design/` 目录
- 数据格式用 JSON/C# 伪代码展示
- mermaid sequence diagram 用于网络流程
- 表格用于性能目标对比
- 中文输出 + 英文术语
