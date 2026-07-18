# Slider — 生成

> **UI 工具优先**：UI 元素修改必须使用 `set_ui_properties` 和 `get_ui_element_info`，**不要**用 `set_component_property` 操作 UI 组件。这两个工具专为 UI 设计，参数更直观，能正确处理 RectTransform + Image + Slider 联动。

## 创建

```
create_ui_element(type="Slider", name="MySlider", parentPath="Parent")
```

**注意**：根据 bridge 版本，可能不会自动创建子节点。创建后立即用 `list_children` 验证。

## 如缺子节点，手动补全

```bash
# 创建透明容器（Fill Area / Handle Slide Area）
create_ui_element(type="Image", name="Fill Area", parentPath="Slider", color="1,1,1,0")
create_ui_element(type="Image", name="Handle Slide Area", parentPath="Slider", color="1,1,1,0")

# 创建可见子节点
create_ui_element(type="Image", name="Background", parentPath="Slider", color="0.2,0.2,0.22,1")
create_ui_element(type="Image", name="Fill", parentPath="Slider/Fill Area", color="0.3,0.6,0.9,1")
create_ui_element(type="Image", name="Handle", parentPath="Slider/Handle Slide Area", color="0.9,0.9,0.9,1")
```

## RectTransform（横滑条）

> 用 `set_ui_properties` 设置 anchor 和尺寸，**不要用** `set_component_property RectTransform`。

```bash
# Background 撑满
set_ui_properties(elementPath="Slider/Background", anchorMinX=0, anchorMinY=0, anchorMaxX=1, anchorMaxY=1, width=0, height=0)

# Fill Area：Y 轴居中，X 轴撑满左右
set_ui_properties(elementPath="Slider/Fill Area", anchorMinX=0, anchorMinY=0.5, anchorMaxX=1, anchorMaxY=0.5, width=-10, height=0)

# Fill：撑满 Fill Area（上下拉伸）
set_ui_properties(elementPath="Slider/Fill Area/Fill", anchorMinX=0, anchorMinY=0, anchorMaxX=1, anchorMaxY=1, width=0, height=0)

# Handle Slide Area：四向撑满（centered + stretch all）
set_ui_properties(elementPath="Slider/Handle Slide Area", anchorMinX=0, anchorMinY=0, anchorMaxX=1, anchorMaxY=1, width=-10, height=0)

# Handle: Y 撑满，X 固定 20px
set_ui_properties(elementPath="Slider/Handle Slide Area/Handle", anchorMinX=0.5, anchorMinY=0, anchorMaxX=0.5, anchorMaxY=1, width=20, height=0)
```

## Handle 有自定义子节点时

```bash
# 创建 Knob 子节点作为视觉元素
create_ui_element(type="Image", name="Knob", parentPath="Slider/Handle Slide Area/Handle", color="1,1,1,1", size="24,24")

# 隐藏 Handle 自身 Image（alpha=0），避免与 Knob 重叠
set_ui_properties(elementPath="Slider/Handle Slide Area/Handle", color="1,1,1,0")
```

> **原因**：Slider 组件每帧重置 Handle 的 sizeDelta 为 (0,0)，无法直接修改 Handle 的尺寸。必须通过子节点实现自定义大小的把手。

## 设置 Slider 引用

> Slider 组件上的引用（fillRect / handleRect）不是 UI 属性，用 `set_component_property`。

```bash
set_component_property Slider fillRect = "Slider/Fill Area/Fill"
set_component_property Slider handleRect = "Slider/Handle Slide Area/Handle"
set_component_property Slider targetGraphic = "Slider/Handle Slide Area/Handle"
set_component_property Slider minValue = "0"
set_component_property Slider maxValue = "1"
set_component_property Slider value = "0.5"
```

## LayoutGroup 场景

```bash
add_component LayoutElement
set_ui_properties(elementPath="Slider", preferredHeight=20)
```
