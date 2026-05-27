# ScrollView — 生成

```
create_ui_element(type="ScrollView", name="MyScroll", parentPath="Parent")
```

## Content 配置

```bash
# Content 必须有这些组件
add_component Content VerticalLayoutGroup
add_component Content ContentSizeFitter

# ContentSizeFitter: 高度自适应
set_component_property ContentSizeFitter verticalFit = "PreferredSize"

# Content RectTransform: 顶部锚点（关键！）
set_component_property RectTransform anchorMin = "0,1"
set_component_property RectTransform anchorMax = "1,1"
set_component_property RectTransform pivot = "0,1"
set_component_property RectTransform sizeDelta = "0,0"
```

## ScrollRect 参数

```bash
set_component_property ScrollRect vertical = "true"
set_component_property ScrollRect horizontal = "false"
```
