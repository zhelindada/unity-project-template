# UI Toolkit (UITK) Generation Convention

本规范定义在 Unity 项目中使用 UI Toolkit 生成 UI 时的约定，核心原则是**动静分离**——明确哪些内容放入 UXML/USS 静态文件，哪些内容由 C# 代码动态生成。

---

## 1. 核心原则：动静分离（Static/Dynamic Division）

所有 UI 生成决策都围绕一个判断：**这个元素在运行时是否会变化？**

### 1.1 放入静态文件（UXML + USS）

以下内容必须写死在 UXML/USS 中，不从代码动态生成：

| 类别 | 判断标准 | 示例 |
|------|---------|------|
| **持久存在的页面框架** | 页面打开后始终存在，不会因为数据变化而消失/新增 | 页头、页脚、侧边栏、Tab 栏、分区容器 |
| **固定布局结构** | 布局容器本身的层级关系，不随数据变化 | ScrollView 的 Viewport/Content 骨架、HorizontalLayoutGroup 容器 |
| **简单绑定到单值** | 仅用于显示 Model 的一个字段值，字段类型是基本类型（string/int/float/bool） | 角色名 Label 绑定 `playerName`、生命值 Slider 绑定 `health`、金币数 Label 绑定 `gold` |
| **静态装饰元素** | 不会因逻辑而切换存在/消失的视觉元素 | 分隔线、背景面板、图标（非动态切换的）、标题文字 |
| **数据不变的表单** | 表单项数量和类型固定，只是值不同 | 设置页面的音量滑动条、画质下拉框、用户名输入框 |

### 1.2 放入 C# 代码动态生成

以下内容必须由代码动态生成，不写在 UXML 中：

| 类别 | 判断标准 | 示例 |
|------|---------|------|
| **列表类内容** | 元素数量由数据决定，运行时增删 | 背包格子列表、任务列表、好友列表、聊天消息列表 |
| **数据驱动的重复元素** | 多个同类元素，结构相同但数量不固定 | 卡牌手牌、技能图标行、商店货架商品 |
| **条件性显隐的区块** | 根据状态整块出现/消失的内容 | 展开/收起的详情面板、空状态占位图 vs 有数据时的列表 |
| **运行时组合的复杂元素** | 元素的子结构根据运行时状态决定 | 带镶嵌宝石孔位的装备槽（孔位数由装备品质决定） |
| **动态选项/下拉项** | Dropdown 或 RadioButtonGroup 的选项列表在运行时填充 | 分辨率选择、服务器列表下拉 |

### 1.3 判断决策树

```
这个 UI 元素/区域...
├── 元素数量固定且不会增减？
│   ├── 是 → 它只是显示/绑定 Model 的一个值？
│   │   ├── 是 → ✅ UXML + 数据绑定
│   │   └── 否 → ✅ UXML + C# 控制显隐/交互
│   └── 否 → ❌ 必须用 C# 动态生成
│
├── 它是列表/重复元素？
│   └── 是 → ❌ 必须用 C# 动态生成（在 UXML 中可定义单个 Item 模板）
│
├── 它根据条件整块出现/消失？
│   ├── 是 → 在 UXML 中放占位容器 + 设置 `display: none` 初始状态
│   └── 否 → ✅ UXML
│
└── 它是运行时从数据源填充的选项列表？
    └── 是 → ❌ UXML 只放空容器，C# 填充选项
```

---

## 2. 文件组织与命名

### 2.1 目录结构

```
Assets/$projname/
├── UI/
│   ├── UXML/
│   │   ├── Panels/          # 全屏页面/面板
│   │   │   ├── MainMenuScreen.uxml
│   │   │   ├── SettingsScreen.uxml
│   │   │   └── HUD.uxml
│   │   ├── Components/      # 可复用 UI 组件/控件
│   │   │   ├── HealthBar.uxml
│   │   │   ├── ItemSlot.uxml
│   │   │   └── DialogBox.uxml
│   │   └── Templates/       # 列表项/动态元素模板
│   │       ├── InventoryItemTemplate.uxml
│   │       └── QuestEntryTemplate.uxml
│   ├── USS/
│   │   ├── common.uss       # 全局公共样式（颜色 token、字体、间距 scale）
│   │   ├── panels/          # 面板专属样式（与 UXML Panels 一一对应）
│   │   │   ├── MainMenuScreen.uss
│   │   │   └── HUD.uss
│   │   └── components/      # 组件专属样式（与 UXML Components 一一对应）
│   │       ├── HealthBar.uss
│   │       └── DialogBox.uss
│   └── Scripts/
│       ├── ViewModels/      # ViewModel（数据 + 绑定逻辑）
│       │   ├── MainMenuViewModel.cs
│       │   └── HUDViewModel.cs
│       ├── Presenters/       # Presenter（生成逻辑 + 交互）
│       │   ├── MainMenuPresenter.cs
│       │   └── HUDPresenter.cs
│       └── Components/       # 可复用 UI 组件的控制器
│           ├── HealthBarController.cs
│           └── ItemSlotController.cs
```

