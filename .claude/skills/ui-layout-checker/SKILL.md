---
name: ui-layout-checker
description: 对照 Layout & Spacing System 规范检查 HTML/CSS 页面——验证 spacing scale、Card 规格、Section 结构、CSS Reset、Overflow 保护等全部 9 条规则。Use when 需要审查页面是否符合 8-point grid 规范、检查间距是否使用 token、或发布前做 UI 布局质量审计。
---

对照 `.claude/conventions/layout-spacing-system.md` 规范，检查 HTML/CSS 文件是否合规。3 步流程：读取规范 → 逐项检查 → 输出报告。

**Tier:** POWERFUL
**Category:** UI / Quality Assurance
**Tags:** HTML, CSS, layout, spacing, audit, 8-point-grid, component-card, quality

## 前置条件

- 规范文件 `.claude/conventions/layout-spacing-system.md` 必须存在
- 指定要检查的 HTML/CSS 文件路径（可以是多个文件）

## 工作流程

### 阶段 0：加载规则集

1. 读取 `.claude/conventions/layout-spacing-system.md` 并提取全部 9 条规则
2. 读取用户指定的 HTML/CSS 文件内容
3. 解析 HTML 结构（元素层级、class 名、inline style）和 CSS 规则（选择器 + 属性）

### 阶段 1：逐规则检查

按以下 9 条规则顺序检查：

#### Rule 1: Global Canvas

| 检查项 | 方法 | 预期 |
|--------|------|------|
| 页面容器 | 检查主容器是否有 `max-width: 1280px` | 必须 |
| 居中 | 检查 `margin: 0 auto` 或等效 | 必须 |
| padding token | 检查 padding 是否为 `var(--space-2xl)` 或 48px | 必须 |
| body reset | 检查 `body { margin: 0 }` | 必须 |
| header | 检查是否有 `<header>`（sticky top） | 建议 |
| main | 检查是否有 `<main>` 包裹主内容 | 必须 |
| footer | 检查是否有 `<footer>` | 建议 |
| sidebar 宽度 | 如有 sidebar，宽度是否为 260px | 按实际情况 |

#### Rule 2: Spacing Scale（8-point Grid）

| 检查项 | 方法 | 预期 |
|--------|------|------|
| Token 定义 | 扫描 `:root` 或全局 CSS 是否有 `--space-` 变量 | 必须有 7 个 |
| 裸 px 出现 | grep 所有 `margin`/`padding`/`gap` 值，检测是否使用了非 token 值的 px | 仅允许 `1px`（border）和 token 值 |
| 非 scale 值 | 检查任意 margin/padding 是否为 4/8/16/24/32/48/64 以外的 px 值 | 出现即违规 |
| Card padding | Card 容器 padding 是否为 24px 或 `var(--space-lg)` | 必须 |

#### Rule 3: Typography Defaults

| 检查项 | 方法 | 预期 |
|--------|------|------|
| body font | `font-family` 是否包含 `system-ui` 或无衬线 | 必须 |
| body line-height | `line-height` 是否为 1.6 或等效 | 必须 |
| heading line-height | 标题 `line-height` 是否为 1.3 或等效 | 必须 |
| heading margin | 标题是否有 `margin-top: 0; margin-bottom: var(--space-md)` | 建议 |

#### Rule 4: Component Card Specification

| 检查项 | 方法 | 预期 |
|--------|------|------|
| Card 存在 | 检查是否有 `.component-card` 或等效 card 容器 | 按实际情况 |
| border | 是否为 `1px solid #e5e7eb` | 必须 |
| border-radius | 是否为 8px | 必须 |
| box-shadow | 是否有 `0 1px 3px rgba(0,0,0,0.06)` | 必须 |
| Card padding | 是否为 `var(--space-lg)` | 必须 |
| Card background | 是否为 `#fff` | 必须 |
| Demo 区 | Card 内是否有 demo 区域（min-height 120px, 居中, 背景 #f9fafb） | 必须 |
| Grid 布局 | 是否使用 `repeat(auto-fill, minmax(300px, 1fr))` | 必须 |
| Grid gap | 是否为 `var(--space-xl)` | 必须 |

#### Rule 5: Section Structure

