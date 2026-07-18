# Layout — 布局组件

LayoutGroup、ContentSizeFitter、LayoutElement 负责 UI 元素的自动排列和尺寸管理。

## 组件概览

| 组件 | 作用 |
|------|------|
| **VerticalLayoutGroup** | 垂直排列子元素 |
| **HorizontalLayoutGroup** | 水平排列子元素 |
| **GridLayoutGroup** | 网格排列，固定 cellSize |
| **ContentSizeFitter** | 根据子元素内容自动调整自身尺寸 |
| **LayoutElement** | 覆盖 LayoutGroup 的尺寸控制 |

## LayoutGroup 参数

```
Padding Left/Right/Top/Bottom: 0
Spacing:                        4
Child Alignment:                Upper Left / Middle Center 等
Child Control Width:            true
Child Control Height:           true
Child Force Expand Width:       false
Child Force Expand Height:      false
Reverse Arrangement:            false
```

## 关键陷阱

**在 LayoutGroup 下，直接修改子元素的 `sizeDelta` 是无效的。** LayoutGroup 每帧通过 `SetLayoutInputForAxis` 重新计算子元素的尺寸。唯一绕过方式是给子元素加 **LayoutElement**。

```
add_component LayoutElement
set_component_property LayoutElement preferredHeight = "20"
```

## LayoutElement

```
Min Width / Height:       0
Preferred Width / Height: 0          (★ 期望尺寸)
Flexible Width / Height:  0          (★ 弹性系数，填满剩余空间)
Layout Priority:          0
```

常见用法：
- 固定高度：`preferredHeight=40, flexibleHeight=0`
- 弹性填充：`flexibleHeight=1`
- Slider 在 LayoutGroup 下：`preferredHeight=20`

## ContentSizeFitter

```
Horizontal Fit:   Unconstrained / MinSize / PreferredSize
Vertical Fit:     Unconstrained / MinSize / PreferredSize
```

ScrollView 的 Content 必设：`VerticalFit = PreferredSize`
