# Button — 规划

## 决策清单

1. **尺寸**：Width 从 config patterns 读取，Height 36-52px
2. **颜色**：Normal/Hover/Press/Disabled 四色从 config
3. **文字**：字号从 config sizeByRole.button，居中显示
4. **OnClick**：在脚本 Awake/Start 中 `button.onClick.AddListener()`
5. **层级**：确保没有全屏 RaycastTarget Panel 覆盖在上面

## 前置检查

```
□ config.colors 已加载
□ config.fonts.sizeByRole 已读取
□ EventSystem 存在
□ 目标父节点存在且 active
```
