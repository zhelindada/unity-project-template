# TMP_Dropdown — 生成

```
create_ui_element(type="TMP_Dropdown", name="MyDropdown", parentPath="Parent")
```

自动生成完整层级。设置引用：

```bash
set_object_reference ... fieldName="template" sourceType="RectTransform"
set_object_reference ... fieldName="captionText" sourceType="TextMeshProUGUI"
set_object_reference ... fieldName="itemText" sourceType="TextMeshProUGUI"
```

Template 初始设为 inactive。
