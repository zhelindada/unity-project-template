# UI Toolkit 动画方案

## 可用方案（三层递进）

### 1. USS Transitions — 简单过渡

Unity 6 UITK 原生支持的 CSS 式过渡，适合 hover/focus/state 切换：

```css
.my-panel {
    opacity: 0;
    transition-duration: 0.3s;
    transition-property: opacity, translate;
    transition-timing-function: ease-out;
}
.my-panel.visible {
    opacity: 1;
}
```

可动画属性：`opacity`、`scale`、`rotate`、`translate`、`width/height`、`color`、`background-color`

不可动画：`display`（离散）、`visibility`（离散）

### 2. Third-Party Tween Engine — 复杂序列

推荐 [UI Toolkit: Tween Engine](https://assetstore.unity.com/packages/tools/utilities/ui-toolkit-tween-engine-365906)：

- 30+ 可动画属性
- 32 easing curves
- 序列/回调支持
- 预设动画（fade, slide, shake, pop）

```csharp
element.TweenScale(new Vector2(1.2f, 1.2f), 0.3f)
       .SetEase(Ease.OutBack)
       .OnComplete(() => onOpen?.Invoke());
```

### 3. Presenter 驱动动画 — 复杂交互

对于「tooltip 开 tooltip、面板叠面板」场景，Presenter 层控制动画时序：

```csharp
public class TooltipPresenter
{
    public async void ShowTooltip(VisualElement anchor, TooltipData data)
    {
        var tooltip = CreateTooltip(data);
        tooltip.AddToClassList("tooltip-enter");
        parent.Add(tooltip);

        await AwaitTransition(tooltip);

        if (data.HasSubTooltip)
            ShowTooltip(tooltip, data.SubTooltip);
    }
}
```

## 原则

- **简单过渡** → USS transitions
- **入场/退场序列** → Tween Engine
- **嵌套弹窗、条件动画** → Presenter 代码控制
- **动画不绑 data binding** — MVVM binding 管状态，Presenter 管动画
