---
name: gdd-to-prototype
description: 根据项目内的 GDD 设计文档和游戏框架，自动生成可玩原型——创建核心玩法循环、灰盒关卡、占位资产、基础 UI 和输入控制。Use when 需要从 GDD 快速生成可验证的可玩原型，验证核心玩法假设，或搭建垂直切片的技术骨架。
---

# GDD to Prototype

**Tier:** POWERFUL
**Category:** Game Design / Prototyping
**Tags:** GDD, prototype, rapid prototyping, vertical slice, MVP, greybox

## Overview

读取 GDD 文档集（`GDD/` 目录 + gdd-orchestrator 产出），检测项目游戏框架，自动生成可玩原型。输出是可运行的代码、场景和资产，不是文档。

触发短语："生成原型"、"搭建垂直切片"、"根据 GDD 做可玩版本"。

## Workflow

### Step 1: 框架检测
详见 `references/framework-detection.md`。
- Unity → 使用 gladekit-unity MCP 工具操作场景
- Unreal → 输出 Blueprint/C++ 代码 + 手动操作指引
- Godot → 输出 GDScript/C# 代码 + 场景文件
- 纯代码框架 → 直接生成代码文件

### Step 2: 读取 GDD
重点提取以下文档：
- `02-gameplay/` → 核心玩法循环、玩家机制、战斗系统
- `03-level-design/` → 关卡布局、敌人配置
- `04-ui-ux/` → 最小 HUD 需求
- `06-tech-design/` → 架构约束、输入系统选择

### Step 3: 确定原型范围
按 `references/prototype-scope-guide.md` 确定。核心原则：**只做验证核心假设所需的最小集合**。
1 个角色（核心机制） + 1 个灰盒关卡 + 1-2 种敌人 + 1 个最小 HUD。不做菜单/设置/存档。

### Step 4: 生成原型

**Unity（有 MCP 工具）：**
按 `references/unity-prototype-patterns.md` 的脚本模板和场景搭建序列执行：
创建 Ground/Player/Enemies/UI_Canvas/GameManager → 生成脚本（PlayerController、CameraFollow、EnemyAI、GameManager）→ 设置输入/物理/碰撞 → 编译 → Play Mode 验证。

**非 Unity 框架：**
输出完整文件方案（路径 + 代码），用户自行放入项目。

4.1 生成方法
优先生成所有UI界面资源，并绑定所有资源

### Step 5: 验证清单
- [ ] 场景可运行不报错
- [ ] 玩家可移动/跳跃/核心交互
- [ ] 核心 Gameplay 循环可体验
- [ ] Camera 跟随正常、基础 UI 显示正确
- [ ] 帧率 ≥ 30fps（未优化状态）

## Output

```
Assets/_Prototype/
├── Scenes/Prototype_Main.unity
├── Scripts/{Core/GameManager, Player/PlayerController, Camera/CameraFollow, Enemy/EnemyAI}.cs
├── Prefabs/ (生成的 prefab)
└── UI/ (生成的 Canvas/HUD)
```

## Rules

- 单个脚本 ≤ 100 行；中文注释 + 英文标识符
- 不做：存档、菜单、设置、本地化、音效（核心验证除外）、性能优化（标 `// TODO` 即可）
- 全部使用内置 Primitive（Cube/Sphere/Capsule/Plane）作为占位资产
- 标注所有推演假设：`> 💡 假设: ...`

## References

- [Unity Prototype Patterns](references/unity-prototype-patterns.md) — Unity 脚本模板与场景搭建序列
- [Prototype Scope Guide](references/prototype-scope-guide.md) — 从 GDD 确定原型范围
- [Framework Detection](references/framework-detection.md) — 框架检测与适配
