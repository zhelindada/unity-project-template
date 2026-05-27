# Patterns — UI 布局模式

布局模式不是单个组件，而是组件组合的设计模式。从 config 的 `patterns` 中读取尺寸参数。

## 12 种模式速查

| 模式 | 场景 | 关键尺寸 |
|------|------|----------|
| **modal** | 弹窗/对话框 | 宽 30-70%，高 30-80%，遮罩 alpha≥0.3 |
| **sidebar** | 侧边栏 | 宽 12-30%（max 400px），高撑满 |
| **topBar** | 顶栏 | 高 36-72px，宽撑满 |
| **bottomBar** | 底栏 | 高 32-64px，宽撑满 |
| **tabBar** | 标签栏 | 高 32-52px，宽撑满 |
| **toolbar** | 工具栏 | 高 32-52px，按钮 28-44px |
| **contentArea** | 主内容区 | 全屏拉伸，负 SizeDelta 留空间 |
| **splitPane** | 左右双栏 | 左 15-40%，右 60-85% |
| **scrollableList** | 可滚动列表 | 行高 32-64px，Content 顶部锚点 |
| **form** | 表单 | label 宽 60-180px，行高 32-52px |
| **hud** | 游戏 HUD | 四角固定，offset 8-32px |
| **cardGrid** | 卡片网格 | 卡片 120-280×160-360px，gap 8-24px |

## modal — 弹窗

```
遮罩 Panel:  anchor(0,0)→(1,1), color=overlay(0,0,0,0.5), RaycastTarget=true
内容 Panel:  anchor(0.5,0.5), pivot(0.5,0.5), sizeDelta from config
布局:        VerticalLayoutGroup（标题 + 内容 + 按钮行）
按钮行:      HorizontalLayoutGroup（取消在左，确认在右）
```

## sidebar — 侧边栏

```
anchor:      left:(0,0)→(0,1) 或 right:(1,0)→(1,1)
sizeDelta:   宽从 config（180-400px），高 0（撑满）
布局:        VerticalLayoutGroup, ChildForceExpandWidth=true
```

## scrollableList — 可滚动列表

```
ScrollView → 填充父级 anchor(0,0)→(1,1)
Content:     anchor(0,1)→(1,1), pivot(0,1)
             必须: VerticalLayoutGroup + ContentSizeFitter(VerticalFit=PreferredSize)
行模板:      高 44px, HorizontalLayoutGroup 排列列内容
```

## contentArea — 主内容区

```
anchor:      (0,0)→(1,1)
sizeDelta:   负值为顶栏/底栏/侧边栏留空间
             e.g. 左侧栏 250px → sizeDelta.x = -250
```
