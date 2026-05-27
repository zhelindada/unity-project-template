# ScrollView — 规划

1. **滚动方向**：垂直列表最常见
2. **Content 组件**：VerticalLayoutGroup + ContentSizeFitter
3. **Content Anchor**：顶部对齐 (0,1)→(1,1)，不能用中心拉伸
4. **Scrollbar**：可选，建议加 Vertical Scrollbar
5. **Viewport 裁剪**：需要 Mask 或 RectMask2D
