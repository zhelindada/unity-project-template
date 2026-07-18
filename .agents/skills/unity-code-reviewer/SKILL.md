---
name: unity-code-reviewer
description: 专业 Unity C# 代码审查器——检测反模式、性能陷阱，强制执行项目架构规范。Use when 需要审查 Unity 代码质量、检查性能问题（GC/Update/Memory）、或验证代码是否符合项目架构规范。
---

# Unity Code Reviewer

**Tier:** POWERFUL
**Category:** Unity / Quality Assurance
**Tags:** Unity, code review, performance, anti-patterns, static analysis, QA

## Overview

专为 Unity 项目设计的代码审查技能。基于 5 Pillar 原则（Performance / Memory / Modernity / Architecture / Minimalism）检测常见问题，并将次优代码直接映射到对应的"正确写法"和已有的 Pattern Skills。

## Use this skill when

- 审查新提交的 Unity C# 代码
- 排查性能热点代码（Update 中 GC、Find 滥用等）
- 检查代码是否遵循项目架构规范
- 需要自动扫描大目录输出问题报告

## Do not use this skill when

- 非 Unity 项目 → 用通用 code review 工具
- 只是格式化或命名风格检查 → 用 .editorconfig
- 审查 Shader/HLSL 代码 → 需要 Shader 专用检查

## Core Features

1. **Diagnostic Scanning**: 正则表达式自动检测常见 Unity 性能错误（如 `Camera.main` 每帧调用、`GetComponent` 在 Update 中）
2. **Review Protocols**: 5 Pillar 规则集——Performance（避免每帧分配）、Memory（装箱/GC）、Modernity（C# 7+ 特性）、Architecture（模块边界）、Minimalism（删无用代码）
3. **Corrective Intelligence**: 直接将次优代码映射到"正确写法"和已有 Pattern Skills

## Core Files

- `DiagnosticScanner.py.txt`: 自动化检测引擎
- `ReviewProtocols.md.txt`: 审查规则逻辑文档
- `CorrectMethods.md.txt`: 优化参考手册

## Usage

说"Review [文件名]"或"扫描 [目录] 输出报告"。返回按 Severity 排序的问题列表 + 修复建议。
