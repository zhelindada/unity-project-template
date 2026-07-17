# AGENTS.md

This file provides guidance to Codex (Codex.ai/code) when working with code in this repository.

## 核心架构

详见.Codex/conventions/project-structrue.md

## 技术栈

详见.Codex/conventions/tech-stack.md

## CSS 布局与间距规范

详见.Codex/conventions/layout-spacing-system.md

生成 HTML/CSS 页面、组件展示或 UI 原型时必须严格遵循该规范中的 8-point grid spacing scale、Card 规格、Section 结构和 Anti-Squish 规则。

# 游戏策划案

策划案存储在 Asset/$projname/Designs目录下，所有游戏设计包括prototype相关文档和产出都会在这个目录或子目录下

## 使用方式

`$projname` 是占位符，如需初始化项目是需重命名为实际项目名称。

该模板不需要构建/运行命令——它是一个 Unity 项目，通过 Unity Editor 打开和运行。使用 GladeKit MCP 工具与 Unity Editor 交互来完成场景编辑、脚本创建等操作。

## Git 配置

- `.gitattributes` 配置了 `text=auto`，自动处理换行符标准化
