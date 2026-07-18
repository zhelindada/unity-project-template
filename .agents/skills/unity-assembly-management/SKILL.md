---
name: unity-assembly-management
description: 程序集定义管理（Assembly Definitions）——通过 .asmdef 文件管理项目模块边界，加速编译，强制模块架构规范。支持 Runtime/Editor/Tests 分离。Use when 需要拆分大型项目为独立模块、需要加速编译时间、或需要强制模块依赖规范。
---

# Unity Assembly Management

**Tier:** POWERFUL
**Category:** Unity / Architecture
**Tags:** Unity, assembly definition, asmdef, modular architecture, compilation, project structure

Manage project boundaries using Assembly Definitions (.asmdef) for faster compile times and modular architecture. Based on the patterns by Adam Myhre.

## Core Features
- **Hybrid Approach**: Supports both "Pro Path" (Explicit GUIDs, Version Defines) and "Light Path" (Auto-referenced modules).
- **Rule Enforcement**: Enforces "Runtime never depends on Editor" and inward dependency flows.
- **Scaffolding**: Automated creation of Runtime/Editor/Tests assembly splits.

## Core Files
- `AsmdefTemplate.json.txt`: Flexible template for various assembly configurations.
- `AsmdefScaffolder.cs.txt`: Editor utility to generate standard module structures.
- `AsmdefValidator.cs.txt`: Script to verify architectural boundaries.

## Usage
1. Use `AsmdefScaffolder` to create a new module partition.
2. Define dependencies explicitly in the Inspector using GUIDs.
3. Validate regularly to ensure no architectural drift has occurred.