### 2.2 命名规则

| 文件类型 | 命名规则 | 示例 |
|---------|---------|------|
| UXML - 面板 | `{Name}Screen.uxml` 或 `{Name}Panel.uxml` | `SettingsScreen.uxml` |
| UXML - 组件 | `{Name}.uxml` | `HealthBar.uxml` |
| UXML - 模板 | `{Name}Template.uxml` | `InventoryItemTemplate.uxml` |
| USS - 面板 | 与对应 UXML 同名 | `SettingsScreen.uss` |
| USS - 公共 | `common.uss` | — |
| ViewModel | `{Name}ViewModel.cs` | `SettingsViewModel.cs` |
| Presenter | `{Name}Presenter.cs` | `SettingsPresenter.cs` |
| 组件控制器 | `{Name}Controller.cs` | `HealthBarController.cs` |

### 2.3 UXML 根元素命名

每个 UXML 文件的根元素必须有 `name` 属性，格式为 kebab-case：

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements">
    <ui:VisualElement name="main-menu-screen" class="screen">
        <!-- 内容 -->
    </ui:VisualElement>
</ui:UXML>
```

---

## 3. UXML 生成约定

### 3.1 只描述结构，不写死数据

- UXML 中不包含会随数据变化的值（如具体文字、数量、列表项内容）
- 静态显示文字（如标题、按钮文字、标签）可以写 `text="设置"` 或 `label="开始游戏"`
- 数据驱动的文字留空或写占位符 `text="..."`，由绑定填充

```xml
<!-- ✅ 正确：静态标题写死在 UXML -->
<ui:Label text="角色名称" class="field-label" />
<!-- ✅ 正确：动态值用 binding-path -->
<ui:Label text="..." binding-path="characterName" class="field-value" />

<!-- ❌ 错误：列表内容写死在 UXML -->
<ui:ScrollView name="inventory-list">
    <ui:VisualElement class="item">  <!-- 固定数量的 item，不可取 -->
        <ui:Label text="铁剑" />
    </ui:VisualElement>
</ui:ScrollView>
```

### 3.2 使用语义化元素类型

优先使用 UITK 内置复合控件，而非用 `VisualElement` 拼凑：

| 需求 | 使用 | 避免 |
|------|------|------|
| 按钮 | `<ui:Button>` | `<ui:VisualElement>` + 手写点击 |
| 文本输入 | `<ui:TextField>` | `<ui:VisualElement>` + 手写输入 |
| 开关 | `<ui:Toggle>` | `<ui:VisualElement>` + 手写状态 |
| 下拉选择 | `<ui:DropdownField>` | `<ui:VisualElement>` + 手写弹出 |
| 滑动选择 | `<ui:Slider>` / `<ui:SliderInt>` | `<ui:VisualElement>` + 手写拖拽 |

### 3.3 列表容器放空壳

对于需要动态填充的列表，UXML 中只放容器和必要的骨架：

```xml
<!-- ✅ 正确：只放容器结构 -->
<ui:ScrollView name="quest-list" class="quest-scroll">
    <!-- Content 由 Presenter 动态填充 QuestEntryTemplate -->
</ui:ScrollView>

<!-- ✅ 正确：列表项模板在独立 .uxml 文件中定义 -->
<!-- QuestEntryTemplate.uxml -->
<ui:VisualElement name="quest-entry" class="quest-entry">
    <ui:Label name="quest-title" class="quest-title" />
    <ui:Label name="quest-desc" class="quest-desc" />
    <ui:VisualElement name="quest-rewards" class="quest-rewards" />
</ui:VisualElement>
```

### 3.4 条件区域使用初始隐藏

需要运行时条件性显隐的区域，UXML 中使用 `style="display: none;"` 初始隐藏：

```xml
<ui:VisualElement name="empty-state" class="empty-state" style="display: none;">
    <ui:Label text="暂无数据" />
</ui:VisualElement>
<ui:VisualElement name="data-content" class="data-content">
    <!-- 有数据时的内容 -->
