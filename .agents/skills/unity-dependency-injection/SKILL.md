---
name: unity-dependency-injection
description: 依赖注入（Dependency Injection）——通过 Attribute + 反射实现 SOLID 中的依赖反转原则（DIP），解耦高层逻辑与具体实现。Use when 需要解耦模块间依赖、需要可替换的实现（武器系统更换武器类型）、或需要在 Awake 阶段自动注入依赖。
---

# Unity Dependency Injection (DI)

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, dependency injection, DI, SOLID, decoupling, attributes

A lightweight system to satisfy the "D" in SOLID. Allows classes to request dependencies via `[Inject]` without knowing who provides them.

## Core Features
1. **Attribute-Based Injection**: Use `[Inject]` to mark fields and `[Provide]` to define sources.
2. **Interface Focus**: Encourages programming to abstractions, making systems hot-swappable.
3. **Automated Wiring**: Scene-wide injection happens on Awake, ensuring dependencies are ready before Start.
4. **Diagnostic Logging**: Tracks successful injections and warns about missing providers.

## Core Files (Max 3)
- `DIAttributes.cs.txt`: Contains the marker attributes for the compiler.
- `DependencyInjector.cs.txt`: The engine that finds and wires dependencies in the scene.
- `DIExample.cs.txt`: Shows how to decouple a Hero from an AbilitySystem.

## Usage

### 1. Define an Interface
```csharp
public interface IWeapon { void Fire(); }
```

### 2. Provide the Implementation
```csharp
public class PlasmaRifle : MonoBehaviour, IWeapon {
    [Provide] public IWeapon GetWeapon() => this;
    public void Fire() { /* ... */ }
}
```

### 3. Inject into Client
```csharp
public class Player : MonoBehaviour {
    [Inject] private IWeapon currentWeapon;
    void Start() => currentWeapon.Fire();
}
```

### 4. Setup
Place a `DependencyInjector` component in your scene or persistent prefab.
