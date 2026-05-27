# Toggle — 生成

## 创建

```
create_ui_element(type="Toggle", name="MyToggle", parentPath="Parent")
```

自动生成完整层级：Background + Checkmark + Label，Graphic 已设。

## 配置

```bash
set_component_property Toggle isOn = "false"
set_component_property TextMeshProUGUI text = "Enable Sound"
```

## ToggleGroup

```bash
# 在父节点上加 ToggleGroup
add_component ToggleGroup

# 每个 Toggle 的 Group 引用设到父节点
set_object_reference targetGameObject="Toggle1" componentType="Toggle" fieldName="group" sourceGameObject="Parent" sourceType="ToggleGroup"
```

## 检查清单

```
□ Background 和 Checkmark 子节点存在
□ Toggle.Graphic → Checkmark Image
□ Interactable = true
□ ToggleGroup（如需要）已关联
```
