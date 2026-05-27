# TMP_InputField — 生成

## 创建

```
create_ui_element(type="TMP_InputField", name="MyInput", parentPath="Parent")
```

自动生成完整层级和引用。

## 配置

```bash
set_component_property TMP_InputField contentType = "Standard"
set_component_property TMP_InputField characterLimit = "20"
set_component_property TextMeshProUGUI text = ""
set_component_property TextMeshProUGUI text = "请输入..."   # Placeholder
```

## 检查

```
□ TextArea、Placeholder、Text 子节点存在
□ TextComponent → Text TMP
□ Placeholder → Placeholder RT
□ TextArea 有 RectMask2D 或 Mask
□ Interactable = true
```
