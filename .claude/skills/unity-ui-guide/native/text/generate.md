# TextMeshPro — 生成

## 创建

```
create_ui_element(type="TMP") → 生成 TextMeshProUGUI 组件
```

## 关键属性设置

```bash
# 字体（必须完整路径）
set_component_property TextMeshProUGUI font = "Assets/.../FontName SDF.asset"

# 文字内容
set_component_property TextMeshProUGUI text = "Hello World"

# 字号
set_component_property TextMeshProUGUI fontSize = "24"

# 对齐
set_component_property TextMeshProUGUI alignment = "Center"

# 颜色（fontColor 控制实际渲染颜色，color 也可以）
set_component_property TextMeshProUGUI fontColor = "1,1,1,1"
```

## 属性名速查

| Inspector 名 | internalName | 示例值 |
|-------------|-------------|--------|
| Font Asset | `font` | `"Assets/.../FontName SDF.asset"` |
| Font Size | `fontSize` | `"24"` |
| Font Color | `fontColor` | `"1,1,1,1"` |
| Text | `text` | `"Hello"` |
| Alignment | `alignment` | `"Center"` |
| Overflow | `overflowMode` | `"Overflow"` |

## 注意事项

- `font` 属性需要完整 Asset 路径，不能只用名称
- 设置 `font` 后检查是否出现 `TMP SubMeshUI` 子对象（fallback 标志）
- `RaycastTarget`：纯显示文本建议设 false，减少不必要的射线检测
