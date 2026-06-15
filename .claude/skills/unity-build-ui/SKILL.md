---
name: unity-build-ui
description: 在 Unity 中构建 UI 界面和 UI组件 —— 创建 Canvas、新建脚本、布局元素、设置属性和引用、保存为 Prefab。Use when 需要在 Unity 场景中搭建 UI、创建 HUD/菜单/弹窗、或将 UI 保存为预制体供代码加载。
---

在 Unity 中构建 UI 界面，6 步完整流程：准备 → 搭建结构 → 配置属性 → 设置引用 → 最终调整 → 导出 Prefab。

**Tier:** POWERFUL
**Category:** Unity / UI Systems
**Tags:** Unity, UGUI, Canvas, UI construction, prefab, layout, MCP

## 工作流程

按顺序执行以下阶段，不要跳过。

### 阶段 0：准备 — 确保环境就绪

1. 调用 `get_scene_hierarchy` 检查当前场景
2. 确保 Canvas 存在。若没有则调用 `create_canvas` 创建（默认 ScreenSpaceOverlay）
3. 确保 CanvasScaler 的 Reference Resolution 是 3840 * 2160
4. 确保 EventSystem 存在。若没有则调用 `create_event_system`。检查项目用的是new input system 还是 old input package, 生成对应组件
5. 若需要 TextMeshPro 资源，先调用 `import_tmp_essential_resources`

### 阶段 1：设计结构 — 理解需求

1. 向用户确认 UI 的层级结构（根节点 → 子面板 → 具体元素）
2. 确认每个元素需要的组件和交互行为
3. 列出需要创建的完整元素清单后再开始
4. 确保整体风格的一致性
5. 确保文本的可读性（文本和所在背景Image节点的颜色要有一定的对比度）

### 阶段 2：搭建结构 — 创建元素

按从外到内的顺序创建：

1. 先创建容器面板（Panel）作为根节点
2. 在面板内创建子元素（Text、Button、Image、Slider、Toggle、InputField、ScrollView 等）
3. 使用 `set_game_object_parent` 建立父子关系
4. 用 `rename_game_object` 给关键节点命名（如 "LoginButton"、"HealthBar"）
5. 用 `set_transform` / `set_local_transform` 调整位置

创建 UI 元素用 `create_ui_element`：

| elementType | 用途 |
|-------------|------|
| `Panel` | 容器/背景面板 |
| `Text` / `TMP` | 文本标签 |
| `Button` | 按钮（含子 Text） |
| `Image` | 图片 |
| `Slider` | 滑动条 |
| `Toggle` | 开关/复选框 |
| `Dropdown` / `TMP_Dropdown` | 下拉菜单 |
| `InputField` / `TMP_InputField` | 输入框 |
| `ScrollView` | 滚动视图 |
| `HorizontalLayoutGroup` / `VerticalLayoutGroup` / `GridLayoutGroup` | 自动布局 |

常用参数：
- `parentPath` — 父节点路径
- `text` — 文本内容
- `color` — 颜色 `"r,g,b,a"`（0-1 范围）
- `fontSize` — 字号
- `size` — 尺寸 `"width,height"`
- `anchoredPosition` — 锚点位置 `"x,y"`
- `alignment` — 对齐方式

### 阶段 3：配置属性 — 调整外观和行为

对每个已创建的元素，按需配置属性：

**内置组件属性**用 `set_component_property`：
- `Image` — `color`、`sprite`、`fillAmount`
- `Button` — `interactable`、`colors`
- `Text` / `TMP_Text` — `text`、`fontSize`、`color`、`alignment`
- `Slider` — `minValue`、`maxValue`、`value`
- `Toggle` — `isOn`
- `InputField` — `text`、`placeholder`
- `RectTransform` — `sizeDelta`、`anchoredPosition`、`anchorMin`、`anchorMax`、`pivot`

