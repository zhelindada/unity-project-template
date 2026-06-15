---
name: unity-ui-procedural
description: 纯代码构建 UI（Procedural UI Toolkit）——用 C# 流式接口替代 UI Builder/UXML 构建界面。包含 Drag/Resize 等通用 PointerManipulator。Use when 需要动态生成界面（背包/工具栏/列表）、需要纯代码工作流便于版本控制、或需要可复用的拖拽/缩放交互组件。
---

# Unity UI Toolkit Procedural UI

**Tier:** POWERFUL
**Category:** Unity / UI Systems
**Tags:** Unity, UI Toolkit, procedural, code-first, fluent API, manipulator, BEM

Build lightning-fast, highly dynamic user interfaces entirely through code. This skill enables the "Logic-First" UI workflow, perfect for complex systems like inventories, tooltips, and dynamic HUDs.

## Core Features
1. **Fluent Hiyerarşi Construction**: Use chained extension methods like `.CreateChild<T>()` and `.WithClass()` to build UI trees that read like documentation.
2. **Generic Pointer Manipulators**: Decouple interaction logic (Drag, Resize, Selection) from visuals using the `Manipulator` API.
3. **No-Builder Workflow**: Eliminates the need for complex UXML files, making UI version-control friendly and procedurally scalable.
4. **BEM Integration**: Extension methods prioritize CSS class application to maintain a clean styling system.

## Core Files (Max 3)
- `UIToolkitExtensions.cs.txt`: Generic methods for chaining element creation and styling.
- `DragManipulator.cs.txt`: A cross-platform pointer-based drag system for any VisualElement.
- `ProceduralViewExample.cs.txt`: A "Hotbar/Inventory" demo showing the power of the fluent syntax.

## Usage

### 1. Build a Panel (The Fluent Way)
```csharp
var root = GetComponent<UIDocument>().rootVisualElement;
root.CreateChild<VisualElement>( "main-menu" )
    .WithClass( "menu--active" )
    .CreateChild<Button>( "menu__btn--start" )
    .text = "Start Game";
```

### 2. Add Interactions
```csharp
var window = root.Q( "inventory-window" );
window.AddManipulator( new DragManipulator() );
```

## Key Principle
Always use **USS classes** for styling even when building procedurally. Avoid setting `.style.width` or colors directly in C# unless it is for dynamic calculations (like health bars).
