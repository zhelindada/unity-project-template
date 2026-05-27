# Toggle — 开关

Toggle = Toggle 组件 + Background/Checkmark + Label

## 层级结构

```
Toggle (GameObject)                    ← Toggle 组件 + Image
├── Background (GameObject)            ← Image（复选框背景）
│   └── Checkmark (GameObject)         ← Image（勾选标记 ✓）★
└── Label (GameObject)                 ← TextMeshProUGUI（选项文字）
```

## 必须的引用

| 引用 | 指向 | 说明 |
|------|------|------|
| `Toggle.Graphic` | Checkmark 的 Image | 控制勾选标记的显示/隐藏 |
| `Toggle.Group` | ToggleGroup（可选） | 互斥组 |

## Toggle 组件参数

```
Interactable:     true
Transition:       Color Tint
Is On:            false                 (初始状态)
Graphic:          Checkmark Image       (★ 必须设置)
Group:            null / ToggleGroup

--- ColorTint ---
Normal Color:     (1,1,1,1)
Highlighted:      (0.9,0.9,0.9,1)
Pressed:          (0.7,0.7,0.7,1)
Selected:         (0.85,0.9,1,1)
```

## ToggleGroup

多个 Toggle 互斥时，创建一个 ToggleGroup 组件（放在父节点），每个 Toggle 的 `Group` 引用指向它。