**自定义脚本属性**用 `set_script_component_property`：
- 先通过 `get_gameobject_components` 确认脚本存在
- 再设置具体字段值

**材质和颜色**：
- 需要自定义材质时用 `create_material`，然后用 `assign_material_to_renderer` 应用
- URP 项目 shader 用 `"Universal Render Pipeline/Lit"`

### 阶段 4：创建组件脚本（可选）

当被要求创建面板类型的UI界面时，需要创造一个UI脚本，类名以xxxPanel结尾，继承框架中合适继承的父类，需要完成以下几个基本目标

1. 在Awake绑定引用，添加Listener，（如果使用R3）反应式UI将对应Reactive元素添加Subscription并绑到生命周期, OnDisable时销毁Listener
2. 在Start中初始化文字，字段，显隐和其他显示属性
3. 如果有动画，在OnEnable结尾中播放动画
4. 业务逻辑

当被要求创建内容组件类型的UI组件时，需要创造一个UI脚本，继承框架中合适继承的父类, 实现合适的接口, 且需要完成以下几个基本目标

1. 确定当前组件处在哪个Panel中，Start时获取引用。
2. 业务逻辑

### 阶段 5：设置引用 — 连接组件

使用 `set_object_reference` 连接组件间的引用：

```
targetGameObject — 持有引用的目标对象
componentType   — 目标组件类型
fieldName       — 引用字段名
sourceGameObject — 被引用的源对象
sourceType      — 引用类型（GameObject / Transform / 具体组件类型）
```

常见引用场景：
- Button onClick 导航目标
- Slider 关联的 Fill/Background
- ScrollView 的 Content 引用
- 自定义脚本的 GameObject/Component 引用

**获取可设置的引用字段**：
- 先调用 `get_component_inspector_properties` 并设置 `onlyReferences: true` 或 `onlyUnassigned: true`
- 确认字段名后再设置

### 阶段 6：最终调整 — 检查和微调

1. 用 `get_scene_hierarchy` 检查最终层级结构
2. 用 `get_gameobject_info` 抽查关键元素的位置和组件
3. 用 `group_objects` 将相关元素分组（如需要）
4. 确认所有交互元素功能完整

### 阶段 6：导出 Prefab — 保存为预制体

1. 确保 UI 根节点命名规范（如 "UILoginPanel"、"UIHUD"）
2. 调用 `create_prefab` 保存：
   - `prefabPath` — 如 `"Assets/Prefabs/UI/UILoginPanel.prefab"`
   - `gameObjectPath` — 场景中根节点的路径
3. 若目标文件夹不存在，先调用 `create_folder` 创建（如 `"Prefabs/UI"`）

保存后报告 Prefab 路径和包含的子元素数量。

## 错误处理

- 创建脚本组件前，必须先 `create_script` 然后 `compile_scripts`，等编译完成再 `add_component`
- Canvas 创建失败 → 检查是否已有 Canvas，如有则复用
- 设置引用前必须先确认目标组件存在（用 `get_gameobject_components`）
- Prefab 路径中已有同名文件 → 先提示用户确认是否覆盖

## 快速参考：常用 UI 模板

### 按钮
```
create_ui_element(elementType="Button", name="MyButton", parentPath="...", color="1,1,1,1")
→ 子 Text 自动创建，用 find_game_objects + set_component_property 改文字
```

### 滑动条（带背景和填充）
```
create_ui_element(elementType="Slider", name="HealthBar", parentPath="...")
→ 自动创建 Background、Fill Area、Handle Slide Area 子结构
→ 用 get_gameobject_info 找到 Fill 节点，用 set_component_property 改颜色
```

### 文本标签
```
create_ui_element(elementType="TMP", name="Title", parentPath="...",
  text="标题文字", fontSize=28, color="1,1,1,1", alignment="Center")
```

### 滚动列表
```
create_ui_element(elementType="ScrollView", name="ItemList", parentPath="...",
  size="400,600")
→ Content 节点下添加子元素作为列表项
```
