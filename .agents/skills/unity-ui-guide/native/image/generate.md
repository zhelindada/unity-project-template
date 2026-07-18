# Image — 规划

## 决策清单

1. **Sprite vs 纯色**：有素材用 Sprite，无素材用纯色（SourceImage=null + Color）
2. **透明容器**：需要空容器时用 `color="1,1,1,0"` + `RaycastTarget=false`
3. **交互性**：点击区域 RaycastTarget=true，纯装饰 false
4. **颜色**：从 config.colors 选取语义色

## 常见用途

| 用途 | 配置 |
|------|------|
| 面板背景 | type=Simple, color=config.colors.panel |
| 按钮背景 | type=Simple, color=config.colors.button |
| 透明容器 | type=Simple, color="1,1,1,0", raycastTarget=false |
| 进度条 | type=Filled, fillMethod=Horizontal, fillAmount=0~1 |
| 弹窗遮罩 | type=Simple, color=config.colors.overlay, raycastTarget=true |
