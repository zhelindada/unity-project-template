# Canvas — 检查

## 快速验证

```
□ Canvas activeSelf = true
□ RenderMode = ScreenSpaceOverlay
□ CanvasScaler.uiScaleMode = ScaleWithScreenSize
□ referenceResolution = 1920×1080
□ matchWidthOrHeight 与游戏方向匹配（横屏=1，竖屏=0）
□ GraphicRaycaster enabled
□ EventSystem 存在于场景中
□ 所有 UI GameObject 是 Canvas 的子节点
```

## 常见错误

| 问题 | 原因 | 修复 |
|------|------|------|
| UI 不显示 | Canvas 未激活或渲染模式错误 | 检查 `get_gameobject_info Canvas` |
| UI 不响应点击 | EventSystem 缺失或 GraphicRaycaster 未启用 | `create_event_system` |
| 分辨率变化时 UI 错位 | matchWidthOrHeight 与游戏方向不匹配 | 横屏设为 1，竖屏设为 0 |
| UI 在不同设备上大小不一致 | referenceResolution 未正确设置 | 设置为设计分辨率 |
