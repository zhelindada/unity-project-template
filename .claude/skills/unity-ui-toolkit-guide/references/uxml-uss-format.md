# UXML / USS 规范

## Unity 6 UXML 格式要点

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements"
         xmlns:uie="UnityEditor.UIElements"
         xsi:schemaLocation="UnityEngine.UIElements ../UISchema/UIElements.xsd">

    <!-- 用 name 属性标识元素，不用 id -->
    <ui:VisualElement name="root-container" class="fullscreen">

        <!-- Label：用 text 属性设初始值 -->
        <ui:Label name="title" class="heading-large" text="Title" />

        <!-- Button：文字在 text 属性 -->
        <ui:Button name="confirm-btn" class="btn-primary" text="Confirm" />

    </ui:VisualElement>
</ui:UXML>
```

规则：
- 根元素必须是 `<ui:UXML>`
- 用 `name` 标识元素（USS 中用 `#name` 选择）
- 用 `class` 设置样式类（USS 中用 `.class` 选择）
- 不要在 UXML 中硬编码文本 — 用 ViewModel binding 或代码赋值

## USS 格式要点

```css
/* 全局变量 */
:root {
    --color-primary: rgb(30, 144, 255);
    --color-bg-dark: rgb(20, 20, 20);
    --spacing-md: 12px;
    --font-size-lg: 24px;
}

/* class 选择器 */
.fullscreen {
    width: 100%;
    height: 100%;
    flex-direction: column;
}

/* name 选择器 */
#title {
    font-size: var(--font-size-lg);
    -unity-font-style: bold;
    color: var(--color-primary);
}

/* 状态伪类 */
.btn-primary:hover {
    background-color: rgb(50, 160, 255);
}

/* 支持 flexbox */
.horizontal-layout {
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
}
```

规则：
- 全局颜色/间距用 CSS 变量
- `flex-direction` 默认 `column`（与 HTML 的 `row` 不同）
- 字体粗细用 `-unity-font-style: bold`（不是 `font-weight`）
- `position: absolute` 在 UITK 中有效，但 `fixed`/`sticky` 不支持

## 命名约定

| 类型 | 约定 | 示例 |
|------|------|------|
| Panel 容器 | `{name}-panel` | `inventory-panel` |
| Button | `{action}-btn` | `close-btn`, `confirm-btn` |
| Label | `{content}-label` | `title-label`, `hp-label` |
| 列表/滚动 | `{name}-list` | `item-list` |
| 模板元素 | `{name}-template` | `item-slot-template` |

## 项目规范文档

Skill 会先查找以下位置的设计规范文件并遵循：

1. `Assets/Designs/ui-conventions.md`
2. `Assets/{ProjectName}/Designs/ui-conventions.md`

规范文件内容由 project setup 工作流生成，skill 不自行创建。
