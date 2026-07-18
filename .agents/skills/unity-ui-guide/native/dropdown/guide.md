# TMP_Dropdown — 下拉框

## 层级结构

```
TMP_Dropdown (GameObject)              ← Image + TMP_Dropdown
├── Label (GameObject)                 ← TMP（当前选中项）
├── Arrow (GameObject)                 ← Image（▼ 箭头）
└── Template (GameObject)              ← Image + ScrollRect（下拉列表模板）
    └── Viewport (GameObject)          ← Image + Mask
        └── Content (GameObject)       ← 空节点
            └── Item (GameObject)      ← Toggle 模板
                ├── Item Background
                ├── Item Checkmark
                └── Item Label
```

## 必须的引用

| 引用 | 指向 |
|------|------|
| `Template` | Template 的 RectTransform |
| `Caption Text` | Label 的 TMP |
| `Item Text` | Item/Item Label 的 TMP |

## 注意事项

- Item 是列表项模板（Toggle），运行时动态生成
- Template 初始应设为 inactive
- Options 在代码或 Inspector 中设置
