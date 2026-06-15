---
name: unity-check-ui
description: 检查 Unity 场景中 UI 组件的规范性和 4K 适配性 —— 验证 Canvas 配置、元素尺寸、字体大小是否符合 4K（3840×2160）标准。Use when 需要审查场景 UI 是否适配 4K 显示器、检查 UI 元素尺寸规范、或在发布前做 UI 质量审计。
---

检查 Unity 场景 UI 组件是否符合 4K 分辨率规范。3 步流程：收集信息 → 逐项检查 → 输出报告。

**Tier:** POWERFUL
**Category:** Unity / UI Systems
**Tags:** Unity, UGUI, 4K, UI audit, Canvas, resolution, quality assurance

## 工作流程

### 阶段 0：准备

1. 调用 `get_scene_hierarchy` 获取完整场景层级
2. 确认 Canvas 存在，不存在则报错
3. 调用 `get_gameobject_info` 检查 Canvas 及其 CanvasScaler 组件配置
4. 读取 `references/4k-ui-spec.md` 获取当前 4K 规范标准

### 阶段 1：检查 Canvas 基准配置

| 检查项 | 属性 | 预期值 | 工具 |
|--------|------|--------|------|
| Reference Resolution | `m_ReferenceResolution` | `"3840,2160"` | `get_component_inspector_properties` CanvasScaler |
| Screen Match Mode | `m_ScreenMatchMode` | Expand (0) 或 Shrink (1) | 同上 |
| Match | `m_MatchWidthOrHeight` | `0.5`（居中匹配） | 同上 |
| Render Mode | - | ScreenSpaceOverlay | `get_gameobject_info` |

### 阶段 2：遍历 UI 元素检查尺寸

对每个 UI 节点，按元素类型检查 `RectTransform` 和关键组件：

1. 调用 `get_gameobject_components` 列出组件
2. 调用 `get_component_inspector_properties` 读取 RectTransform 属性
3. 根据元素角色匹配 `references/4k-ui-spec.md` 中的规范

**检查优先级**：
- 🔴 严重：Canvas 分辨率不匹配、标题/正文字号严重偏离
- 🟡 警告：间距/边距偏离推荐值、元素尺寸接近但不精确
- 🔵 建议：可选优化项（如锚点模式选择）

### 阶段 3：输出检查报告

按以下格式汇总：

```
## UI 4K 适配检查报告

### Canvas 配置
- Reference Resolution: [实际值] → [状态]
- Screen Match Mode: [实际值] → [状态]

### 文本元素字号
| 路径 | 角色 | 实际字号 | 预期范围 | 状态 |
|------|------|----------|----------|------|

### 元素尺寸
| 路径 | 类型 | 实际尺寸 | 预期尺寸 | 状态 |
|------|------|----------|----------|------|

### 间距与布局
| 路径 | 检查项 | 实际值 | 预期值 | 状态 |

### 总结
- 🔴 严重: N 项
- 🟡 警告: N 项
- 🔵 建议: N 项
```

## 批量操作

检查多个元素时，用 `batch_execute` 打包独立的 `get_gameobject_components` / `get_component_inspector_properties` 调用。

## 常见问题

| 问题 | 原因 | 修复 |
|------|------|------|
| 4K 下 UI 太小 | Reference Resolution 不是 3840×2160 | 修改 CanvasScaler |
| 文字模糊 | 字号在 4K 下不足 | 参照 spec 增大字号 |
| 按钮点不到 | 按钮尺寸在 4K 下过小 | 最小高度 60px |

## 资源

详细 4K 规范标准见 `references/4k-ui-spec.md`，包含各元素类型的推荐尺寸、字号等级和布局间距表。
