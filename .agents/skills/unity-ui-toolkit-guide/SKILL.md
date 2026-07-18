---
name: unity-ui-toolkit-guide
description: Two-stage Unity UI Toolkit development — UI planning/building then MVVM+Presenter logic scripts. Use when working with UIDocument, UXML layouts, USS stylesheets, or UI Toolkit controller scripts in Unity 6 projects.
---

Unity UI Toolkit 开发向导，分两阶段执行。目标 Unity 6。

## 前置条件

- Unity 项目已初始化
- `Assets/Designs/`（或 `Assets/{ProjectName}/Designs/`）下存在 Markdown 设计文档
- 项目规范文件（`ui-conventions.md`、`code-style.md`）已存在于 Design 目录下，skill 只读取不生成

## 阶段 1：UI 规划与构建

### 1.1 读取设计文档

扫描 `Assets/Designs/` 或 `Assets/{ProjectName}/Designs/`，读取页面/面板清单、控件层级树、交互事件列表。

### 1.2 编写 UI 计划

输出 UI 实现计划，至少包含：
- 页面/面板清单及层级关系
- 每个面板的控件树（控件类型 + `name` + 关键属性）
- 数据流向（哪些控件需要 binding，binding 到哪个 ViewModel）
- 动画触发点标记（显示/隐藏/过渡）

### 1.3 生成 UXML + USS

直接输出 Unity 6 标准的 UXML 和 USS（不用 HTML 转换）。遵循项目规范文件和 [uxml-uss-format.md](references/uxml-uss-format.md) 的约定。

### 1.4 迭代

在 Unity Editor 中预览效果，调整布局和样式直到视觉确认。

---

## 阶段 2：逻辑脚本

### 2.1 设计 ViewModel

按 [mvvm-pattern.md](references/mvvm-pattern.md) 为每个页面创建 ViewModel，暴露可绑定属性（`[CreateProperty]`），不引用 UI 元素。

### 2.2 设计 Presenter

Presenter 挂载到 GameObject，持有 `UIDocument` 引用。负责 ViewModel 管不了的：
- **动画控制** — USS transition 或 tween engine（参见 [animation-approach.md](references/animation-approach.md)）
- **嵌套弹窗** — tooltip 叠 tooltip、面板开面板的显示时序
- **跨模块交互** — 涉及多个 ViewModel 的操作
- **Button 回调** — `RegisterCallback<ClickEvent>()`（Button clickable 不支持 DataBinding）

### 2.3 脚本组织

```
Assets/Scripts/UI/
├── ViewModels/       # 纯数据层，不引用 UI 元素
├── Presenters/       # 挂载到 GameObject，持有 UIDocument 引用
├── Components/       # 可复用 UI 组件（自定义 VisualElement）
└── Data/             # Model / ScriptableObject
```

---

## 核心规则

以下规则在任何阶段都不能违反：

### USS 引用写在 UXML 中

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements">
    <Style src="project://database/Assets/.../FooUI.uss?..."/>
    <ui:VisualElement class="root">...</ui:VisualElement>
</ui:UXML>
```

### 样式写在 USS 中，不写 inline

只在 C# 中用 `AddToClassList()`；仅动态值（进度条宽度、动画 transform）允许 inline style。

### 中文文本用 Write 工具写入

MCP 的 `create_script`/`modify_script` 通过 JSON 序列化会把非 ASCII 字符转成 `\uXXXX` 转义序列，Unity 渲染为乱码。含中文的 `.uxml`/`.uss`/`.cs` 必须用 Write 工具写入原始 UTF-8。

## 关键约束

- **不用 HTML 转换** — 直接写 UXML/USS
- **单 UXML 文件** — 所有页面和面板写在一个 UXML 中。页面以 `name` 区分（如 `Page_MainMenu`、`Page_Map`），默认 `display:none`，由代码通过 name 查找并切换显示。禁止拆分页面到多个 UXML 文件。
- **动画不绑 binding** — MVVM 管状态同步，Presenter 管动画时序
- **规范只读不写** — 不生成项目规范文件
- **Unity 6 标准** — 使用最新 UXML/USS 语法和 runtime binding API

## 参考

- [UXML/USS 规范](references/uxml-uss-format.md)
- [MVVM 模式](references/mvvm-pattern.md)
- [动画方案](references/animation-approach.md)
