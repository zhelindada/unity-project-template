# Unity UI 布局指导文档（大模型版）

本文档指导大模型如何在 Unity 中正确搭建 UI 布局，涵盖 RectTransform 属性、布局组件和常见页面模式。每个模式均给出可直接使用的参数组合。

---

## 1. 核心概念速查

### 1.1 Canvas —— 所有 UI 的根

- 新建 Canvas 默认 `Render Mode = Screen Space Overlay`，参考分辨率 1920×1080
- Canvas 自身的 RectTransform **不可修改**（由 Canvas Scaler 控制）
- Canvas 下必须有一个 EventSystem（没有则用 `create_event_system` 创建）

### 1.2 RectTransform 四要素

| 属性 | 类型 | 含义 |
|---|---|---|
| **Anchor Min / Max** | Vector2 (0~1) | 锚点矩形，决定元素相对于父级的位置参考点和伸缩方式 |
| **Pivot** | Vector2 (0~1) | 元素自身的轴心点，旋转和缩放围绕此点，（0.5,0.5）= 中心 |
| **Anchored Position** | Vector2 (px) | 轴心相对于锚点矩形的偏移。**当 anchor 为单点时直接等于位置；当 anchor 为矩形时为上下左右边距** |
| **Size Delta** | Vector2 (px) | 元素大小。**当 anchor 为单点时 = 宽高；当 anchor 为矩形时 = 相对于锚点矩形的增减量** |

### 1.3 Anchor 单点 vs 矩形

```
单点 anchor（Min == Max）：
  SizeDelta = 实际宽高
  AnchoredPosition = Pivot 到 Anchor 的偏移

矩形 anchor（Min != Max）：
  SizeDelta = 实际大小 - (AnchorMax - AnchorMin) * 父级宽高
  等价于：元素四边到锚点矩形四边的偏移
  设 left/right/top/bottom 边距时 SizeDelta = (-left-right, -top-bottom)
```

---

## 2. 常见布局模式速查表

### 模式 A：全屏拉伸（填充父级，留边距）

**用途**：主内容区、ScrollView 容器、面板背景

```
Anchor Min: (0, 0)
Anchor Max: (1, 1)
Pivot:      (0.5, 0.5)
Position:   (0, 0)
Size Delta: (-left-right, -top-bottom)   ← 负值 = 留边距
```

**示例**：填充整个 Canvas，左右各留 100px，上下留 100px
```
Size Delta: (-200, -200)
```

### 模式 B：顶部居中条（Top Bar）

**用途**：顶栏、标题栏、工具栏

```
Anchor Min: (0.5, 1)
Anchor Max: (0.5, 1)
Pivot:      (0.5, 1)
Position:   (0, 0)
Size Delta: (width, height)              ← width=0 表示撑满父级宽度
```

**示例**：高 60px，撑满宽度
```
Size Delta: (0, 60)
```

### 模式 C：左侧面板

**用途**：侧边栏、导航面板、属性面板

```
Anchor Min: (0, 0.5)
Anchor Max: (0, 0.5)
Pivot:      (0, 0.5)
Position:   (0, 0)
Size Delta: (width, height)              ← height=0 表示撑满父级高度
```

### 模式 D：右侧面板

**用途**：Inspector、详情面板

```
Anchor Min: (1, 0.5)
Anchor Max: (1, 0.5)
Pivot:      (1, 0.5)
Position:   (0, 0)
Size Delta: (width, 0)
```

### 模式 E：居中元素

**用途**：弹窗、对话框、居中按钮

```
Anchor Min: (0.5, 0.5)
Anchor Max: (0.5, 0.5)
Pivot:      (0.5, 0.5)
Position:   (0, 0)
Size Delta: (width, height)
```

### 模式 F：底部居中条

**用途**：底栏、状态栏

```
Anchor Min: (0.5, 0)
Anchor Max: (0.5, 0)
Pivot:      (0.5, 0)
Position:   (0, 0)
Size Delta: (0, height)
```

### 模式 G：水平拉伸（固定高度，宽度自适应）

**用途**：横跨左右的条状区域

