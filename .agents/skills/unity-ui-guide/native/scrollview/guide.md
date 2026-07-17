# ScrollView — 滚动视图

## 层级结构

```
ScrollView (GameObject)                ← Image + ScrollRect
├── Viewport (GameObject)              ← Image + Mask / RectMask2D
│   └── Content (GameObject)           ← 空节点，子元素放这里
├── Scrollbar Horizontal (optional)
└── Scrollbar Vertical (optional)
```

## Content 关键配置

Content 上面必须额外加：
- **VerticalLayoutGroup**（子元素垂直排列）
- **ContentSizeFitter**（VerticalFit = PreferredSize）

## Content RectTransform（★ 最关键）

```
Anchor Min:  (0, 1)           ← 顶部对齐 + 水平拉伸
Anchor Max:  (1, 1)           ← 绝对不能用 (0,0)→(1,1)！
Pivot:       (0, 1)           ← 轴心在顶部
SizeDelta:   (0, 0)
```

**为什么不能用 (0,0)→(1,1)**？Content 会被强制填满 Viewport，即使 ContentSizeFitter 要求撑大也不会超出 → 无法滚动。

## ScrollRect 参数

```
Content:      Content RectTransform    (★ 必须设置)
Viewport:     Viewport RectTransform   (★ 必须设置)
Horizontal:   false
Vertical:     true
Movement Type: Clamped / Elastic
Inertia:      true
Scroll Sensitivity: 30
```

## 必须的引用

| 引用 | 指向 |
|------|------|
| `Content` | Content 的 RectTransform |
| `Viewport` | Viewport 的 RectTransform |
| `Vertical Scrollbar` | Scrollbar Vertical 的 RectTransform（可选） |

## 检查清单

```
□ Viewport 有 Mask 或 RectMask2D
□ Content 有 VerticalLayoutGroup
□ Content 有 ContentSizeFitter(VerticalFit=PreferredSize)
□ Content anchor 是 (0,1)→(1,1) 而非 (0,0)→(1,1)
□ Content 子元素总高度 > Viewport 高度（内容超出才能滚动）
□ ScrollRect.Vertical = true
```
