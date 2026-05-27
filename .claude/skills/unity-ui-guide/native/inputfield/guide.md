# TMP_InputField — 文本输入框

TMP_InputField = Image（背景）+ TMP_InputField 组件 + TextArea/Placeholder/Text

## 层级结构

```
TMP_InputField (GameObject)            ← Image + TMP_InputField
├── Text Area (GameObject)             ← RectMask2D（裁剪）
│   ├── Placeholder (GameObject)       ← TextMeshProUGUI（提示文字）
│   └── Text (GameObject)              ← TextMeshProUGUI（实际文字）
```

## 必须的引用

| 引用 | 指向 | 说明 |
|------|------|------|
| `Text Component` | Text Area/Text 的 TMP | 文字渲染目标 |
| `Text Viewport` | Text Area 的 RectTransform | 裁剪视口 |
| `Placeholder` | Text Area/Placeholder 的 RT | 占位提示 |

## 组件参数

```
Content Type:     Standard / Integer / Decimal / Password 等
Line Type:        Single Line / Multi Line
Character Limit:  0（无限制）
Placeholder Text: "请输入..."
```

## 注意事项

- TextArea 必须有 RectMask2D 或 Mask 来裁剪溢出文字
- Placeholder 和 Text 是两个独立的 TMP 组件
- ContentType 根据输入数据类型选择