| 检查项 | 方法 | 预期 |
|--------|------|------|
| Section 标签 | 是否用 `<section>` 包裹分类 | 必须 |
| Section 标题 h2 | 是否用 `<h2>` 作为 section 标题 | 必须 |
| Section mb | `margin-bottom` 是否为 `var(--space-2xl)` | 必须 |

#### Rule 6: CSS Reset & Box Sizing

| 检查项 | 方法 | 预期 |
|--------|------|------|
| box-sizing | 是否有全局 `*, *::before, *::after { box-sizing: border-box }` | 必须 |
| body margin/padding | 是否有 `body { margin: 0; padding: 0 }` | 必须 |

#### Rule 7: Framework Overrides

| 检查项 | 方法 | 预期 |
|--------|------|------|
| 裸任意值 | 检测是否有 `p-[14px]` / `m-[13px]` 等 Tailwind 任意值 | 出现即违规 |
| Bootstrap 覆盖 | Bootstrap 项目中是否有冲突的额外 margin | 发现即报告 |

#### Rule 8: Content Overflow Protection

| 检查项 | 方法 | 预期 |
|--------|------|------|
| pre overflow | `<pre>` 或代码块是否有 `overflow-x: auto; max-width: 100%` | 必须 |
| 无溢出风险 | 检查是否有固定宽度超出容器的元素 | 发现即违规 |

#### Rule 9: Anti-Squish Rule

| 检查项 | 方法 | 预期 |
|--------|------|------|
| 紧贴元素 | 相邻块元素之间是否有间距（margin/padding/gap >= 4px） | 必须 |
| 文字贴边 | 文字是否直接触及容器边缘（无 padding 容器） | 违规 |

### 阶段 2：违规分级

| 级别 | 标记 | 含义 |
|------|------|------|
| 🔴 严重 | MUST 规则违反 | 间距使用裸 px、缺失 box-sizing reset、Card 无 padding |
| 🟡 警告 | SHOULD 规则违反 | 间距 token 值正确但用了 px 而非 var()、缺少建议的 header/footer |
| 🔵 建议 | MAY/建议 | 可选的改进项 |

### 阶段 3：输出检查报告

按以下格式输出报告：

```
# UI Layout 规范检查报告

**检查文件**: [文件列表]
**检查时间**: [时间]
**规范版本**: Layout & Spacing System v1

---

## 🔴 严重违规（MUST FIX）

| # | 规则 | 位置 | 问题 | 修复建议 |
|---|------|------|------|----------|
| 1 | Rule 2: Spacing Scale | style.css:45 | `margin: 15px` 非 token 值 | 改为 `margin: var(--space-md)` (16px) |

## 🟡 警告（SHOULD FIX）

| # | 规则 | 位置 | 问题 | 修复建议 |
|---|------|------|------|----------|

## 🔵 建议（NICE TO HAVE）

| # | 规则 | 位置 | 问题 | 修复建议 |
|---|------|------|------|----------|

---

## 违规汇总

| 规则 | 🔴 严重 | 🟡 警告 | 🔵 建议 |
|------|----------|----------|----------|
| Rule 1: Global Canvas | N | N | N |
| Rule 2: Spacing Scale | N | N | N |
| Rule 3: Typography | N | N | N |
| Rule 4: Card Spec | N | N | N |
| Rule 5: Section Structure | N | N | N |
| Rule 6: CSS Reset | N | N | N |
| Rule 7: Framework | N | N | N |
| Rule 8: Overflow | N | N | N |
| Rule 9: Anti-Squish | N | N | N |

---

## 批量修复建议

[按规则分组列出所有需要修改的代码片段及修复后版本]
```

## 执行规则

- 必须读取完整文件后再检查，不可凭印象
- 每个违规必须标注精确位置（文件名 + 行号）
- 间距值不在 scale 内 → 建议最近 token 值（如 15px → `var(--space-md)` 即 16px）
- 缺失 token 定义 → 在报告顶部给出 `:root` 代码块
- 批量修复建议需直接提供可粘贴的 CSS 代码
- 中文输出 + 英文术语

## 快速触发

- "检查 UI 布局规范"
- "检查这个页面的 spacing"
- "对照 layout convention 审查"
- "ui layout audit"
