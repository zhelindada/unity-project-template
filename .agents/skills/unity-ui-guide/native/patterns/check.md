# Patterns — 检查

## modal
```
□ 遮罩 alpha ≥ 0.3（constraints）
□ 遮罩 RaycastTarget = true（阻止穿透点击）
□ 弹窗尺寸 ≤ 父级 80%（constraints）
□ 按钮顺序：次要（取消）在左，主要（确认）在右
```

## sidebar
```
□ 宽度 ≤ 父级 30%（constraints）
□ 宽度 ≥ 180px，≤ 400px
□ 内容 VerticalLayoutGroup 存在
```

## scrollableList
```
□ Content 有 VerticalLayoutGroup
□ Content 有 ContentSizeFitter(VerticalFit=PreferredSize)
□ Content anchor = (0,1)→(1,1)
□ Viewport 有 Mask
```

## constraints 通用检查
```
□ 可点击元素 ≥ 24px
□ 字号 ≥ 12px
□ 正文 alpha ≥ 0.8
□ 元素 alpha ≥ 0.3
```