</ui:VisualElement>
```

---

## 4. USS 生成约定

### 4.1 使用 CSS Variables 管理 Design Token

在 `common.uss` 中定义全局变量，所有 USS 文件通过 `@import` 引用：

```css
/* common.uss */
:root {
    /* Spacing Scale (8-point grid) */
    --space-xs: 4px;
    --space-sm: 8px;
    --space-md: 16px;
    --space-lg: 24px;
    --space-xl: 32px;
    --space-2xl: 48px;

    /* Color Palette */
    --color-primary: #4A90D9;
    --color-primary-hover: #357ABD;
    --color-bg-dark: #1E1E2E;
    --color-bg-panel: #2A2A3C;
    --color-text-primary: #FFFFFF;
    --color-text-secondary: #A0A0B0;
    --color-border: #3A3A4E;

    /* Typography */
    --font-size-title: 28px;
    --font-size-heading: 22px;
    --font-size-body: 16px;
    --font-size-caption: 12px;

    /* Layout */
    --panel-max-width: 1280px;
    --sidebar-width: 260px;
}
```

### 4.2 选择器约定

- 用 **class** 控制样式（`.button-primary`），不用元素类型选择器
- 用 **name** 做唯一标识和 C# 查询（`#quest-list`），尽量不用于样式
- C# 代码用 `Q<VisualElement>("name")` 查询元素

```css
/* ✅ 正确：class 控制样式 */
.screen { flex-grow: 1; padding: var(--space-lg); }
.main-menu-title { font-size: var(--font-size-title); margin-bottom: var(--space-xl); }

/* ✅ 正确：name 仅做标识 */
#main-menu-screen { }
```

### 4.3 样式层级

```
common.uss            ← 全局变量和基础样式
  ├── panels/*.uss    ← 面板特有样式（导入 common.uss）
  └── components/*.uss ← 组件特有样式（导入 common.uss）
```

每个面板 USS 文件头部导入公共样式：
```css
@import url("../common.uss");
```

### 4.4 Flexbox 布局

UITK 默认使用 Flexbox 模型。指定方向时必须同时设置 `flex-direction`：

```css
.horizontal-container {
    flex-direction: row;
    align-items: center;
}
.vertical-container {
    flex-direction: column;
    align-items: stretch;
}
.fill-remaining {
    flex-grow: 1;
}
```

---

## 5. C# 代码生成约定

### 5.1 何时生成脚本

依照动静分离原则，只在以下情况编写 C# UI 脚本：

1. **动态列表填充** — 运行时实例化 Template 并填充数据
2. **条件显隐控制** — 根据状态切换 UI 区块可见性
3. **交互逻辑** — 按钮点击、Toggle 变化等事件响应
4. **数据绑定** — 将 Model 数据绑定到 UXML 元素的 `binding-path`
5. **动画/过渡** — USS transitions 无法表达的复杂动画逻辑

### 5.2 ViewModel 约定

```csharp
// 必须继承 INotifyPropertyChanged
// 所有需要绑定到 UI 的属性必须触发 PropertyChanged
public class SettingsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string playerName;
    public string PlayerName
    {
        get => playerName;
        set { playerName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayerName))); }
    }

    // 列表数据不绑定到单个 binding-path——交由 Presenter 动态生成
    public ObservableCollection<QuestData> Quests { get; } = new();
}
```

### 5.3 Presenter 约定

Presenter 负责：
- 加载 UXML 并获取根元素
- 设置数据上下文（`rootVisualElement.dataSource = viewModel`）
- 动态生成列表内容
- 连接事件和交互逻辑
- 管理子组件生命周期

```csharp
public class SettingsPresenter
{
    private readonly SettingsViewModel viewModel;
    private VisualElement root;
    private ScrollView questList;

    public SettingsPresenter(SettingsViewModel vm)
    {
        viewModel = vm;
    }

    public void Initialize(VisualElement parent)
    {
        var tree = Resources.Load<VisualTreeAsset>("UI/UXML/Panels/SettingsScreen");
        root = tree.Instantiate();
        root.dataSource = viewModel;
        parent.Add(root);

        // 获取静态结构中的容器引用
        questList = root.Q<ScrollView>("quest-list");

        // 动态填充列表
        PopulateQuestList();

        // 连接事件
        root.Q<Button>("save-btn").clicked += OnSaveClicked;
    }

    private void PopulateQuestList()
    {
        questList.Clear();
        var template = Resources.Load<VisualTreeAsset>("UI/UXML/Templates/QuestEntryTemplate");
        foreach (var quest in viewModel.Quests)
        {
            var entry = template.Instantiate();
            entry.Q<Label>("quest-title").text = quest.Title;         // 动态填充
            entry.Q<Label>("quest-desc").text = quest.Description;    // 动态填充
            questList.Add(entry);
        }
    }
}
```

