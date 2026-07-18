---
name: unity-abstract-factory
description: 抽象工厂模式（Abstract Factory）——创建一族相关对象，无需指定具体类。支持 ScriptableObject 组合式和简单 Prefab 实例化两种路径。Use when 需要创建一族相关对象（武器+防具+配件）、需要根据配置动态选择 Prefab 生成、或需要统一的对象创建入口。
---

# Unity Abstract Factory

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, abstract factory, ScriptableObject, prefab, object creation

## Overview

抽象工厂模式用于创建一族相关对象而无需指定具体类。在 Unity 中，通过 `IFactory<T>` 泛型接口 + ScriptableObject 实现数据驱动的对象家族创建。

## Use this skill when

- 需要创建一族相关对象（如不同品质的武器 + 防具 + 配件）
- 需要根据配置数据动态选择 Prefab 生成
- 需要统一的对象创建入口，方便后续替换生成逻辑
- 需要在 Inspector 中配置工厂而无需写代码

## Do not use this skill when

- 只需要简单的 `Instantiate(prefab)` → 直接用 Prefab 工厂即可
- 只有一个对象类型 → 用 Builder 或直接 Prefab 实例化

## Core Features

1. **Generic Interface**: `IFactory<T>` 类型安全的创建接口
2. **SO-Based Factories**: ScriptableObject 驱动的数据化对象家族
3. **Simple Prefab Factory**: 快速 GameObject 生成

## Core Files (2 files)

- `Factory.cs.txt`: 完整工厂模式（接口 + SO 基类 + Prefab 工厂）
- `EquipmentFactory.cs.txt`: 示例——按品质创建不同装备

## Usage

复杂对象家族继承 `Factory<T>`，简单生成用 `PrefabFactory`。

## Reference Material

- [Implementation Guide](references/guide.md)
