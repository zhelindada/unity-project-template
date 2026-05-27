# Patterns — 生成

每种模式的具体 MCP 工具调用步骤。

## modal

```bash
# 遮罩
create_ui_element(type="Image", name="Overlay", parentPath="Canvas", color="0,0,0,0.5")
set_component_property RectTransform anchorMin="0,0" anchorMax="1,1"
set_component_property Image raycastTarget="true"

# 内容
create_ui_element(type="Panel", name="ModalContent", parentPath="Canvas", color="0.1,0.1,0.12,0.95")
set_component_property RectTransform anchorMin="0.5,0.5" anchorMax="0.5,0.5" pivot="0.5,0.5"
add_component VerticalLayoutGroup
```

## scrollableList

```bash
create_ui_element(type="ScrollView", name="SongList", parentPath="Canvas")
# Content 上
add_component VerticalLayoutGroup
add_component ContentSizeFitter
set_component_property ContentSizeFitter verticalFit="PreferredSize"
set_component_property RectTransform anchorMin="0,1" anchorMax="1,1" pivot="0,1"
```

## sidebar

```bash
create_ui_element(type="Panel", name="Sidebar", parentPath="Canvas", color="0.1,0.1,0.12,0.95")
set_component_property RectTransform anchorMin="0,0" anchorMax="0,1" sizeDelta="250,0"
add_component VerticalLayoutGroup
```

## contentArea

```bash
create_ui_element(type="Panel", name="ContentArea", parentPath="Canvas", color="0,0,0,0")
set_component_property RectTransform anchorMin="0,0" anchorMax="1,1" sizeDelta="-260,0"
```