### 5.4 模板实例化规范

模板 UXML 的根元素是唯一"契约"，C# 通过 `Q<T>("name")` 查找子元素并填充数据：

```
ItemTemplate.uxml 的根结构:
<ui:VisualElement name="inventory-item">
    <ui:VisualElement name="item-icon" />
    <ui:Label name="item-name" />
    <ui:Label name="item-count" />
</ui:VisualElement>

C# 填充代码:
var item = template.Instantiate();
item.Q<VisualElement>("item-icon").style.backgroundImage = data.Icon;
item.Q<Label>("item-name").text = data.Name;
item.Q<Label>("item-count").text = $"x{data.Count}";
```

---

## 6. 与 unity-uitk-builder / unity-uitk-logic 的协作

### 6.1 使用时机

| Skill | 何时使用 |
|-------|---------|
| `unity-uitk-builder` | 设计文档已有页面布局 → 生成 UXML + USS 静态骨架 |
| `unity-uitk-logic` | UXML/USS 已完成 → 生成 ViewModel + Presenter 逻辑脚本 |

### 6.2 标准生成流程

```
设计文档/线框图
    │
    ▼
[unity-uitk-builder]  ← 分析页面结构，按动静分离原则
    │                    静态部分 → UXML + USS
    │                    动态列表 → UXML 只放容器 + 独立 ItemTemplate
    │
    ├── 输出: Panels/*.uxml + Components/*.uxml + Templates/*.uxml
    └── 输出: common.uss + panels/*.uss + components/*.uss
    │
    ▼
[unity-uitk-logic]     ← 为每个 Panel 生成 ViewModel + Presenter
    │                    列表数据的 ObservableCollection → Presenter 动态生成
    │                    单值数据 → ViewModel 属性 + UXML binding-path
    │
    ├── 输出: ViewModels/*.cs + Presenters/*.cs
    └── 输出: Components/*Controller.cs
```

### 6.3 Builder 阶段必须执行的动静判断

在调用 unity-uitk-builder 前（或 builder 内部第一步），必须对设计文档中每个 UI 区域标注分类：

```
[静态 → UXML] 顶部导航栏（4 个按钮，数量固定）
[静态 → UXML + 绑定] 玩家名 Label
[动态 → C#] 任务列表（数量由后端数据决定）
[动态 → C#] "暂无任务"空状态 vs 有任务时的列表
[模板 → UXML] QuestEntryTemplate（单条任务的结构，不含数据）
```

---

## 7. 完整示例：任务面板

### 7.1 设计文档标注

```
┌─ QuestPanel ────────────────────────────────┐
│ [静态→UXML] 标题 "任务"                       │
│                                              │
│ [静态→UXML] 筛选标签栏 (全部/主线/支线)       │
│                                              │
│ [动态→C#] ┌─ QuestList (ScrollView) ──────┐ │
│            │ [模板→UXML] QuestEntry        │ │
│            │   ├ title  [绑定]             │ │
│            │   ├ desc   [绑定]             │ │
│            │   ├ status [绑定]             │ │
│            │   └ rewards [绑定]            │ │
│            │ [模板→UXML] QuestEntry        │ │
│            │ ... (数量动态)                 │ │
│            └────────────────────────────────┘ │
│                                              │
│ [动态→C#] 空状态占位 "暂无任务"              │
│             (列表为空时顯示)                  │
│                                              │
│ [静态→UXML] 关闭按钮                          │
└──────────────────────────────────────────────┘
```

### 7.2 最终产出

| 文件 | 类型 | 说明 |
|------|------|------|
| `QuestPanel.uxml` | 静态 | 模板之外的全部结构 |
| `QuestPanel.uss` | 静态 | 面板样式 |
| `QuestEntryTemplate.uxml` | 模板 | 单条任务结构（无数据） |
| `QuestPanelViewModel.cs` | 动态 | 数据 + 属性绑定 |
| `QuestPanelPresenter.cs` | 动态 | 列表生成 + 交互 + 显隐控制 |

---

## 8. 快速参考

### 8.1 动静速查表

| 场景 | 放到 | 方式 |
|------|------|------|
| 页面标题/标签文字 | UXML | 写死 `text="xxx"` |
| 按钮（文字固定） | UXML | `<ui:Button text="确定">` |
| 输入框（单个固定） | UXML | `<ui:TextField>` + binding-path |
| 角色名/血量/金币 | UXML | `<ui:Label>` + binding-path |
| 装备槽 × N | 模板 UXML | ItemTemplate + C# 列表生成 |
| 背包网格 | C# | 完全动态生成 |
| 下拉框选项 | UXML(容器) + C# | 代码填充 Items |
| 空状态/错误提示 | UXML | 初始 hidden + C# 控制显隐 |
| 弹窗结构 | UXML | 固定结构 |
| 弹窗内容（动态） | C# | 代码填充 |
| 背景/分隔线/装饰 | UXML | 静态不变 |

