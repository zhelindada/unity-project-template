# Layout — 生成

## VerticalLayoutGroup

```bash
add_component VerticalLayoutGroup
set_component_property VerticalLayoutGroup spacing = "4"
set_component_property VerticalLayoutGroup childAlignment = "UpperLeft"
set_component_property VerticalLayoutGroup childForceExpandWidth = "true"
set_component_property VerticalLayoutGroup childForceExpandHeight = "false"
```

## LayoutElement（给子元素加）

```bash
add_component LayoutElement
set_component_property LayoutElement preferredHeight = "20"
```

## ContentSizeFitter

```bash
add_component ContentSizeFitter
set_component_property ContentSizeFitter verticalFit = "PreferredSize"
```