```
Anchor Min: (0, y)
Anchor Max: (1, y)
Pivot:      (0.5, 0.5)
Position:   (0, 0)
Size Delta: (0, fixedHeight)             ← 宽度跟随父级
```

### 模式 H：垂直拉伸（固定宽度，高度自适应）

```
Anchor Min: (x, 0)
Anchor Max: (x, 1)
Pivot:      (0.5, 0.5)
Position:   (0, 0)
Size Delta: (fixedWidth, 0)              ← 高度跟随父级
```

---

## 3. 布局组件使用指南

### 3.1 何时使用布局组件 vs. 手动定位

| 场景 | 方案 |
|---|---|
| 子元素数量固定、大小已知 | 手动设置 RectTransform + Anchor |
| 子元素数量动态变化 | **必须使用** LayoutGroup |
| 子元素需要自动排列（列表、标签栏等） | LayoutGroup |
| 父容器大小需要随子元素变化 | LayoutGroup + ContentSizeFitter |
| 列表中有部分元素需要固定大小，部分需要伸缩 | LayoutElement（控制个别子元素） |

### 3.2 HorizontalLayoutGroup

**用途**：子元素水平排列（按钮栏、标签页、工具栏）

```yaml
添加方式: add_component -> HorizontalLayoutGroup
关键属性:
  Spacing: 2~8              # 子元素间距（常用 4）
  Child Alignment: MiddleCenter / MiddleLeft
  Child Force Expand Width: true   # 子元素均匀拉伸填满
  Child Force Expand Height: true  # 子元素高度拉伸
  Child Control Width: true        # 控制子元素宽度
  Child Control Height: true       # 控制子元素高度
  Reverse Arrangement: false
```

**常见配置**：
```
# 工具栏（按钮均匀分布）
Spacing: 4
Child Alignment: MiddleLeft
Child Force Expand Width: false
Child Force Expand Height: true

# 等宽标签页（两个按钮各占 50%）
Spacing: 2
Child Alignment: MiddleCenter
Child Force Expand Width: true
```

### 3.3 VerticalLayoutGroup

**用途**：子元素垂直排列（表单、列表、侧边面板）

```yaml
添加方式: add_component -> VerticalLayoutGroup
关键属性:
  Spacing: 2~6              # 子元素间距（常用 4）
  Child Alignment: UpperCenter / UpperLeft
  Child Force Expand Width: true
  Child Force Expand Height: false  ← 通常不强制拉伸子元素高度
  Child Control Width: true
  Child Control Height: true
```

**常见配置**：
```
# 表单面板（元素从上到下，各自保持自身高度）
Spacing: 4
Child Alignment: UpperLeft
Child Force Expand Width: true
Child Force Expand Height: false
Child Control Height: false    ← 让子元素控制自己的高度

# 标签页内容（元素均匀分布）
Spacing: 0
Child Alignment: UpperCenter
Child Force Expand Height: true
```

### 3.4 LayoutElement

**用途**：覆写 LayoutGroup 对某个子元素的尺寸控制

```yaml
添加方式: add_component -> LayoutElement
关键属性:
  Preferred Width / Height: 优先尺寸（LayoutGroup 会尽量满足）
  Flexible Width / Height: 弹性系数（0 = 不伸缩，1+ = 按比例分配剩余空间）
  Min Width / Height: 最小尺寸
  Ignore Layout: 是否跳过布局
```

**常见配置**：
```
# 固定高度元素（如标题栏、标签栏）
Preferred Height: 40
Flexible Height: 0

# 弹性填充元素（如内容区、ScrollView）
Preferred Height: 0
Flexible Height: 1            ← 占据所有剩余空间

# 固定宽度按钮
Preferred Width: 120
Flexible Width: 0
```

### 3.5 ContentSizeFitter

**用途**：根据子内容自动调整父容器大小

```yaml
添加方式: add_component -> ContentSizeFitter
关键属性:
  Horizontal Fit: PreferredSize / MinSize / Unconstrained
  Vertical Fit: PreferredSize / MinSize / Unconstrained
```

