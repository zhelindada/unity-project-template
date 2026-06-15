---
name: unity-decorator-pattern
description: 装饰器模式（Decorator）——动态包裹对象来修改行为，完美适配 Buff/Debuff 系统和数值修正链。Use when 需要叠加数值修正（攻击力+20%+10 固定值）、运行时动态增减 Buff/Debuff、或需要可组合的数值计算链。
---

# Unity Decorator Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, decorator, buff, debuff, stat modifier, composable

## Overview

装饰器模式通过包裹对象动态修改数值，各修正器独立并可叠加。在 Unity 中通过 `IDecorator<T>` 泛型接口实现百分比/固定值修正链。

## Use this skill when

- 需要叠加多个数值修正（攻击力 +20% 百分比 +10 固定值）
- 运行时动态增减 Buff/Debuff
- 需要可组合的数值计算链，每个修正器独立且可任意排序
- 不同伤害类型共享同一修正器逻辑

## Do not use this skill when

- 只有单一修正 → 直接计算即可
- 修正逻辑非常简单且不会扩展 → 简单的 `float` 乘法即可

## Core Features

1. **Generic Interface**: `IDecorator<T>` 适用于任意数值类型
2. **Stat Modifiers**: 百分比修正 + 固定值修正
3. **Composable**: 任意叠加和排序

## Core Files (1 file)

- `Decorator.cs.txt`: 完整装饰器模式（接口 + 基类 + 修正器）

## Usage

```csharp
IDecorator<float> damage = new BaseStat(100);
damage = new PercentageModifier(damage, 0.2f); // +20%
damage = new FlatModifier(damage, 10);          // +10
float final = damage.GetValue();                // 130
```

## Reference Material

- [Implementation Guide](references/guide.md)
