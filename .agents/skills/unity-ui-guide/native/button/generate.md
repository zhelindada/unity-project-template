# Button — 生成

## 创建

```
create_ui_element(type="Button", name="MyButton", parentPath="Parent")
```
自动生成 Image + Button + 子 Text(TMP)，Target Graphic 已设。

## 配置

```bash
# 文字
set_component_property TextMeshProUGUI text = "Start"
set_component_property TextMeshProUGUI fontSize = "22"

# 颜色
set_component_property Button colors_normalColor = "1,1,1,1"
set_component_property Button colors_highlightedColor = "0.9,0.9,0.9,1"

# 基础属性
set_component_property Button interactable = "true"
set_component_property Button transition = "ColorTint"
```

## 尺寸

```
set_component_property RectTransform sizeDelta = "200,50"
```

## OnClick 绑定（在脚本中）

```csharp
_myButton.onClick.AddListener(OnButtonClicked);
```

## 检查清单

```
□ 子 Text(TMP) 存在
□ Button.TargetGraphic 已设
□ Interactable = true
□ Image.RaycastTarget = true
```
