---
name: unity-mvc-pattern
description: MVC / MVP 架构模式——通过 Model-View-Controller/Presenter 分离数据、界面和逻辑。基于 ObservableList 响应式集合。Use when 复杂 UI（背包/技能树/设置）、高级游戏管理器、或需要独立修改界面和逻辑的场景。
---

# Unity MVC / MVP Pattern

**Tier:** POWERFUL
**Category:** Unity / Design Patterns
**Tags:** Unity, design pattern, MVC, MVP, separation of concerns, UI architecture, observable

This skill implements the architectural principle of "Separation of Concerns." It divides your code into three distinct layers: Model (Data), View (Visuals), and Presenter (Logic bridge).

## Core Features
1. **ObservableList**: A reactive collection that notifies listeners (the View) when data changes.
2. **Passive View**: Views are MonoBehaviours that only care about HOW to display data, not WHERE it comes from.
3. **Mediated Logic**: The Presenter handles the "Wiring" and lifecycle, keeping Data and Visuals completely decoupled.
4. **MMO Hotbar Pattern**: A concrete implementation of the multi-slot observable layout described in the reference text.

## Core Files (Max 3)
- `ObservableList.cs.txt`: Generic collection wrapper with structural change notifications.
- `GenericPresenter.cs.txt`: Base class for creating M-V-P relationships without boilerplate.
- `MMOHotbarExample.cs.txt`: Practical application showing data flowing from a Model to a UI view.

## Usage

### 1. The Model (Data)
Contains your state using `Observable<T>` or `ObservableList<T>`. It does not know about Unity UI.

### 2. The View (Visuals)
A MonoBehaviour that exposes public methods for updating its state (e.g., `SetHealth(float val)`).

### 3. The Presenter (Bridge)
```csharp
public class MyPresenter : Presenter<MyModel, MyView> {
    protected override void Bind() {
        model.health.OnChanged += view.SetHealth;
    }
}
```

## When to Use
Use this pattern for complex UI (Inventories, Skill Trees, Settings) or high-level game managers where logic and visuals are likely to change independently.