**常见配置**：
```
# ScrollView 的 Content 对象（垂直列表）
Horizontal Fit: Unconstrained
Vertical Fit: PreferredSize   ← 高度随内容增长，ScrollView 自动出现滚动条
```

### 3.6 布局组件组合模式

```
┌─────────────────────────────────────────┐
│ EditorArea                              │  ← VerticalLayoutGroup
│   ChildControlHeight: false             │     (让子元素自己控制高度)
│   ChildForceExpandWidth: true           │
│                                         │
│   ┌─────────────────────────────────┐   │
│   │ TabBar                          │   │  ← HorizontalLayoutGroup + LayoutElement
│   │   childAlignment: MiddleCenter  │   │     Preferred Height: 40
│   │   childForceExpandWidth: true   │   │     Flexible Height: 0
│   │                                 │   │
│   │  [List Btn]   [Timeline Btn]   │   │
│   │                                 │   │
│   └─────────────────────────────────┘   │
│                                         │
│   ┌─────────────────────────────────┐   │
│   │ Content View (List / Timeline)  │   │  ← LayoutElement
│   │                                 │   │     Flexible Height: 1
│   │   (ScrollView or other content) │   │     (占据所有剩余空间)
│   └─────────────────────────────────┘   │
│                                         │
└─────────────────────────────────────────┘
```

---

## 4. ScrollView 标准搭建流程

ScrollView 是多层嵌套结构，必须严格按以下层级创建：

```
ScrollView                       ← Image + ScrollRect + Mask
├── Viewport                     ← Image + Mask
│   └── Content                  ← 无特殊组件（在这里放内容）
│       ├── Item1
│       ├── Item2
│       └── ...
├── Scrollbar Horizontal         ← 可选
└── Scrollbar Vertical           ← 可选
```

**搭建步骤**：

```yaml
步骤1: 创建 ScrollView
  工具: create_ui_element -> type="ScrollView"
  父级: 目标容器

步骤2: Content 必须加 VerticalLayoutGroup + ContentSizeFitter
  工具: add_component VerticalLayoutGroup 到 Content
        add_component ContentSizeFitter 到 Content
        set_component_property ContentSizeFitter verticalFit=PreferredSize

步骤3: 设置 ScrollView 填充父级（全屏拉伸模式）
  工具: set_component_property RectTransform
        m_AnchorMin=0,0  m_AnchorMax=1,1

步骤4: 创建列表项时，每一项作为 Content 的子元素
  工具: instantiate_prefab 或 create_ui_element
```

**ScrollView 不滚动的常见原因**：
1. Content 缺少 ContentSizeFitter + VerticalFit=PreferredSize
2. Content 的 RectTransform anchor 不是顶部对齐
3. Viewport 的 Mask 组件丢失
4. ScrollRect 的 Content 引用未正确设置

---

## 5. 对齐与锚点选择决策树

```
元素需要固定在父级的某个边？
├── 顶部 → Anchor Min=(0.5,1) Max=(0.5,1) Pivot=(0.5,1)
├── 底部 → Anchor Min=(0.5,0) Max=(0.5,0) Pivot=(0.5,0)
├── 左侧 → Anchor Min=(0,0.5) Max=(0,0.5) Pivot=(0,0.5)
├── 右侧 → Anchor Min=(1,0.5) Max=(1,0.5) Pivot=(1,0.5)
├── 左上 → Anchor Min=(0,1) Max=(0,1) Pivot=(0,1)
├── 右上 → Anchor Min=(1,1) Max=(1,1) Pivot=(1,1)
├── 左下 → Anchor Min=(0,0) Max=(0,0) Pivot=(0,0)
└── 右下 → Anchor Min=(1,0) Max=(1,0) Pivot=(1,1)

元素需要填充父级？
├── 全屏填充（留边距用负 SizeDelta）→ Min=(0,0) Max=(1,1)
├── 水平拉伸（固定高度）→ Min=(0,y) Max=(1,y)
└── 垂直拉伸（固定宽度）→ Min=(x,0) Max=(x,1)

元素需要居中？
└── → Min=(0.5,0.5) Max=(0.5,0.5) Pivot=(0.5,0.5)
```

