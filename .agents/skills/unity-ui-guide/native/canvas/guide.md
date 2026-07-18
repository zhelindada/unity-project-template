# Canvas — UI 基础设施

Canvas 是所有 UI 的根节点。没有 Canvas，任何 UI 元素都无法渲染。

## 组件概览

| 组件 | 作用 |
|------|------|
| **Canvas** | UI 渲染的根容器，决定渲染模式 |
| **CanvasScaler** | 分辨率适配，控制 UI 在不同屏幕上的缩放 |
| **GraphicRaycaster** | 输入事件检测，让 UI 元素可点击 |

## Canvas

三种 Render Mode：

| 模式 | 说明 | 适用场景 |
|------|------|----------|
| `Screen Space - Overlay` | UI 直接覆盖在屏幕上，无需摄像机 | ★ 最常用 |
| `Screen Space - Camera` | UI 绑定到指定摄像机 | 需要 UI 受摄像机效果影响时 |
| `World Space` | UI 存在于 3D 世界中 | 3D 交互面板、VR 界面 |

关键参数：
- `Render Mode` = `Screen Space - Overlay`
- `Pixel Perfect` = false
- `Receives Events` = true

## CanvasScaler

推荐 `Scale With Screen Size` 模式：

```
UI Scale Mode:          Scale With Screen Size
Reference Resolution:   1920 × 1080
Screen Match Mode:      Match Width Or Height
Match Width Or Height:  0.5（通用）/ 0（竖屏）/ 1（横屏）
```

**matchWidthOrHeight 规则**：
- `0` = 按宽度缩放，竖屏手游适用
- `1` = 按高度缩放，横屏游戏适用
- `0.5` = 综合缩放，通用桌面/平板

## GraphicRaycaster

```
Ignore Reversed Graphics: true
Blocking Objects:         None
Blocking Mask:            Everything
```

## 注意事项

- Canvas 必须先于所有 UI 元素创建
- 所有 UI GameObject 必须是 Canvas 的子节点
- EventSystem 必须存在才能交互（`create_event_system`）
- RectTransform 是所有 Canvas 子元素的标准 Transform 组件
