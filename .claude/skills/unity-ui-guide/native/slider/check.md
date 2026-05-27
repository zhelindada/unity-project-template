# Slider — 检查

> 验证 UI 状态使用 `get_ui_element_info`，**不要用** `get_component_inspector_properties`。

## 快速验证

```
□ 4 个子节点完整：Background + Fill Area(Fill) + Handle Slide Area(Handle)
□ Fill Rect 引用 → Fill 的 RectTransform
□ Handle Rect 引用 → Handle 的 RectTransform
□ Target Graphic → Handle 的 Image
□ Interactable = true
□ Fill Area：Y 轴居中（anchorMin.y=0.5），X 轴撑满（anchorMin.x=0, anchorMax.x=1）
□ Fill：撑满 Fill Area（anchorMin=0,0 anchorMax=1,1）
□ Handle Slide Area：四向撑满（anchorMin=0,0 anchorMax=1,1）
□ Fill 和 Handle 颜色不同
□ Handle 如有子节点 → Handle 自身 Image alpha=0
```

## LayoutGroup 场景

```
□ Slider 有 LayoutElement 组件
□ LayoutElement.preferredHeight > 0
```

## 脚本引用

```
□ 父脚本上的 [SerializeField] Slider 引用非 null
□ 重建 Slider 后用 set_object_reference 重新关联
```

## 常见错误

| 问题 | 原因 | 修复 |
|------|------|------|
| Slider 不可见 | 无子节点或高度为 0 | 创建子节点 / 加 LayoutElement |
| 无法交互 | 引用断开或 Interactable=false | 重新关联引用 / 设为 true |
| Handle 大小异常 | Slider 驱动 Handle Rect，sizeDelta 被覆盖 | 正常行为，检查 Handle Y anchor(0→1) |
| 数值不变 | onValueChanged 未绑定 | 脚本中 InitSettingsUI() 检查引用 |
| 重建后 slider 不工作 | 脚本引用断了 | set_object_reference 重新关联 |
| Handle 有子节点但看不到 | Handle 自身 Image 遮挡/与子节点重叠 | Handle Image alpha=0，用子节点作为视觉元素 |