---

## 6. 真实场景案例

### 案例 1：编辑器主界面（全屏 4 区域布局）

```
Canvas (1920×1080)
├── TopPanel          ← 顶部工具栏
│   Anchor: (0.5,1)→(0.5,1)  Pivot: (0.5,1)  SizeDelta: (0, 60)
│   HorizontalLayoutGroup
│   ├── BackButton   ← 绝对定位
│   │   Anchor: (0,1)→(0,1)  Pivot: (0.5,0.5)  Position: (40,-30)  Size: (80,40)
│   ├── TitleLabel   ← 绝对定位
│   │   Anchor: (0.5,0.5)→(0.5,0.5)  Pivot: (0.5,0.5)  Size: (200,40)
│   └── SaveButton   ← 绝对定位
│       Anchor: (1,1)→(1,1)  Pivot: (0.5,0.5)  Position: (-40,-30)  Size: (80,40)
│
├── LeftPanel         ← 左侧边栏
│   Anchor: (0,0.5)→(0,0.5)  Pivot: (0,0.5)  SizeDelta: (200, 0)
│   VerticalLayoutGroup
│
├── EditorArea        ← 编辑区（中间填充）
│   Anchor: (0,0)→(1,1)  Pivot: (0.5,0.5)  SizeDelta: (-200,-200)
│   VerticalLayoutGroup
│   ├── TabBar        ← LayoutElement: PreferredHeight=40
│   └── TimelineView  ← LayoutElement: FlexibleHeight=1
│
└── RightPanel        ← 右侧面板
    Anchor: (1,0.5)→(1,0.5)  Pivot: (1,0.5)  SizeDelta: (200, 0)
    VerticalLayoutGroup
```

### 案例 2：可滚动列表（带表头）

```
ListView
├── ListHeader             ← 固定表头
│   Anchor: (0,1)→(1,1)  Pivot: (0.5,1)  SizeDelta: (0, 30)
│   HorizontalLayoutGroup
│   ├── HdrIndex  →  Size: (36, 24)
│   ├── HdrTime   →  Size: (90, 24)
│   ├── HdrLane   →  Size: (55, 24)
│   ├── HdrType   →  Size: (90, 24)
│   └── HdrDuration → Size: (75, 24)
│
└── NoteListScrollView     ← 可滚动内容
    Anchor: (0,0)→(1,1)  (填充剩余空间)
    └── Content
        VerticalLayoutGroup + ContentSizeFitter(Vertical=PreferredSize)
        ├── NoteRow1
        ├── NoteRow2
        └── ...
```

### 案例 3：动态行模板（NoteRowTemplate）

```
NoteRowTemplate              ← 初始设置 SetActive(false)
├── IndexLabel               ← TMP_Text "#0"
│   Size: (36, 30)
├── TimeField                ← TMP_InputField
│   Size: (90, 30)
├── LaneField                ← TMP_InputField
│   Size: (55, 30)
├── TypeDropdown             ← TMP_Dropdown
│   Size: (90, 30)
├── DurationField            ← TMP_InputField
│   Size: (75, 30)
├── SelectBtn                ← Button "选择"
│   Size: (50, 28)
└── DeleteBtn                ← Button "X"
    Size: (28, 28)
```

**关键规则**：模板必须设为 inactive（`set_game_object_active=false`），运行时通过 `Instantiate` 克隆后设为 active。

---

## 7. 大模型操作注意事项

### 7.1 创建 UI 元素的正确顺序

1. **先创建父容器**，再创建子元素（通过 `parentPath` 参数）
2. **创建 ScrollView** 时，会自动生成 Viewport 和 Content
3. **创建脚本**后必须等待 `compile_scripts` 返回 `status=idle`，再 `add_component`
4. **模板 prefab** 创建后必须 `SetActive(false)`

### 7.2 常用组件对应的 create_ui_element type

