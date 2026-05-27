# Button — 按钮交互

Button = Image（背景）+ Button 组件 + 子 Text(TMP)（文字）

## 层级结构

```
Button (GameObject)                    ← Image + Button 组件
└── Text (TMP) (GameObject)           ← TextMeshProUGUI 组件
```

## Button 组件参数

```
Interactable:     true
Transition:       Color Tint            (★ 推荐)
Target Graphic:   自身的 Image           (★ 必须设置！)
Navigation:       None                  (游戏 UI 不需要键盘导航)

--- ColorTint 颜色 ---
Normal Color:     (1,1,1,1)            (config.colors.button)
Highlighted:      (0.9,0.9,0.9,1)      (config.colors.buttonHover)
Pressed:          (0.7,0.7,0.7,1)      (config.colors.buttonPress)
Disabled:         (0.5,0.5,0.5,0.5)    (config.colors.buttonDisabled)
Fade Duration:    0.1
```

## 必须的引用

| 引用 | 指向 | 说明 |
|------|------|------|
| `Button.Target Graphic` | Button 自身的 Image 或子 Text | 颜色过渡目标 |

## 经典陷阱

- 弹窗遮罩 Panel（全屏、RaycastTarget=true）覆盖在按钮上方 → 按钮无法点击
- 按钮 Image 无 Sprite + Color.a=0 → 不可见
- Target Graphic 设为子 Text → 只有文字变色，背景不变
