# Slider — 规划

## 决策清单

1. **方向**：LeftToRight（横滑条）最常见
2. **子节点**：Background + Fill Area/Fill + Handle Slide Area/Handle（4 个）
3. **Fill Area**：Y 轴居中（anchor 0.5），X 轴撑满左右（anchor 0→1）
4. **Fill**：撑满 Fill Area（anchor 0→1，stretch 上下）
5. **Handle Slide Area**：四向撑满（anchor 0→1），Handle 可在整个区域内滑动
6. **Handle 子节点**：如 Handle 有自定义子节点（Knob 等），Handle 自身 Image alpha=0
7. **颜色区分**：Fill 用 accent 色，Handle/Knob 用亮色，两者必须不同
8. **LayoutGroup 场景**：父节点有 LayoutGroup 时必须加 LayoutElement
9. **值范围**：MinValue/MaxValue 从 config 或代码设置

## 前置检查

```
□ 确定滑动方向
□ 确认父节点是否有 LayoutGroup
□ config.colors.sliderFill 和 sliderTrack 已加载
□ 确认脚本引用路径（_speedSlider / _offsetSlider 等）
```

## 创建顺序

1. 创建 Slider GameObject（`create_ui_element` type="Slider"）
2. 验证或手动创建 4 个子节点
3. **用 `set_ui_properties`** 设置 RectTransform 规则（anchor、size）
4. 用 `set_component_property` 设置 Slider 引用（fillRect, handleRect, targetGraphic）
5. 用 `set_ui_properties` 设置颜色
6. 如有 LayoutGroup → 加 LayoutElement，用 `set_ui_properties` 设 preferredHeight
7. 在脚本中关联 `_slider` 引用（`set_object_reference`）
