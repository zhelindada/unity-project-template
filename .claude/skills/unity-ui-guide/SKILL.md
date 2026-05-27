---
name: unity-ui-guide
description: Unity UI 组件参数详解与创建规范——Canvas、CanvasScaler、GraphicRaycaster、Image、Button、TextMeshPro、Slider、Toggle、Dropdown、InputField、ScrollRect 等全部 UI 组件的参数说明与最佳实践。Use when 需要创建/修改 Unity UI、排查 UI 不显示/不可交互问题、或需要参考 UI 组件参数。
---

# Unity UI 组件参数详解与创建规范

## 核心工具规则

> **UI 操作必须使用专用工具**：
> - **`set_ui_properties`** — 修改 UI 元素的 RectTransform、Image 颜色/尺寸、Text 内容、Layout 属性等
> - **`get_ui_element_info`** — 读取 UI 元素的所有属性和事件绑定
> 
> **禁止**用 `set_component_property` / `get_component_inspector_properties` 操作 UI 元素。这两个通用工具不理解 UI 联动关系（如 Slider 驱动 Handle、LayoutGroup 控制子节点），容易导致修改无效或被覆盖。
> 
> `set_component_property` 仅用于非 UI 组件（Rigidbody、Collider 等）或 Slider/Button 的组件级引用字段（fillRect、onClick 等）。

## 快速开始

1. **找概念参数配置** → [config/ui-config.json](config/ui-config.json)
2. **找组件参数配置** → 从下方目录找到目标组件文件夹
3. **按流程执行/修改UI元素** → 每个文件夹内 `guide → plan → generate → check`

## 组件目录

| 组件 | 文件夹 | 说明 |
|------|--------|------|
| Canvas 基础设施 | [canvas/](canvas/guide.md) | Canvas, CanvasScaler, GraphicRaycaster |
| 文本显示 | [text/](text/guide.md) | TextMeshPro 文本 |
| 图片显示 | [image/](image/guide.md) | Image, RawImage |
| 按钮 | [button/](button/guide.md) | Button 交互 |
| 滑动条 | [slider/](slider/guide.md) | Slider 滑动条 |
| 开关 | [toggle/](toggle/guide.md) | Toggle 开关 |
| 输入框 | [inputfield/](inputfield/guide.md) | TMP_InputField |
| 下拉框 | [dropdown/](dropdown/guide.md) | TMP_Dropdown |
| 滚动视图 | [scrollview/](scrollview/guide.md) | ScrollRect + Scrollbar |
| 布局 | [layout/](layout/guide.md) | LayoutGroup, ContentSizeFitter, LayoutElement |
| UI 模式 | [patterns/](patterns/guide.md) | Modal, Sidebar, List 等 12 种布局模式 |

## 跨组件参考

- 配置说明 → [config/ui-config.json](config/ui-config.json)
- 踩坑经验 → [pitfalls.md](pitfalls.md)

## 每个组件文件夹结构

```
{component}/
├── guide.md      ← 组件概述、参数参考、注意事项
├── plan.md       ← 规划：何时用、需要什么、布局考量
├── generate.md   ← 生成：创建步骤、参数设置、MCP 工具用法
└── check.md      ← 检查：验证清单、常见错误
```

## 配置速查

### Canvas

| 字段 | 值 | 说明 |
|------|-----|------|
| `renderMode` | `ScreenSpaceOverlay` | 不改 |
| `scaler.uiScaleMode` | `ScaleWithScreenSize` | 不改 |
| `scaler.referenceResolution` | `1920,1080` | 默认值 |
| `scaler.matchWidthOrHeight` | 0.5 | 竖屏=0，横屏=1 |

### 字号

| 角色 | 默认 |
|------|------|
| title | 36 |
| heading | 28 |
| body | 24 |
| caption | 18 |
| button | 22 |

### 色板（暗色主题）

| 语义色 | 值 |
|--------|-----|
| panel | `0.1,0.1,0.12,0.95` |
| overlay | `0,0,0,0.5` |
| button | `0.25,0.25,0.3,1` |
| textPrimary | `0.9,0.9,0.9,1` |
| textSecondary | `0.6,0.6,0.65,1` |
| sliderTrack | `0.2,0.2,0.22,1` |
| sliderFill | `0.3,0.6,0.9,1` |

### 硬性约束

| 约束 | 阈值 |
|------|------|
| 文字-背景对比度 | ≥ 3.0 |
| 可点击元素最小尺寸 | ≥ 24px |
| 字号下限 | ≥ 12px |
| 元素可见 alpha | ≥ 0.3 |
| 正文可读 alpha | ≥ 0.8 |
| 侧边栏最大宽度 | ≤ 父级 30% |
| 弹窗最大尺寸 | ≤ 父级 80% |
