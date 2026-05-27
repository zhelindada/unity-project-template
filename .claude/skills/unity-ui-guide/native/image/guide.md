# Image — 图片显示

Image 是 UI 中最常用的视觉组件，用于背景、图标、进度条等。

## 关键参数

```
Source Image:   null / 精灵引用        (null = 纯色矩形)
Color:          (1,1,1,1)              (纯色背景时直接设此值)
Material:       null
Raycast Target: true                   (装饰性图片设 false)
```

## Image Type

| Type | 说明 | 适用场景 |
|------|------|----------|
| Simple | 标准拉伸显示 | 最常用 |
| Sliced | 九宫格，边框不变形 | 带边框的 Panel |
| Tiled | 平铺填充 | 重复纹理 |
| Filled | 填充式，可做进度条 | 血条、加载条 |

## 纯色矩形

最常见用法：`SourceImage=null` + 设置 `Color` = 纯色矩形背景。

```
create_ui_element(type="Image") → set_component_property Image color = "0.1,0.1,0.12,0.95"
```

## 注意事项

- `Color.a > 0` 是可见性的必要条件
- 装饰性非交互图片务必设 `RaycastTarget = false`，减少不必要的射线检测
- 作为按钮背景时 `RaycastTarget = true`
- 透明面板（alpha=0）只作容器用，也建议设 `RaycastTarget = false`
