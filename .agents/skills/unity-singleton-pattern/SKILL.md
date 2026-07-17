---
name: unity-singleton-pattern
description: 单例模式（Singleton）——全局管理器访问与严格生命周期控制。支持 Standard（单场景）/ Persistent（跨场景）/ Regulator（热替换）三种变体。Use when 需要全局系统（GameManager/UIManager/InputManager）、需要跨场景持久化对象、或需要热替换全局系统。
---

# Unity Singleton Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, singleton, global manager, lifecycle, persistent

A robust suite of Singleton variations for global systems. Adheres to the Ohm-Yura project rule: "Use Singletons ONLY for global systems (GameManager, UIManager, InputManager)."

## Core Features
1. **Singleton<T>**: Standard management. Destroys new duplicates to keep the original.
2. **PersistentSingleton<T>**: Extends Singleton with Don't Destroy On Load (DDOL) for cross-scene state.
3. **RegulatorSingleton<T>**: Advanced inverse logic. Destroys OLD instances and keeps the NEWEST (useful for hot-swapping).
4. **StaticInstance<T>**: Lightweight global access without duplication enforcement.

## Core Files (Max 3)
- `SingletonBase.cs.txt`: Generic base classes for classic and persistent variants.
- `SingletonRegulator.cs.txt`: Logic for the time-stamped regulator variant.
- `SingletonExample.cs.txt`: Concrete examples for Game and UI managers.

## Usage

### 1. Standard Manager
```csharp
public class UIManager : Singleton<UIManager> { }
// Use via: UIManager.Instance.Show();
```

### 2. Persistent Manager
```csharp
public class InventoryManager : PersistentSingleton<InventoryManager> { }
// Survives scene changes.
```

### 3. Regulator (System Swap)
```csharp
public class MusicSystem : RegulatorSingleton<MusicSystem> { }
// Spawning a new one will automatically kill the old one.
```

## Key Principle
Always use the most restrictive version. If it doesn't need to persist cross-scene, use `Singleton<T>`. If you just need a reference but don't mind duplicates, use `StaticInstance<T>`.
