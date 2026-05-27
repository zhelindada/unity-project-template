# Image — 检查

## 快速验证

```
□ Color.a > 0（可见）
□ SourceImage 或纯色 Color 至少一项有效
□ RaycastTarget 与交互意图匹配
□ 透明容器 RaycastTarget=false
```

## 常见错误

| 问题 | 原因 | 修复 |
|------|------|------|
| 面板不显示 | Color.a = 0 或父级 inactive | 检查 Color alpha 和父级状态 |
| 无法点击 | RaycastTarget=false | 设为 true |
| 透明面板挡住后面按钮 | alpha=0 但 RaycastTarget=true | 设为 false |
