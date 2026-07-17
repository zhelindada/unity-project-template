---
name: unity-composition-pattern
description: 组合模式（Composition over Inheritance）——用 ScriptableObject 配置 + C# Tuple 替代深层继承，通过 Has-A 关系构建模块化对象。Use when 继承层次过深（Vehicle→Car→ElectricCar）、需要组合而非继承来复用行为、或 ScriptableObject 数据驱动的实体配置。
---

# Unity Composition Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, composition, ScriptableObject, inheritance, modular

A fundamental design skill to prevent "Inheritance Hell." This pattern replaces 10 specific classes (e.g., RedStone, GoldStone) with one generic class that composes behaviors from ScriptableObject assets.

## Core Features
1. **Has-A Logic**: Objects define their properties through members (Configs) rather than their base class type.
2. **SO-to-Runtime Bridge**: Uses ScriptableObjects as static data templates that generate dynamic runtime instances.
3. **Tuple Deconstruction**: Uses C# ValueTuples to return multi-part data from composite objects without extra boilerplate.
4. **Hierarchy Flattening**: Drastically reduces the number of C# files needed by offloading variations to Project Assets.

## Core Files (Max 3)
- `EntityConfigSO.cs.txt`: Base class for creating interchangeable data packets.
- `ModularEntity.cs.txt`: The generic container that holds one or more configurations.
- `CompositionExample.cs.txt`: Demonstrates resource harvesting with Tuple destructuring.

## Usage

### 1. Identify "Is-A" vs "Has-A"
- Bad: `ElectricCar : Car : Vehicle` (Inheritance)
- Good: `Vehicle` has an `EngineConfig` (Composition)

### 2. Multi-Value Returns (Tuples)
Instead of internal state-tracking:
```csharp
public (int gold, int weight) Gather() => (10, 5);
// Client destructures instantly:
var (g, w) = node.Gather();
```

### 3. Strategy via SO
Attach different `EntityConfigSO` assets to the same `ModularEntity` to change its identity in the Inspector.
