# Layout & Spacing System – Component Showcase

本规范强制执行 CSS 布局和间距系统。生成任何 HTML/CSS 页面、组件展示或 UI 原型时必须遵循。

## 1. Global Canvas

- 所有页面内容包裹在容器内：
  ```css
  max-width: 1280px;
  margin: 0 auto;
  padding: var(--space-2xl); /* 48px on all sides */
  ```
- body 无默认 margin（reset: `body { margin: 0; }`）。
- 页面使用 `<header>`（sticky top, full-width, 内部容器居中）、`<main>` 和 `<footer>`。
- Sidebar（如存在）：固定宽度 260px，padding 为 `var(--space-lg)`。
- 主内容区域填充剩余空间，必要时可滚动。

## 2. Spacing Scale（8-point grid，Token 化）

使用 CSS custom properties；禁止在此 scale 外使用裸 px 值：

```css
--space-xs: 4px;    /* 极少使用，用于紧凑装饰 */
--space-sm: 8px;
--space-md: 16px;
--space-lg: 24px;
--space-xl: 32px;
--space-2xl: 48px;
--space-3xl: 64px;
```

- 同级块间距：`margin-bottom: var(--space-lg)`
- Section 间距：`margin-bottom: var(--space-2xl)`
- Card 内边距：`var(--space-lg)` 四周
- Card 网格间距：`var(--space-xl)`
- 行内间距（如按钮组）：`var(--space-sm)`
- **任何元素**的 margin/padding 不得使用此 scale 以外的值

## 3. Typography Defaults

```css
body {
  font-family: system-ui, sans-serif;
  line-height: 1.6;
}
h1, h2, h3, h4, h5, h6 {
  line-height: 1.3;
  margin-top: 0;
  margin-bottom: var(--space-md);
}
p {
  margin-bottom: var(--space-md);
}
```

- 小标签/说明文字：`font-size: 0.875rem; color: #666;`
- 任何文字不得触碰容器边缘——由 padding 保证

## 4. Component Card Specification

每个组件预览必须放入以下 Card 容器：

```css
.component-card {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  padding: var(--space-lg);
  background: #fff;
  display: flex;
  flex-direction: column;
}
```

Card 内部结构（自上而下）：
- **Demo 区域**：`min-height: 120px; display: flex; align-items: center; justify-content: center; padding: var(--space-md); background: #f9fafb; border-radius: 6px; margin-bottom: var(--space-md);`
- **组件名称**：`<h3>`，`margin-bottom: var(--space-xs)`
- **简述**：`<p>`，`font-size: 0.875rem; color: #4b5563;`
- **属性表（可选）**：干净表格，collapse borders，cell padding `var(--space-sm)`

Card 网格布局：
```css
.card-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: var(--space-xl);
}
```
同一行 Card 必须等高（grid 自动保证）。

## 5. Section Structure

每个组件分类是一个 `<section>`，内部包含：
- Section 标题：`<h2>`，`margin-bottom: var(--space-lg)`
- 可选描述段落
- Card grid 容器

```html
<section>
  <h2>分类名称</h2>
  <p>可选描述</p>
  <div class="card-grid">
    <!-- cards -->
  </div>
</section>
```

## 6. CSS Reset & Box Sizing

**必须**在所有页面开头包含：

```css
*,
*::before,
*::after {
  box-sizing: border-box;
}
body {
  margin: 0;
  padding: 0;
}
```

## 7. Framework Overrides（Tailwind / Bootstrap）

- **Tailwind**：使用 `container`、`mx-auto`、`px-12`、`gap-8`、`p-6` 等映射到 8-point scale 的类。禁止使用任意值如 `p-[14px]`；只使用匹配 spacing scale 的预定义工具类。
- **Bootstrap**：使用 `container`、`row`、`col` 配合 `g-4`（24px gap），Card 用 `p-4`。不添加额外的自定义 margin。
- **两者都**严格遵循 section margin-bottom 和 card grid gap 规范，框架默认值冲突时覆盖。

## 8. Content Overflow Protection

- 代码块或长字符串：包裹在 `<pre>` 中，设置 `overflow-x: auto; max-width: 100%;`
- **禁止**任何元素溢出容器导致水平挤压

## 9. Anti-Squish Rule（不可协商）

任意两个视觉元素之间必须有刻意的间距 token。如果无法确定，默认添加 `margin-bottom: var(--space-md)`。

## 10. Quick Reference — 默认值速查

| 场景 | Token | 值 |
|------|-------|-----|
| 页面容器 padding | `--space-2xl` | 48px |
| Section 间距 | `--space-2xl` | 48px |
| 同级块间距 | `--space-lg` | 24px |
| Card padding | `--space-lg` | 24px |
| Card grid gap | `--space-xl` | 32px |
| 行内元素间距 | `--space-sm` | 8px |
| 标题下间距 | `--space-md` | 16px |
| 段落间距 | `--space-md` | 16px |
| Demo 区 padding | `--space-md` | 16px |
