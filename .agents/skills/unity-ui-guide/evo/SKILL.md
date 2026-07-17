---
name: unity-ui-guide-evo
description: "[Evo UI] Evo UI 框架组件参数详解与创建规范——基于 uGUI + TextMesh Pro 的第三方 UI 框架，提供 Styler 全局样式系统、预配置 UI 元素、图标系统等。Use when 项目引入了 Evo UI 包，需要创建/修改 Evo UI 组件、配置 Styler 全局主题、或排查 Evo UI 相关问题。"
---

# Evo UI 组件参数详解与创建规范

**UI Framework:** Evo UI (基于 uGUI + TextMesh Pro)

## Overview

Evo UI 是 Michsky 开发的 Unity 第三方 UI 框架，基于 uGUI 和 TextMesh Pro 构建。它提供预配置的 UI 元素、集中的 Styler 样式架构，以及图标系统等扩展功能。

> 在使用 Evo UI 之前，需要熟悉 Unity 原生 uGUI 系统。Evo UI 扩展而非替代 uGUI。

## 兼容性

| 项目 | 要求 |
|------|------|
| Unity 版本 | Unity 6 (6000.x) 或 Unity 2022.3 |
| 输入系统 | 支持 New Input System 和旧 Input Manager |
| 渲染管线 | Built-in、URP、HDRP 均兼容 |
| 依赖 | 2D Sprite + Unity UI (uGUI) + TextMesh Pro |

## 快速开始

1. **了解架构概念** → [guide.md](guide.md)
2. **创建/修改 UI 元素** → 使用 Create 菜单或 Add Component（Evo → UI）
3. **配置 Styler 样式** → Tools → Evo UI → Open Styler Browser

## 核心系统

| 系统 | 说明 | 详见 |
|------|------|------|
| **Styler** | 全局样式系统，集中管理颜色/字体/精灵/音频 | [guide.md#styler-系统](guide.md) |
| **UI Elements** | 预配置 UI 控件（Dropdown 等） | [guide.md#ui-元素](guide.md) |
| **Icons** | 内置图标系统 + Icon Selector 浏览器 | [guide.md#图标系统](guide.md) |
| **Procedural Rect** | 程序化生成的矩形形状 | [guide.md#procedural-rect](guide.md) |
| **Evo Localization** | 本地化集成 | [guide.md#本地化](guide.md) |

## 创建 UI 的两种方式

### 方式 1：Create 菜单（推荐）
- 顶部菜单栏或右键 Hierarchy → Evo UI 子菜单
- 自动组装完整的 UI 元素，引用已正确配置
- 适用于：所有预配置 UI 元素

### 方式 2：Add Component 菜单
- 选中对象 → Add Component → Evo → UI
- 需手动分配所有必要引用
- 适用于：对已有对象追加 Evo UI 功能

## 定制化三种策略

| 策略 | 说明 |
|------|------|
| **Styler 驱动** | 全局样式自动应用到所有 Styler 连接的对象 |
| **手动编辑** | 移除 Styler Object 组件或启用 Use Custom 选项后直接调属性 |
| **混合模式** | 部分对象用 Styler，部分手动编辑 |

## 自定义编辑器 GUI

可通过 **Tools → Evo UI → Disable Custom Editor** 关闭自定义编辑器 GUI。但部分复杂组件（如 List View、UI Animator）会忽略此设置。

> 每个 Evo UI 组件都有 Help 图标，点击可打开对应的官方文档页面。
