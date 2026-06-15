---
name: unity-ui-layout
description: 在 Unity 中配置 UI 元素的 RectTransform 和布局组件 —— 选择锚点/轴心模式、设置尺寸和位置、使用 LayoutGroup/LayoutElement/ContentSizeFitter 构建自适应布局。Use when 需要精准定位 UI 元素、搭建自适应列表/面板、或排查锚点/布局不生效问题。
---

为 Unity UI 元素配置正确的位置、尺寸和自适应行为。4 步流程：分析需求 → 选择模式 → 设置属性 → 添加布局组件。

**Tier:** POWERFUL
**Category:** Unity / UI Systems
**Tags:** Unity, UGUI, RectTransform, anchor, pivot, layout, responsive, adaptive

## 工作流程

按顺序执行以下阶段。

### 阶段 0：理解需求

向用户确认（或从上下文推断）以下信息：

1. 元素相对于父级的位置（顶部/底部/左侧/右侧/居中/全屏填充？）
2. 元素的尺寸行为（固定大小 or 自适应父级？）
3. 子元素是否动态变化（动态 → LayoutGroup；固定 → 手动定位）

### 阶段 1：选择锚点/轴心模式

根据需求从速查表选择对应模式，设置 RectTransform 属性。

**属性名必须用 internalName**，通过 `set_component_property` 设置，组件类型为 `RectTransform`：

| 显示名 | internalName | 示例值 |
|--------|-------------|--------|
| Anchor Min | `m_AnchorMin` | `"0,0"` |
| Anchor Max | `m_AnchorMax` | `"1,1"` |
| Anchored Position | `m_AnchoredPosition` | `"0,0"` |
| Size Delta | `m_SizeDelta` | `"1920,1080"` |
| Pivot | `m_Pivot` | `"0.5,0.5"` |

### 阶段 2：应用锚点模式

**单点锚点（Min == Max）**：
- `SizeDelta` = 实际宽高（直接控制尺寸）
- `AnchoredPosition` = Pivot 到 Anchor 的偏移

**矩形锚点（Min != Max）**：
- `SizeDelta` = 实际大小 - 锚点矩形大小。设边距时用负值 `(-left-right, -top-bottom)`
- `AnchoredPosition` = 四边到锚点矩形四边的偏移

**8 种基础模式速查**：

```
全屏填充（留边距用负 SizeDelta）:
  Anchor Min: (0,0)   Anchor Max: (1,1)    Pivot: (0.5,0.5)
  Position: (0,0)     SizeDelta: (-左-右, -上-下)

顶部居中：Anchor (0.5,1)→(0.5,1)  Pivot (0.5,1)   SizeDelta: (0, 高度)
底部居中：Anchor (0.5,0)→(0.5,0)  Pivot (0.5,0)   SizeDelta: (0, 高度)
左侧居中：Anchor (0,0.5)→(0,0.5)  Pivot (0,0.5)   SizeDelta: (宽度, 0)
右侧居中：Anchor (1,0.5)→(1,0.5)  Pivot (1,0.5)   SizeDelta: (宽度, 0)
居中对齐：Anchor (0.5,0.5)→(0.5,0.5) Pivot (0.5,0.5) SizeDelta: (宽, 高)
左上角：   Anchor (0,1)→(0,1)      Pivot (0,1)     SizeDelta: (宽, 高)
左下角：   Anchor (0,0)→(0,0)      Pivot (0,0)     SizeDelta: (宽, 高)
右上角：   Anchor (1,1)→(1,1)      Pivot (1,1)     SizeDelta: (宽, 高)
右下角：   Anchor (1,0)→(1,0)      Pivot (1,0)     SizeDelta: (宽, 高)
```

**水平/垂直拉伸模式**：

```
水平拉伸（固定高度，宽度随父级）:
  Anchor Min: (0, y)   Anchor Max: (1, y)    SizeDelta: (0, fixedHeight)

垂直拉伸（固定宽度，高度随父级）:
  Anchor Min: (x, 0)   Anchor Max: (x, 1)    SizeDelta: (fixedWidth, 0)
```

