# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个 Unity 项目模板（template repo），用于快速初始化新的 Unity 项目。项目使用 **URP（Universal Render Pipeline）**，渲染管线版本为 17.0.4（Unity 6 / 6000.x LTS）。

## 核心架构

模板采用分层架构，`Assets/` 下的目录结构即约定：

- `Assets/$projname/Scripts/` — 游戏业务代码，按层级划分：
  - `Constants/` — 常量定义
  - `Data/` — 数据层（ScriptableObject、数据模型等）
  - `View/` — 视图层（UI/MonoBehaviour 组件）
- `Assets/Dada/` — 框架层和可复用核心脚本（非业务逻辑）
- `Assets/Config/` — 配置类 ScriptableObject 资产存放处
- `Assets/Art/` — 所有美术资源，按类型分子目录（Animations / Atlas / Audios / Fonts / Materials / Prefabs / Spines / Sprites / Textures）
- `Assets/Scenes/` — 场景文件，支持子场景（subscene）目录结构

## 关键依赖

| 包 | 用途 |
|---|---|
| `jp.hadashikick.vcontainer` (1.17.0) | DI 容器，项目核心 IoC 框架 |
| `com.cysharp.messagepipe` (1.8.1) | 消息管道，事件/消息解耦通信 |
| `com.cysharp.messagepipe.vcontainer` (1.8.1) | MessagePipe 与 VContainer 集成 |
| `com.cysharp.unitask` (2.5.10) | 零 GC 的 async/await 实现 |
| `com.cysharp.r3` (1.3.0) | 响应式编程（Rx）库 |
| `com.cysharp.zstring` (2.6.0) | 零分配字符串格式化 |
| `com.unity.addressables` (1.22.3) | 资源管理（Addressables 系统） |
| `com.esotericsoftware.spine.spine-unity` (4.2.x) | 2D 骨骼动画 |
| `com.unity.cinemachine` (2.10.3) | 相机系统 |
| `com.tayx.graphy` (3.0.5) | 性能监控（FPS/内存等） |
| `com.unity.test-framework` (1.1.33) | 单元测试框架 |

## 私有 Registry

项目配置了两个 scoped registry：
- `https://august.amberweather.com/nexus/repository/unity_group` — 内部/定制包（含 `com.aframework`、`com.amber`、`com.bframework`、`com.cframework` 等）
- `https://package.openupm.com` — OpenUPM 社区包

## 使用方式

`$projname` 是占位符，创建新项目时需重命名为实际项目名称。

该模板不需要构建/运行命令——它是一个 Unity 项目，通过 Unity Editor 打开和运行。使用 GladeKit MCP 工具与 Unity Editor 交互来完成场景编辑、脚本创建等操作。

## Git 配置

- `.gitattributes` 配置了 `text=auto`，自动处理换行符标准化