| UI 元素 | elementType | 备注 |
|---|---|---|
| 面板 / 容器 | `Panel` | 带 Image 组件，可做背景 |
| 文本 | `TMP` | TextMeshPro 文本 |
| 按钮 | `Button` | 带 Image + Button，自动生成 Text 子对象 |
| 文本输入框 | `TMP_InputField` | 带 Placeholder + Text 子对象 |
| 下拉框 | `TMP_Dropdown` | 带 Label + Arrow + Template |
| 滑动条 | `Slider` | |
| 开关 | `Toggle` | 带 Background + Checkmark + Label |
| 滚动视图 | `ScrollView` | 自动生成 Viewport/Content/Scrollbar |
| 布局组 | `HorizontalLayoutGroup` | 当作"Panel"创建后 add_component |
| 布局组 | `VerticalLayoutGroup` | 当作"Panel"创建后 add_component |

### 7.3 设置 RectTransform 属性的正确方法

用 `set_component_property` 工具，组件类型为 `RectTransform`，但**属性名必须用 internalName**：

```
Anchor Min     →  m_AnchorMin    (值格式: "0,0")
Anchor Max     →  m_AnchorMax    (值格式: "1,1")
Anchored Position → m_AnchoredPosition (值格式: "0,0")
Size Delta     →  m_SizeDelta    (值格式: "1920,1080")
Pivot          →  m_Pivot        (值格式: "0.5,0.5")
```

### 7.4 常见陷阱

| 陷阱 | 原因 | 解决方案 |
|---|---|---|
| 元素不显示 | 被父级裁剪或位置超出屏幕 | 检查 SizeDelta 和 Position |
| ScrollView 不滚动 | Content 没有 ContentSizeFitter | 加 ContentSizeFitter + VerticalFit=PreferredSize |
| 布局组件不生效 | ChildControlHeight/Width = false | 设为 true |
| 子元素在 LayoutGroup 中被压缩 | ForceExpand = false 且子元素无 LayoutElement | 加 LayoutElement 设 Preferred size |
| 文字显示为 □ | 字体不支持该字符集 | 改用目标语言支持的字体或换用英文 |
| 点击事件不响应 | EventSystem 缺失或 Raycast Target 未启用 | 确认有 EventSystem |

### 7.5 批量操作用于效率

创建复杂 UI（如一个包含多列的面板）时，将多个独立的工具调用打包到一个 `batch_execute` 中：

```json
{
  "calls": [
    {"toolName": "create_ui_element", "arguments": {...}},
    {"toolName": "create_ui_element", "arguments": {...}},
    {"toolName": "set_component_property", "arguments": {...}},
    ...
  ]
}
```

注意：有依赖关系的调用（如先创建父级再创建子级）仍必须在同一个 batch 中按顺序排列，因为 batch 是顺序执行的。

---

## 8. 快速参考卡片

```
┌─────────────────── 布局速查 ───────────────────┐
│                                                  │
│  顶部固定: A(0.5,1)→(0.5,1) P(0.5,1)           │
│  底部固定: A(0.5,0)→(0.5,0) P(0.5,0)           │
│  左侧固定: A(0,0.5)→(0,0.5) P(0,0.5)           │
│  右侧固定: A(1,0.5)→(1,0.5) P(1,0.5)           │
│  全屏填充: A(0,0)→(1,1)   P(0.5,0.5)           │
│  居中元素: A(0.5,0.5)→(0.5,0.5) P(0.5,0.5)     │
│                                                  │
│  SizeDelta = (0, h) → 宽度自适应，高度固定       │
│  SizeDelta = (w, 0) → 宽度固定，高度自适应       │
│  SizeDelta = (-m, -n) → 四边各留 m,n 边距       │
│                                                  │
│  列表排列: VerticalLayoutGroup + Content         │
│  水平排列: HorizontalLayoutGroup                 │
│  固定大小: LayoutElement (PreferredHeight)       │
│  弹性填充: LayoutElement (FlexibleHeight=1)      │
│  内容自适: ContentSizeFitter (VerticalFit)       │
│                                                  │
└──────────────────────────────────────────────────┘
```

---

*基于 Unity 2022.3 URP + TextMeshPro + GladeKit MCP Bridge*
