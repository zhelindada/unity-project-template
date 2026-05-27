# Button — 检查

## 快速验证

```
□ 子 Text(TMP) 存在
□ Button.Interactable = true
□ Button.TargetGraphic 已设置
□ Image.RaycastTarget = true
□ EventSystem 存在
□ 未被全屏覆盖 Panel 遮挡（检查同级顺序）
□ OnClick 事件已绑定
```

## 常见错误

| 问题 | 原因 | 修复 |
|------|------|------|
| 按钮无法点击 | EventSystem 缺失 / TargetGraphic 未设 | create_event_system / 设 TargetGraphic |
| 点击无反应 | OnClick 未绑定 | 在脚本中添加 AddListener |
| 按钮被遮挡 | 弹窗遮罩在前 | 调整同级顺序，遮罩放在按钮之前 |
| 按钮不可见 | Image.Color.a = 0 | 设置非透明颜色 |
