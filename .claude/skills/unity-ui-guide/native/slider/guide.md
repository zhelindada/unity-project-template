# Slider — 滑动条

Slider = Image（轨道）+ Slider 组件 + 4 个子节点

> **UI 工具优先**：修改 UI 元素的 RectTransform、Image 颜色/尺寸、可见性等属性时，必须使用 `set_ui_properties` 和 `get_ui_element_info`，**禁止**用 `set_component_property` 操作 UI 组件。Slider/Button 等组件自身的逻辑引用（fillRect, handleRect, onClick 等）可用 `set_component_property`。

## 层级结构

```
Slider (GameObject)                    ← Image + Slider 组件
├── Background (GameObject)            ← Image（轨道）
├── Fill Area (GameObject)             ← 空节点，裁剪容器
│   └── Fill (GameObject)              ← Image（填充条）★
├── Handle Slide Area (GameObject)     ← 空节点，手柄容器
│   └── Handle (GameObject)            ← Image（可拖拽手柄）★
```

## RectTransform 精确规则（LeftToRight 横滑条）

| 节点 | Anchor Min | Anchor Max | SizeDelta | 说明 |
|------|-----------|-----------|-----------|------|
| Background | (0, 0) | (1, 1) | (0, 0) | 撑满滑条 |
| Fill Area | (0, **0.5**) | (1, **0.5**) | (-10, 0) | ★ **Y 轴居中**，X 轴撑满左右 |
| Fill | (0, 0) | (1, 1) | (0, 0) | ★ 撑满 Fill Area：上下拉伸 |
| Handle Slide Area | (0, 0) | (1, 1) | (-10, 0) | ★ **四向撑满**（centered + stretch all） |
| Handle | (0.5, 0) | (0.5, 1) | (20, 0) | X=20px，Y 撑满 Handle Slide Area |

## TopToBottom（竖滑条）对应设置

| 节点 | Anchor Min | Anchor Max | SizeDelta |
|------|-----------|-----------|-----------|
| Fill Area | (**0.5**, 0) | (**0.5**, 1) | (0, -10) | ★ X 轴居中，Y 轴撑满上下 |
| Fill | (0, 0) | (1, 1) | (0, 0) | ★ 撑满 Fill Area：左右拉伸 |
| Handle Slide Area | (0, 0) | (1, 1) | (0, -10) | ★ 四向撑满 |
| Handle | (0, 0.5) | (1, 0.5) | (0, 20) | Y=20px，X 撑满 |

## 必须的引用

| 引用 | 指向 | 说明 |
|------|------|------|
| `Fill Rect` | Fill 的 RectTransform | 控制填充条 |
| `Handle Rect` | Handle 的 RectTransform | 控制手柄位置 |
| `Target Graphic` | Handle 的 Image | 颜色过渡目标 |

## 颜色规则

- Slider 自身 Image → 深色 `0.2,0.2,0.22,1`（无 Sprite 时）
- Background → `colors.sliderTrack`（轨道色）
- Fill → `colors.sliderFill` `0.3,0.6,0.9,1`（accent 蓝）
- Handle → `0.9,0.9,0.9,1`（亮灰，**必须与 Fill 不同**）

## 关键陷阱

- **父节点有 LayoutGroup** → 必须给 Slider 加 `LayoutElement` 设 `preferredHeight=20`，否则高度为 0
- **直接改 sizeDelta 无效** → LayoutGroup 每帧覆盖，只能用 LayoutElement
- **销毁重建 Slider 会断开脚本引用** → 必须用 `set_object_reference` 重新关联
- **Handle 颜色必须不同于 Fill** → 否则无法视觉区分
- **Handle Slide Area 必须比 Fill Area 短** → 否则手柄视觉上超出填充条
- **Handle 由 Slider 组件驱动** → sizeDelta 会被 Slider 覆盖，勿手动设置
- **Handle 有子节点时** → Handle 自身 Image 必须设 alpha=0 或移除 Image 组件，用子节点作为视觉元素。Slider 会重置 Handle 的 sizeDelta 为 (0,0)，无法通过 Handle 直接设置尺寸
