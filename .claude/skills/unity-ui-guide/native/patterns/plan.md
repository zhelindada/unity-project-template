# Patterns — 规划

1. 识别 UI 用途（弹窗？列表？表单？侧栏？）
2. 匹配到 12 种模式之一
3. 从 config `patterns.<模式>` 读取尺寸/Anchor/布局
4. 确定组合哪些组件（如 modal = Panel + VerticalLayoutGroup + Button）

## 模式选择

| 你要做 | 用 |
|--------|-----|
| 设置面板、确认框 | modal |
| 导航菜单、属性栏 | sidebar |
| 标题栏 | topBar |
| 歌曲列表、关卡列表 | scrollableList |
| 设置项 | form |
| 游戏内分数/连击 | hud |
| 关卡选择网格 | cardGrid |
