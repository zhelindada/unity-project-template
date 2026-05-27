# Layout — 规划

1. **排列方向**：垂直列表 → VerticalLayoutGroup，水平工具栏 → HorizontalLayoutGroup
2. **固定尺寸子元素**：给子元素加 LayoutElement 设 preferredHeight/Width
3. **弹性填充**：FlexibleHeight/FlexibleWidth = 1
4. **ChildControl/ForceExpand**：ChildForceExpandWidth=false（推荐）避免子元素被强制拉伸