### 阶段 3：添加布局组件（当子元素需要自动排列时）

| 场景 | 方案 |
|------|------|
| 子元素数量固定、大小已知 | 手动设置 RectTransform — 无需布局组件 |
| 子元素数量动态变化 | **必须**使用 LayoutGroup |
| 父容器大小需随子元素变化 | LayoutGroup + ContentSizeFitter |
| 列表中部分固定、部分弹性 | LayoutElement（控制个别子元素） |

#### VerticalLayoutGroup — 垂直排列

通过 `add_component` 添加到父容器：

```
常用配置（表单/列表）:
  Spacing: 4
  ChildAlignment: UpperLeft
  ChildForceExpandWidth: true
  ChildForceExpandHeight: false
  ChildControlHeight: false              ← 让子元素控制自己的高度
```

#### HorizontalLayoutGroup — 水平排列

```
常用配置（工具栏/标签栏）:
  Spacing: 4
  ChildAlignment: MiddleLeft
  ChildForceExpandWidth: false           ← 子元素不自动拉伸
  ChildForceExpandHeight: true
```

#### LayoutElement — 覆写子元素尺寸

单个子元素的弹性控制：

```
固定高度元素（如标题栏）:
  set_component_property LayoutElement
    m_PreferredHeight: 40
    m_FlexibleHeight: 0

弹性填充元素（如内容区）:
  set_component_property LayoutElement
    m_PreferredHeight: 0
    m_FlexibleHeight: 1                  ← 占据所有剩余空间

固定宽度按钮:
  set_component_property LayoutElement
    m_PreferredWidth: 120
    m_FlexibleWidth: 0
```

#### ContentSizeFitter — 父容器适应子内容

ScrollView 的 Content 节点必备：

```
add_component ContentSizeFitter
  set_component_property ContentSizeFitter
    m_HorizontalFit: Unconstrained       (0)
    m_VerticalFit: PreferredSize         (2)
```

### 阶段 4：ScrollView 布局

ScrollView 用于滚动列表时，Content 节点**必须**添加正确配置：

1. `create_ui_element` type=`ScrollView` 创建
2. Content 子节点添加 `VerticalLayoutGroup`
3. Content 子节点添加 `ContentSizeFitter`，VerticalFit = `PreferredSize`
4. ScrollView 自身设为全屏填充模式（填满父容器）

**不滚动的常见原因**：
- Content 缺少 ContentSizeFitter
- Content 的 anchor 不是顶部对齐
- ScrollRect 的 Content 引用未设置

## 批量操作

创建多个独立元素时，打包到 `batch_execute` 减少往返。注意有依赖关系的调用必须顺序排列在同一个 batch 中。

## 常见陷阱

| 问题 | 原因 | 解决 |
|------|------|------|
| 元素不显示 | 被父级裁剪或尺寸为 0 | 检查 SizeDelta 和父级 Mask |
| 布局组件不生效 | 子元素未设为布局控制 | 设 ChildControlHeight/Width=true |
| 子元素被压扁 | 无 LayoutElement 且父级高度不足 | 加 LayoutElement 设 PreferredHeight |
| ScrollView 不滚动 | Content 无 ContentSizeFitter | 加 VerticalFit=PreferredSize |
| 锚点设置无效 | 用了 displayName 而非 internalName | 用 `m_AnchorMin`/`m_AnchorMax` 等 |

## 示例：编辑器主界面

```
EditorArea (VerticalLayoutGroup, Anchor 填满父级留边距)
├── TabBar (LayoutElement: PreferredHeight=40, FlexibleHeight=0)
│   └── HorizontalLayoutGroup
│       ├── ListBtn
│       └── TimelineBtn
└── ContentView (LayoutElement: FlexibleHeight=1)
    └── ScrollView → Content (VerticalLayoutGroup + ContentSizeFitter)
```

## 资源

详细布局模式参考、场景案例和决策树见捆绑的 `UI-Layout-Guide-for-AI.md`。