### 8.2 不可协商规则

1. **列表只能是模板 + C# 动态生成**，禁止在 UXML 中写死多个列表项
2. **UXML 中不含业务数据**（如具体物品名、数量、任务描述文）
3. **所有视觉 token 集中在 common.uss 变量中**，禁止在其他 USS 中裸写具体数值
4. **模板 UXML 必须独立文件**，不得内嵌在面板 UXML 中
5. **name 属性用 kebab-case 做唯一标识**，class 用 CSS 语义命名做样式

---

---

## 9. 文件编码与 Unicode 处理

### 9.1 问题现象

中文字符被写成 `\uXXXX` 转义序列，导致代码和 UI 文件不可读：

```xml
<!-- ❌ 不可读 -->
<ui:Label text="斯德哥尔摩房间" />

<!-- ✅ 应该如此 -->
<ui:Label text="斯德哥尔摩房间" />
```

### 9.2 根因分析

| 文件类型 | 出现 `\uXXXX` 的原因 | 是否应该修复 |
|---------|---------------------|-------------|
| `.cs` C# 脚本 | 写入工具使用 ASCII 编码保存文件，中文被强制转义 | **必须修复** — 重新用 UTF-8 写入 |
| `.uxml` UI 布局 | 同上，或通过非 UTF-8 通道复制/生成 | **必须修复** — 重新用 UTF-8 写入 |
| `.uss` 样式表 | 同上 | **必须修复** — 重新用 UTF-8 写入 |
| `.unity` 场景文件 | Unity YAML 序列化器的**内置行为**，在 Text Serialization 模式下，非 ASCII 字符强制编码为 `\uXXXX` | **禁止手动修复** — Unity 在加载时自动解码，编辑器内显示正常 |
| `.prefab` 预制体 | 同 `.unity` | **禁止手动修复** |
| `.asset` 资产文件 | 同 `.unity` | **禁止手动修复** |

关键区分：
- **用户编写/维护的文件**（.cs / .uxml / .uss）→ 必须用 UTF-8 + 正常字符
- **Unity 引擎序列化的文件**（.unity / .prefab / .asset）→ Unity 自己的编码规则，不要干预

### 9.3 写入文件的编码规范

**规则 9.3.1**: AI 工具（Claude Code 的 Write/Edit 工具）写入 `.cs`、`.uxml`、`.uss` 文件时，必须使用 UTF-8 编码，直接写入正常中文字符，不得使用 `\uXXXX` 转义。

**规则 9.3.2**: 绝对禁止手动编辑 `.unity`、`.prefab`、`.asset` 文件。这些文件的 `\uXXXX` 序列是 Unity 引擎的标准行为，在编辑器中自动正常显示。手动"修复"会破坏 Unity 对文件的解析。

**规则 9.3.3**: 在 C# 字符串字面量中使用中文时，直接写中文字符，不使用转义：
```csharp
// ✅ 正确
ShowPrompt("你走出了屋子");

// ❌ 错误
ShowPrompt("你走出了屋子");
```

### 9.4 检查与修复流程

每次生成或修改代码/UI 文件后，执行检查：

```bash
# 检查代码和 UI 文件中是否存在 \uXXXX 转义
grep -rn '\\u[0-9a-fA-F]\{4\}' Assets/ \
    --include="*.cs" --include="*.uxml" --include="*.uss"

# 如果有输出 → 该文件需要重新用 UTF-8 写入
# 无输出 → 通过检查
```

修复方法：用 UTF-8 编码重新写入整个文件内容（保持逻辑不变，仅将 `\uXXXX` 替换为实际字符）。

### 9.5 预防措施总结

| # | 措施 | 作用 |
|---|------|------|
| 1 | 始终用 UTF-8 编码写入 `.cs`/`.uxml`/`.uss` | 根除 `\uXXXX` 产生 |
| 2 | 不手动编辑 Unity 序列化文件 | 避免破坏 `.unity`/`.prefab`/`.asset` |
| 3 | 生成代码后用 grep 检查 | 及时发现编码问题 |
| 4 | C# 中直接用中文字符串字面量 | 源码可读、可维护 |

---

*适用于 Unity 6+ UI Toolkit，与 unity-uitk-builder / unity-uitk-logic 技能配合使用*
